using BookShoppingCartMVC.Models;

namespace BookShoppingCartMVC.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
    }
}