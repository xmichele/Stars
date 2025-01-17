using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Terradue.Stars.Interface;

namespace Terradue.Stars.Services.Processing
{
    public class TarEntryAsset : IAsset, IStreamResource
    {
        private string name;
        private ulong size;
        private BlockingStream blockingStream;

        public TarEntryAsset(string name, ulong size, BlockingStream blockingStream)
        {
            this.name = name;
            this.size = size;
            this.blockingStream = blockingStream;
        }

        public string Name => name;

        public string Title => name;

        public IReadOnlyList<string> Roles => new string[] { "data" };

        public IReadOnlyDictionary<string, object> Properties
        {
            get
            {
                Dictionary<string, object> props = new Dictionary<string, object>();
                props.Add("filename", name);
                return props;
            }
        }

        public ContentType ContentType
        {
            get
            {
                string mediaType = MimeTypes.GetMimeType(name);
                return new ContentType(mediaType);
            }
        }

        public ResourceType ResourceType => ResourceType.Asset;

        public ulong ContentLength => size;

        public ContentDisposition ContentDisposition => new ContentDisposition() { FileName = Path.GetFileName(name) };

        public Uri Uri => new Uri(name, UriKind.Relative);

        public bool CanBeRanged => false;

        public Task<Stream> GetStreamAsync(CancellationToken ct)
        {
            return Task.FromResult<Stream>(blockingStream);
        }

        public Task<Stream> GetStreamAsync(long start, CancellationToken ct, long end = -1)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAsset> Alternates => Enumerable.Empty<IAsset>();
    }
}