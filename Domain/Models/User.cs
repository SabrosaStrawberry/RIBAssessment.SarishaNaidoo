
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
