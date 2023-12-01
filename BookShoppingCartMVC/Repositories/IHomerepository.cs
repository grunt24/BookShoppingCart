using BookShoppingCartMVC.Models;

namespace BookShoppingCartMVC
{
    public interface IHomerepository
    {
        Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0);
        Task<IEnumerable<Genre>> GetGenres();
    }
}