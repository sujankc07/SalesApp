using Microsoft.AspNetCore.Authorization;
using SalesApp.Data;
using SalesApp.Models;
using SalesApp.Soap_Endpoint;

namespace SalesApp.Soap_Endpoint
{
    [Authorize]
    public class OrderSoapService:IOrderSoapService
    {
        private readonly OrderDbContext _context;

        public OrderSoapService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<string> SaveOrderAsync(string custNo, SaveOrder request)
        {
            if (request == null || !request.orders.Any())
            {
                return "Invalid request";
            }

            string custNum = custNo;

            List<OrderData> orders = request.orders;

            var orderList = orders.Select(p => new Order
            {
                CustomerNumber = custNum,
                PartNumber = p.PartNumber,
                PartName = p.PartName,
                Year = p.Year,
                Make = p.Make,
                Model = p.Model,
                Price = p.Price
            }).ToList();

            _context.Orders.AddRange(orderList);
            await _context.SaveChangesAsync();

            return "Order saved successfully";

        }

        public async Task<List<OrderData>>  GetOrders(string custNo)
        {
            var result = _context.Orders
                .Where(x => x.CustomerNumber == custNo)
                .Select(y => new OrderData
                {
                    PartNumber = y.PartNumber,
                    PartName = y.PartName,
                    Make = y.Make,
                    Model = y.Model,
                    Year = y.Year,
                    Price = y.Price
                }).ToList();

            return result; 
        }
    }

}
