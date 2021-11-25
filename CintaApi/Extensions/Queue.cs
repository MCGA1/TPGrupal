using CintaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CintaApi.Extensions
{
    public  class Queue<T> where T : class
    {
        private List<DelayQueueItem<T>> items = new List<DelayQueueItem<T>>();

        public void Enqueue(T item)
        {
            items.Add(new DelayQueueItem<T>()
            {
                Value = item,
                Ingreso = DateTime.Now
            }); ;
        }

        public T Dequeue()
        {
            DateTime startTime = DateTime.Now;

            do
            {
                DateTime now = DateTime.Now;

                var item = items.FirstOrDefault(i => i.Preparado==true);
                if (item == null)
                    break;

                items.Remove(item);
                return item.Value;
            }
            while (true);

            return null;
        }

        private  class DelayQueueItem<T>
        {

            private DateTime _ingreso;
            public DateTime Ingreso

            {
                get
                {
                    return _ingreso;
                }
                set
                {
                    _ingreso = value;
                }
            }

            public bool Preparado
            {
                get
                {
                    return DateTime.Now >= Ingreso.AddSeconds(10) ? true : false;
                }
            }


            public T Value { get; set; }
        }



    }
}
