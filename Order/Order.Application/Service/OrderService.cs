using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Order.Application.Service.IService;
using Order.Infrastructure.Repository.IRepository;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _OrderRepository;

        public OrderService(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }
        public async Task<List<Domain.Entities.Order>> GetOrderById(int id)
        {
            return await _OrderRepository.GetOrderById(id);
        }

        public async Task<List<Domain.Entities.Order>> GetOrderByUser(string userEmail)
        {
            return await _OrderRepository.GetOrderByUser(userEmail);
        }

        public async Task OrderNow(Domain.Entities.Order order)
        {
            try
            {
                var allowedit = await _OrderRepository.OrderNow(order);
                if (allowedit)
                {
                    var products = order.OrderItems.GroupBy(x => x.ProductId)
                                      .ToDictionary(x => x.Key, x => x.Select(i => i.Quantity).Sum());

                    UpdateProductQuntity(products);
                }
                else
                {
                    throw new Exception("Quantity not available");
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        private bool UpdateProductQuntity(Dictionary<int, int?> products)
        {
            var RabbitMQServer = "localhost";
            var RabbitMQUserName = "guest";
            var RabbutMQPassword = "guest";

            try
            {
                var factory = new ConnectionFactory()
                { HostName = RabbitMQServer, UserName = RabbitMQUserName, Password = RabbutMQPassword };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    //Direct Exchange Details like name and type of exchange
                    channel.ExchangeDeclare("OfferExchange", "direct");

                    //Declare Queue with Name and a few property related to Queue like durabality of msg, auto delete and many more
                    channel.QueueDeclare(queue: "offer_queue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    //Bind Queue with Exhange and routing details
                    channel.QueueBind(queue: "offer_queue", exchange: "OfferExchange", routingKey: "offer_route");

                    //Seriliaze object using Newtonsoft library
                    string productDetail = JsonConvert.SerializeObject(products);
                    var body = Encoding.UTF8.GetBytes(productDetail);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    //publish msg
                    channel.BasicPublish(exchange: "OfferExchange",
                                         routingKey: "offer_route",
                                         basicProperties: properties,
                                         body: body);

                    return true;
                }
            }

            catch (Exception)
            {
            }
            return false;
        }
    }
}
