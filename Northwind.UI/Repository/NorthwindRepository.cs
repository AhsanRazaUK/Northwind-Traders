using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.UI.Repository
{
    public class NorthwindRepository : INorthwindRepository
    {
        private readonly NorthwindContext context;

        public NorthwindRepository(NorthwindContext context)
        {
            this.context = context;
        }

        public IQueryable<Customer> GetCustomers()
        {
            return context.Customers.AsQueryable();
        }

        public Customer GetCustomer(string CustomerId)
        {
            return context.Customers.Find(CustomerId);
        }
        public IQueryable<Order> GetOrders(string CustomerId)
        {
            return context.Orders
                .Where(o => string.Equals(o.CustomerId, CustomerId, StringComparison.CurrentCultureIgnoreCase));
        }
        public Product GetProduct(string ProductId)
        {
            return context.Products.Find(ProductId);
        }

        public IList<Product> GetOrderProducts(int OrderId)
        {
            var products = from o in context.OrderDetails
                           where o.OrderId == OrderId
                           select o.Product;

            return products.ToList();

        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            var CustomerToUpdate = GetCustomer(customer.CustomerId);

            if (CustomerToUpdate == null)
            {
                return false;
            }
            else
            {
                CustomerToUpdate.ContactName = customer.ContactName;
                CustomerToUpdate.CompanyName = customer.CompanyName;
                CustomerToUpdate.City = customer.City;
                CustomerToUpdate.Region= customer.Region;
                CustomerToUpdate.Address = customer.Address;

                context.Customers.Update(CustomerToUpdate);

                await context.SaveChangesAsync();
                return true;
            }

        }
    }
}