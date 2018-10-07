using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PaypalIntegration.DTO
{
    public class ShippingDetailsDTO
    {
        [Required]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "Please enter valid First Name with 3-75 characters")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{10,11}", ErrorMessage = "Please enter valid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }
        public string Street { get; set; }
        public string LandMark { get; set; }
        public int Country { get; set; }
        public string UserId { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public ItemDetails ItemDetails { get; set; }
    }
}