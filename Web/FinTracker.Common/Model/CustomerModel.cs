using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Model
{
    public class CustomerModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Gender { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [Required]
        public string? Address { get; set; }
    }
}
