using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Northwind.Data.Entities;
using Northwind.UI.Repository;
using Northwind.UI.ViewModels;
using ReflectionIT.Mvc.Paging;

namespace Northwind.UI.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> logger;
        private readonly INorthwindRepository repository;
        private readonly IMapper mapper;

        public CustomersController(ILogger<CustomersController> logger, INorthwindRepository repository, IMapper mapper)
        {
            this.logger = logger;
            this.repository = repository;
            this.mapper = mapper;
        }
        public IActionResult Index(string filter, int page = 1, int pageSize = 10)
        {
            try
            {
                logger.LogInformation("In Index: Get Customers");

                //int skip = (page - 1) * pageSize;
                //int take = pageSize;
                //var customers = await repository.GetCustomers().Skip(skip).Take(take).ToListAsync();

                var customersQuery = repository.GetCustomers();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    customersQuery = customersQuery.Where(p => p.ContactName.Contains(filter));
                }

                var customersViewModel = mapper.Map<IEnumerable<CustomerViewModel>>(customersQuery).AsQueryable().OrderBy(c => c.CustomerId);

                var customers = PagingList.Create(customersViewModel, pageSize, page);

                return View(customers);
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                return RedirectToAction("Error");
            }
        }

        public IActionResult Edit(string Id)
        {
            try
            {
                logger.LogInformation("In Edit: Get Customer");

                var customer = repository.GetCustomer(Id);
                var customerVM = mapper.Map<CustomerViewModel>(customer);


                var editViewModel = new EditCustomerViewModel()
                {
                    CustomerViewModel = customerVM
                };

                var orders = repository.GetOrders(Id);
                editViewModel.OrdersViewModel = new List<OrdersViewModel>();

                foreach (var o in orders)
                {
                    var OVM = new OrdersViewModel()
                    {
                        Orders = o,
                        OrderProducts = new List<Product>()
                    };

                    OVM.OrderProducts.AddRange(repository.GetOrderProducts(o.OrderId));
                    editViewModel.OrdersViewModel.Add(OVM);
                }

                return View(editViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var customerToUpdate = mapper.Map<Customer>(model);

                    if (await repository.UpdateCustomerAsync(customerToUpdate))
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    logger.LogError("Edit Customer: Model State Invalid");
                    return View(model);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                return RedirectToAction("Error");
            }
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
