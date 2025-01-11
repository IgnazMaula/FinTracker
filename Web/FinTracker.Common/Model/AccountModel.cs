using FinTracker.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Model
{
    public class AccountModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public string? Institution { get; set; }
        public string? Description { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }

    }
}
