using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WandererCup.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}

