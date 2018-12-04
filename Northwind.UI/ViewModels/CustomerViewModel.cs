using System.ComponentModel.DataAnnotations;

namespace Northwind.UI.ViewModels
{
    public class CustomerViewModel
    {
        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

        [Display(Name = "Company Name")]
        [StringLength(30)]
        public string CompanyName { get; set; }
        [Display(Name = "Contact Name")]
        [StringLength(30)]
        public string ContactName { get; set; }
        [StringLength(30)]
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        [StringLength(15)]
        public string City { get; set; }
        [StringLength(15)]
        public string Region { get; set; }
        [StringLength(60)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Post Code Required")]
        [Display(Name = "Post Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }
        [StringLength(24)]
        public string Country { get; set; }
        public string Phone { get; set; }
        [StringLength(24)]
        public string Fax { get; set; }

    }
}
