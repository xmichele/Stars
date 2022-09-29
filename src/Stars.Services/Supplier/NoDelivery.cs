using System;
using System.Threading.Tasks;
using Terradue.Stars.Interface;
using Terradue.Stars.Interface.Router;
using Terradue.Stars.Interface.Supplier;
using Terradue.Stars.Interface.Supplier.Destination;

namespace Terradue.Stars.Services.Supplier
{
    internal class NoDelivery : IDelivery
    {
        private IResource route;
        private readonly ICarrier carrier;

        public NoDelivery(IResource route, ICarrier carrier)
        {
            this.route = route;
            this.carrier = carrier;
        }

        public int Cost => 100000;

        public IResource Resource => route;

        public ICarrier Carrier => carrier;

        public IDestination Destination => null;

        public override string ToString()
        {
            return string.Format("No delivery");
        }

        public Func<IDelivery, IResource, Task> PreCheckDeliveryFunction { get; set; }
    }
}