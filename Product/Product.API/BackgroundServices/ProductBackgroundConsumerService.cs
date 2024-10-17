using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Product.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using IModel = RabbitMQ.Client.IModel;

namespace Product.API.BackgroundServices
{
    public class ProductBackgroundConsumerService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private IServiceScopeFactory serviceScopeFactory;
        private IConfiguration _configuration;

        public ProductBackgroundConsumerService(IServiceScopeFactory _serviceScopeFactory, IConfiguration configuration)
        {
            serviceScopeFactory = _serviceScopeFactory;
            _configuration = configuration;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var RabbitMQServer = "";
            var RabbitMQUserName = "";
            var RabbutMQPassword = "";

            RabbitMQServer = _configuration.GetValue<string>("RabbitMQ:RabbitURL");
            RabbitMQUserName = _configuration.GetValue<string>("RabbitMQ:Username");
            RabbutMQPassword = _configuration.GetValue<string>("RabbitMQ:Password");

            var factory = new ConnectionFactory()
            { HostName = RabbitMQServer, UserName = RabbitMQUserName, Password = RabbutMQPassword };

            // create connection
            _connection = factory.CreateConnection();

            // create channel
            _channel = _connection.CreateModel();

            //Direct Exchange Details like name and type of exchange
            _channel.ExchangeDeclare(_configuration.GetValue<string>("RabbitMqSettings:ExchangeName"), _configuration.GetValue<string>("RabbitMqSettings:ExchhangeType"));

            //Declare Queue with Name and a few property related to Queue like durabality of msg, auto delete and many more
            _channel.QueueDeclare(queue: _configuration.GetValue<string>("RabbitMqSettings:QueueName"),
                        durable: true,
            exclusive: false,
            autoDelete: false,
                        arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            _channel.QueueBind(queue: _configuration.GetValue<string>("RabbitMqSettings:QueueName"), exchange: _configuration.GetValue<string>("RabbitMqSettings:ExchangeName"), routingKey: _configuration.GetValue<string>("RabbitMqSettings:RouteKey"));

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message
                var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

                // acknowledge the received message
                _channel.BasicAck(ea.DeliveryTag, false);

                //Deserilized Message
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var productDetails = JsonConvert.DeserializeObject<Dictionary<int, int?>>(message);

                //Stored Offer Details into the Database
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var _dbContext = scope.ServiceProvider.GetRequiredService<EcommerceDBContext>();
                    var productDetailslist = productDetails.ToList();
                    for (var i = 0; i < productDetailslist.Count(); i++)
                    {
                        var productDetail = _dbContext.Products.Where(x => x.Id == productDetailslist[i].Key).FirstOrDefault();
                        productDetail.Quantity = productDetail.Quantity - productDetailslist[i].Value;
                        _dbContext.Products.Update(productDetail);
                    }
                    _dbContext.SaveChanges();
                }

            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_configuration.GetValue<string>("RabbitMqSettings:QueueName"), false, consumer);
            return Task.CompletedTask;
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
