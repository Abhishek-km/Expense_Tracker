using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        [Required]
        public string Description { get; set; }

        [Key]
        public int ID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Category { get; set; }

        [Required]
        public string Date { get; set; }
    }
}
