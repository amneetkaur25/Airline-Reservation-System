using Airline_Resevation_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Airline_Resevation_System.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AirlineDbContext _context;

        public HomeController(AirlineDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {
            // To show only those flights whose departure time has not passed .
            var currentTime = DateTime.Now;
            var flightDetails = _context.Flights.Where(f => f.DepartureTime > currentTime);
           // var flightDetails =  _context.Flights.ToList();
           // return View(flightDetails);
            return View( await PaginatedList<Flight>.CreateAsync(flightDetails, pageNumber, pageSize));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}