using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CatId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CatName { get; set; }

        public int NumOfBooks { get; set; } = 0;

        public virtual ICollection<Book> Books { get; set; }
    }
}
