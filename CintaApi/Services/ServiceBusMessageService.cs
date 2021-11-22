using CintaApi.Extensions;
using CintaApi.Interfaces;
using CintaApi.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CintaApi.Services
{
    public class ServiceBusMessageService : IServiceBusQueueMessage
    {
        public void PonerBulto(Bulto entity)
        {
            var connection = ConnectionFactoryExtensions.ConnectionFactory();


            var channel = connection.CreateConnection().CreateModel();
            {


                channel.QueueDeclare(queue: "Cinta", durable: false, exclusive: false, autoDelete: false, arguments: null);

                //-> Mensaje

                var bulto = JsonConvert.SerializeObject(entity);



                var body = Encoding.UTF8.GetBytes(bulto);

                //-> Enviamos el mensaje
                channel.BasicPublish(exchange: "", routingKey: "Cinta", basicProperties: null, body: body);

            }
            channel.Dispose();
        }

        public async Task<IEnumerable<Bulto>> SacarBulto(string bultoId)
        {
            List<Bulto> bultos = new List<Bulto>();

            var result = SacarBultoListado();

            foreach (var item in result)
            {
               if(item.Id.ToString().Contains(bultoId)) bultos.Add(item);
            }

            return bultos;
          
        }


        public List<Bulto> SacarBultoListado()
        {
            var bulto = new List<Bulto>();

            var factory = new ConnectionFactory() { HostName = "localhost" ,DispatchConsumersAsync=true};

            using (var connection = factory.CreateConnection())
            using (var canal = connection.CreateModel())
            {
                canal.QueueDeclare(queue: "Cinta", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumidor = new AsyncEventingBasicConsumer(canal);
                consumidor.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var mensaje = Encoding.UTF8.GetString(body);

                    bulto.Add(JsonConvert.DeserializeObject<Bulto>(mensaje));

                    await Task.Yield();

                };

                canal.BasicConsume(queue: "Cinta", autoAck: false, consumer: consumidor);

            }


            return bulto;


            //var bulto = new List<Bulto>();
            //var connection = ConnectionFactoryExtensions.ConnectionFactory();


            //var channel = connection.CreateConnection().CreateModel();
            //{

            //    channel.QueueDeclare(queue: "Cinta", durable: false, exclusive: false, autoDelete: false, arguments: null);


            //    var consumidor = new EventingBasicConsumer(channel);

            //    consumidor.Received += (model, ea) =>
            //    {
            //        var body = ea.Body.ToArray();


            //        var mensaje = Encoding.UTF8.GetString(body);

            //        bulto = JsonConvert.DeserializeObject<List<Bulto>>(mensaje);

            //        channel.BasicConsume(queue: "Cinta", autoAck: false, consumer: consumidor);

            //    };
            //    return bulto;
            //}
        }
    }
}

