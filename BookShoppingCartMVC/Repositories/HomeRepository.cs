using BookShoppingCartMVC.Data;
using BookShoppingCartMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMVC.Repositories
{
    public class HomeRepository : IHomerepository
    {
        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //To list down the Genres
        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        //This is to get all the Books 
        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();

            var books = await _context.Books
                .Where(book =>
                    string.IsNullOrWhiteSpace(sTerm) ||
                    (book != null && book.BookName.ToLower().StartsWith(sTerm)))
                .Include(book => book.Genre) // Include the Genre related data
                .Select(book => new Book
                {
                    Id = book.Id,
                    Image = book.Image,
                    AuthorName = book.AuthorName,
                    GenreId = book.GenreId,
                    Price = book.Price,
                    GenreName = book.Genre.GenreName, // Access the GenreName from the included Genre
                    BookName = book.BookName
                })
                .ToListAsync();




            //IEnumerable<Book> books = await (from book in _context.Books
            //             join genre in _context.Genres 
            //             on book.GenreId equals genre.Id
            //             where string.IsNullOrWhiteSpace(sTerm) ||(book!=null && book.BookName.ToLower().StartsWith(sTerm))

            //             select new Book
            //             {
            //                 Id = book.Id,
            //                 Image=book.Image,
            //                 AuthorName=book.AuthorName,
            //                 GenreId=book.GenreId,
            //                 Price=book.Price,
            //                 GenreName=genre.GenreName,
            //                 BookName=book.BookName
            //             }
            //             ).ToListAsync();

            if (genreId > 0)
            {
                books = books.Where(a => a.GenreId == genreId).ToList();
            }

            return books;
        }

    }
}
