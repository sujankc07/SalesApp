namespace SalesApp.Soap_Endpoint;
using System.Collections.Generic;
using System.ServiceModel;
using SalesApp.Models;

[ServiceContract]
public interface IOrderSoapService
{
    [OperationContract]
    Task<string> SaveOrderAsync(string custNo, SaveOrder request);

    [OperationContract]
    Task<List<OrderData>> GetOrders(string custNo);
}
