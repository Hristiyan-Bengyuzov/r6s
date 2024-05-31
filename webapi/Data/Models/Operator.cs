using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class Operator
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Photo { get; set; } = null!;

        [Required]
        public string Icon { get; set; } = null!;

        [Required]
        public string Side { get; set; } = null!;

        public int Health { get; set; }
        public int Speed { get; set; }
        public int Difficulty { get; set; }

        public string? Ability { get; set; }
        public string? AbilityPhoto { get; set; }

        public ICollection<OperatorWeapon> OperatorWeapons { get; set; } = new HashSet<OperatorWeapon>();
        public ICollection<OperatorGadget> OperatorGadgets { get; set; } = new HashSet<OperatorGadget>();
    }
}
