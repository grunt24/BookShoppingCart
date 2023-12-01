using BookShoppingCartMVC.Data;
using BookShoppingCartMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookShoppingCartMVC.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this._contextAccessor = httpContextAccessor;
            this._userManager = userManager;
        }
        // Add Item
        public async Task<int> AddItem(int bookId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged-in");
                }
                var cart = await GetCart(userId);

                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _context.ShoppingCarts.Add(cart);

                }
                _context.SaveChanges();
                //cart detail section
                var cartItem = _context.CartDetails
                    .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem != null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var book = _context.Books.Find(bookId);
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty, 
                        UnitPrice = book.Price //part 9
                        
                    };
                    _context.CartDetails.Add(cartItem);

                }
                _context.SaveChanges();
                transaction.Commit();

            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        // Remove Item
        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            //use BeginTransaction only when making changes with multiple changes 

            //using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");

                var cart = await GetCart(userId);

                if (cart is null)
                {
                    throw new Exception("Cart is Empty");

                }
                //cart detail section
                //find the cartItrem
                var cartItem = _context.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is null)
                {
                    throw new Exception("No items in cart");
                }
                else if (cartItem.Quantity == 1)
                {
                    _context.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                }
                _context.SaveChanges();
                //transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart>GetUserCart()
        {
            //first find user
            var userId = GetUserId();
            if(userId == null)
            {
                throw new Exception("Invalid userId");
            }

            var shoppingCart = await _context.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Book)
                .ThenInclude(a => a.Genre)
                .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            var data = await (from cart in _context.ShoppingCarts
                              join cartDetail in _context.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == userId
                              select new { cartDetail.Id }
                              ).ToListAsync();

            return data.Count;
        }

        public async Task<bool> DoCheckout()
        {
            var userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _context.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1//pending
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();

                // removing the cartdetails
                _context.CartDetails.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }
    }

}
