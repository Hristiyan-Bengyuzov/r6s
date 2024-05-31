using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace webapi.Data.Models
{
    [PrimaryKey(nameof(OperatorId), nameof(GadgetId))]
    public class OperatorGadget
    {
        [ForeignKey(nameof(Operator))]
        public int OperatorId { get; set; }

        [Required]
        public virtual Operator Operator { get; set; } = null!;

        [ForeignKey(nameof(Gadget))]
        public int GadgetId { get; set; }

        [Required]
        public virtual Gadget Gadget { get; set; } = null!;
    }
}
