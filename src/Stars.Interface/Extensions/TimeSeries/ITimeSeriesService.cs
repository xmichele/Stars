using System;
using System.Threading;
using System.Threading.Tasks;
using Stac;
using Stac.Common;

namespace Terradue.Stars.Interface.Extensions.TimeSeries
{
    public interface ITimeSeriesService
    {
        Task<StacCollection> CreateNewTimeSeriesCollectionAsync(IAbstractTimeSeriesOperationRequest<StacCollection> request, CancellationToken cancellationToken);

        Task<StacCollection> UpdateTimeSeriesCollectionAsync(IAbstractTimeSeriesOperationRequest<StacCollection> request, CancellationToken cancellationToken);

        Task<StacCollection> PatchTimeSeriesCollectionAsync(IAbstractTimeSeriesOperationRequest<Patch> request, CancellationToken cancellationToken);

        Task DeleteTimeSeriesCollectionAsync(IAbstractTimeSeriesOperationRequest<StacCollection> request, CancellationToken cancellationToken);

        Task<IAbstractTimeSeriesOperationStatus<T>> GetTimeSeriesOperationStatusAsync<T>(StacCollection collection, CancellationToken cancellationToken);
    }
}