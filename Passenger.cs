using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airline_Resevation_System.Models
{
    public class Passenger
    {
        [Key]
        public Guid BookingReference { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Age should be positive integer.")]
        public int Age { get; set; }

        [Display(Name="FlightNumber")]
        public virtual string FlightNumber { get; set; }

        [ForeignKey("FlightNumber")]
        public virtual Flight Flights { get; set; }
    }
}
