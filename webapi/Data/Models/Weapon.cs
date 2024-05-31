using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class Weapon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Photo { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        public string? Category { get; set; } 

        public ICollection<OperatorWeapon> OperatorWeapons { get; set; } = new HashSet<OperatorWeapon>();
    }
}
