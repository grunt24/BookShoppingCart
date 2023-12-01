using BookShoppingCartMVC.Models.DTOs;
using BookShoppingCartMVC.Models;
using BookShoppingCartMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using BookShoppingCartMVC.Data;

namespace BookShoppingCartMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomerepository _homerepository;
        private readonly ApplicationDbContext _context;

        public AdminController(ILogger<HomeController> logger, IHomerepository homerepository, ApplicationDbContext context)
        {
            _logger = logger;
            _homerepository = homerepository;
            _context = context;
        }
        public async Task<IActionResult> Dashboard(string sterm = "", int genreId = 0)
        {

            IEnumerable<Book> books = await _homerepository.GetBooks(sterm, genreId);
            IEnumerable<Genre> genres = await _homerepository.GetGenres();

            //instance for BookDisplayModel

            BookDisplayModel bookModel = new BookDisplayModel
            {
                Books = books,
                Genres = genres,
                STerm = sterm,
                GenreId = genreId

            };


            return View(bookModel);
            //goto index.cshtml change the model into BookDisplayModel
        }

    }
}
