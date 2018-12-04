using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Data.Entities;

namespace Northwind.UI.Repository
{
    public interface INorthwindRepository
    {
        IQueryable<Customer> GetCustomers();
        Customer GetCustomer(string CustomerId);
        IQueryable<Order> GetOrders(string CustomerId);
        Product GetProduct(string ProductId);
        IList<Product> GetOrderProducts(int OrderId);
        Task<bool> UpdateCustomerAsync(Customer customer);
    }
}