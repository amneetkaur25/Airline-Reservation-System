using Airline_Resevation_System.Models;
using Airline_Resevation_System.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Airline_Resevation_System.Models.ViewModel.BookingViewModel;

namespace Airline_Resevation_System.Controllers
{
    public class BookingController : Controller
    {
        // GET: BookingController
        private readonly AirlineDbContext _context;

        public BookingController(AirlineDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var bookings = _context.Passengers.Include(p => p.Flights)
                .Select(p => new BookingViewModel
                {
                    BookingReference = p.BookingReference,
                    PassengerName = p.Name,
                    FlightNumber =p.Flights.FlightNumber,
                    DepartureTime = p.Flights.DepartureTime
                }).ToList();
            // Booking is the viewModel
            return View(bookings);
        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
