using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Data.Models
{
    [PrimaryKey(nameof(OperatorId), nameof(WeaponId))]
    public class OperatorWeapon
    {
        [ForeignKey(nameof(Operator))]
        public int OperatorId { get; set; }

        [Required]
        public virtual Operator Operator { get; set; } = null!;

        [ForeignKey(nameof(Weapon))]
        public int WeaponId { get; set; }

        [Required]
        public virtual Weapon Weapon { get; set; } = null!;
    }
}
