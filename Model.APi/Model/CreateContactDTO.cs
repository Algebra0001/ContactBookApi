using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.APi.Model
{
    public class CreateContactDTO
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = "";
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; } = "";
       // public string Image { get; set; } = "";
        public string WebSiteUrl { get; set; } = "";
        public string Address { get; set; } = "";
        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = "";
        public string Emails { get; set; } = "";
    }
}
