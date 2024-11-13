using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Models
{
    [PrimaryKey(nameof(BorrowId), nameof(UserId), nameof(BookId))]
    public class Borrow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BorrowId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        public DateTime? ActualReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;

        public int? Rating { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
