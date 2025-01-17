using System;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Stac;

namespace Terradue.Stars.Interface
{
    public interface IDeletableResource : IStreamResource
    {
        Task DeleteAsync(CancellationToken ct);
    }
}