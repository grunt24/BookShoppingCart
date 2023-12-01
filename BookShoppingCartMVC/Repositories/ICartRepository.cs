using BookShoppingCartMVC.Models;

namespace BookShoppingCartMVC.Repositories
{
    public interface ICartRepository
    {
        // Add Item
        Task<int> AddItem(int bookId, int qty);
        Task<int> RemoveItem(int bookId);
        Task<ShoppingCart> GetUserCart();
        Task<ShoppingCart> GetCart(string userId);
        Task<int> GetCartItemCount(string userId = "");
        Task<bool> DoCheckout();


    }
}