using BookShoppingCartMVC.Data;
using BookShoppingCartMVC.Models;
using BookShoppingCartMVC.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShoppingCartMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomerepository _homerepository;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IHomerepository homerepository, ApplicationDbContext context)
        {
            _logger = logger;
            _homerepository = homerepository;
            this._context = context;
        }

        //sterm for search
        public async Task<IActionResult> Index(string sterm="", int genreId = 0)
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}