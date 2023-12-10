using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCartMVC.Models.DTOs
{
    public class BookViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? BookName { get; set; }
        [Required]
        public string? AuthorName { get; set; }
        public double Price { get; set; }
        [Required]
        [ValidateNever]
        [NotMapped]
        public IFormFile ImageUrl { get; set; }
        [Required]
        [ValidateNever]
        public int GenreId { get; set; }
        [ValidateNever]
        public Genre Genre { get; set; }
        [ValidateNever]

        public List<OrderDetail> OrderDetail { get; set; }
        [ValidateNever]

        public List<CartDetail> CartDetail { get; set; }
        [ValidateNever]
        public string GenreName { get; set; }

    }
}
