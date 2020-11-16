using System.Collections.Generic;
using System.Threading.Tasks;
using Terradue.Stars.Interface.Router;
using Terradue.Stars.Interface.Supplier.Destination;

namespace Terradue.Stars.Interface.Supplier
{
    public interface ISupplier : IPlugin
    {
        string Id { get; }

        // TODO add assets filters
        Task<IResource> SearchFor(IResource item);

        IDeliveryQuotation QuoteDelivery(IResource resource, IDestination destination);

        Task<IOrder> Order(IOrderable orderableRoute);
    }
}