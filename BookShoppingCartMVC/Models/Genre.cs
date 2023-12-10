using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCartMVC.Models
{
    [Table("Genre")]

    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [ValidateNever]
        public string GenreName { get; set; }
        [ValidateNever]

        //One Genre can have multiple Books
        public List<Book> Books { get; set; }
    }
}
