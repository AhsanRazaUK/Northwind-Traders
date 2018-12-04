using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Northwind.UI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.UI.ViewComponents
{
    // Under Construction 
    public class MyPagerViewComponent : ViewComponent
    {
        private readonly ILogger<MyPagerViewComponent> logger;
        private readonly INorthwindRepository repository;

        public MyPagerViewComponent(ILogger<MyPagerViewComponent> logger, INorthwindRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<object> pagingList)
        {
            var pageSize = pagingList.Count();

            var totalRecordCount = repository.GetCustomers().Count();
            var pageCount = totalRecordCount > 0
              ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
              : 0;
            
            return await Task.FromResult((IViewComponentResult)View("Default", pageCount));
        }
    }
}
