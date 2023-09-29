using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airline_Resevation_System.Models;
using Microsoft.AspNetCore.Authorization;

namespace Airline_Resevation_System.Controllers
{
    [Authorize(Roles ="User")]
    public class PassengersController : Controller
    {
        private readonly AirlineDbContext _context;

        public PassengersController(AirlineDbContext context)
        {
            _context = context;
        }

        // GET: Passengers
        public async Task<IActionResult> Index()
        {
            var airlineDbContext = _context.Passengers.Include(p => p.Flights);
            return View(await airlineDbContext.ToListAsync());
        }

        // GET: Passengers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Passengers == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers
                .Include(p => p.Flights)
                .FirstOrDefaultAsync(m => m.BookingReference.ToString() == id);
            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // GET: Passengers/Create
        public IActionResult Create()
        {
            // pass only those flights whose departure has not been passed.ie. Future flights only 
            var currentTime = DateTime.Now;
            var futureFlights = _context.Flights.Where(f => f.DepartureTime > currentTime);
            ViewData["FlightNumber"] = new SelectList(futureFlights, "FlightNumber", "FlightNumber");
            return View();
        }

        // POST: Passengers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingReference,Name,Age,FlightNumber")] Passenger passenger)
        {
            // retreive the selected flight 
            var flight = await _context.Flights.SingleOrDefaultAsync(f => f.FlightNumber == passenger.FlightNumber);
            // check if there are available seats or the flight has not departed yet.
                if (flight.AvailableSeats > 0 && flight.DepartureTime >DateTime.Now)
                {
                    passenger.BookingReference = Guid.NewGuid();
                    _context.Add(passenger);
                    await _context.SaveChangesAsync();

                    // decrease available seats for the flight
                    flight.AvailableSeats--;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else if (flight.DepartureTime <= DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Sorry !!! The selected Flight has already departed.");
            }
                else
                {
                ModelState.AddModelError(string.Empty, "Oops!!! No Available Seats for the selected Flight");

                }
            

            ViewData["FlightNumber"] = new SelectList(_context.Flights, "FlightNumber", "FlightNumber", passenger.FlightNumber);
            return View(passenger);
        }

        // GET: Passengers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Passengers == null)
            {
                return NotFound();
            }

            //var passenger = await _context.Passengers.FindAsync(id);
            //if (passenger == null)
            //{
            //    return NotFound();
            //}
            // Assuming 'id' is a Guid
            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(p => p.BookingReference.ToString() == id.ToString()); // Convert Guid to string
            ViewData["FlightNumber"] = new SelectList(_context.Flights, "FlightNumber", "FlightNumber", passenger.FlightNumber);
            return View(passenger);
        }

        // POST: Passengers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookingReference,Name,Age,FlightNumber")] Passenger passenger)
        {
            if (id != passenger.BookingReference.ToString())
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                _context.Update(passenger);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(passenger.BookingReference.ToString()))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            ViewData["FlightNumber"] = new SelectList(_context.Flights, "FlightNumber", "FlightNumber", passenger.FlightNumber);
            return View(passenger);
        }

        // GET: Passengers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Passengers == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers
                .Include(p => p.Flights)
                .FirstOrDefaultAsync(m => m.BookingReference.ToString() == id);
            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // POST: Passengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (_context.Passengers == null)
            {
                return Problem("Entity set 'AirlineDbContext.Passengers'  is null.");
            }
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger != null)
            {
                _context.Passengers.Remove(passenger);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassengerExists(string id)
        {
            return (_context.Passengers?.Any(e => e.BookingReference.ToString() == id)).GetValueOrDefault();
        }
    }
}
