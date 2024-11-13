using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }

        [Required]
        public string AdminFname { get; set; }

        public string? AdminLname { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string AdminEmail { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$")]
        public string AdminPasscode { get; set; }

        [ForeignKey("MasterAdmin")]
        public int? MasterAdminId { get; set; }
        public virtual Admin MasterAdmin { get; set; }
        public List<Admin>? ManagedAdmins { get; set; }
    }
}
