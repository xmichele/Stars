using System;
using Terradue.Stars.Interface.Router;
using Terradue.Stars.Interface.Supplier;
using Terradue.Stars.Interface.Supplier.Destination;
using Terradue.Stars.Services.Router;
using Terradue.Stars.Services.Supplier.Destination;

namespace Terradue.Stars.Services.Supplier
{
    public class LocalDelivery : IDelivery
    {
        private readonly ICarrier carrier;
        private readonly IRoute route;
        private readonly ISupplier supplier;
        private readonly LocalDirectoryDestination dirDestination;
        private readonly LocalFileDestination fileDestination;
        private readonly int cost;

        public LocalDelivery(ICarrier carrier, IRoute route, ISupplier supplier, LocalDirectoryDestination dirDestination, LocalFileDestination fileDestination, int cost)
        {
            this.carrier = carrier;
            this.route = route;
            this.supplier = supplier;
            this.dirDestination = dirDestination;
            this.fileDestination = fileDestination;
            this.cost = cost;
        }

        public int Cost => cost;

        public IDestination Destination => fileDestination;

        public IRoute Route => route;

        public ICarrier Carrier => carrier;

        public ISupplier Supplier => supplier;

        public Uri TargetUri => fileDestination.Uri;

        public string LocalPath => fileDestination.Uri.ToString();
    }
}