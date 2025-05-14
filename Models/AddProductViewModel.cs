using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AgriApp.Models
{
    public class AddProductViewModel
    {

        public string Name { get; set; }

        public string Category { get; set; }

    }
}
