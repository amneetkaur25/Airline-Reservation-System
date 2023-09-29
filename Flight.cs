using System.ComponentModel.DataAnnotations;

namespace Airline_Resevation_System.Models
{
    public class Flight
    {
        [Key]
        public string FlightNumber { get; set; }
        [Required]
        public string DepartureCity { get; set; }
        [Required]
        public string ArrivalCity { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public int AvailableSeats { get; set; }


    }
}
