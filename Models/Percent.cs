using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Percent
    {
        public string Id { get; set; }
        [Required]
        public int? DepositNumber { get; set; }
        [Required]
        public string? DepositName { get; set; }
        [Required]
        public string? InterestRate { get; set; }
        List<Investor> Investors { get; set; } = new();
    }
}
