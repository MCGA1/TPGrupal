using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CintaApi.Extensions
{
    public static  class ConnectionFactoryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ConnectionFactory ConnectionFactory()
        {
            return new ConnectionFactory() { HostName = "localhost",DispatchConsumersAsync=true };
        }
    }
}
