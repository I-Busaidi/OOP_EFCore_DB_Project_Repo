using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required]
        public string BookName { get; set; }

        [Required]
        public string AuthorName { get; set; }
        public int TotalCopies { get; set; } = 0;
        public int BorrowedCopies { get; set; } = 0;

        [Required]
        public int BorrowPeriod { get; set; }

        [Required]
        public decimal CopyPrice { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CatId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Borrow>? Borrows { get; set; }
    }
}
