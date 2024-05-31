using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class Gadget
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Photo { get; set; } = null!;

        public ICollection<OperatorGadget> OperatorGadgets { get; set; } = new HashSet<OperatorGadget>();
    }
}
