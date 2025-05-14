using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriApp.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string FarmerId { get; set; }

        [ForeignKey("FarmerId")]
        public IdentityUser Farmer { get; set; }

    }
}
