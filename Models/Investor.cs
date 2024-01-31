using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Investor
    {
        public int Id { get; set; }
        [Required]
        public int? DepositNumber { get; set; }
        [Required]
        public string? DepositName { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? DepositAmount { get; set; }
        [Required]
        public DateTime? DepositDate { get; set; }
        [Required]
        public string? InterestRate { get; set; }
        public string? TotalAmount { get; set; }
        public Percent? Percent { get; set; }
    }
}
