using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using Terradue.Stars.Interface;
using Terradue.Stars.Interface.Router;

namespace Terradue.Stars.Services.Processing
{
    internal class ZipEntryAsset : IAsset, IStreamable
    {
        private ZipEntry entry;
        private readonly ZipFile zipFile;
        private readonly IAsset parentAsset;

        public ZipEntryAsset(ZipEntry entry, ZipFile zipFile, IAsset parentAsset)
        {
            this.entry = entry;
            this.zipFile = zipFile;
            this.parentAsset = parentAsset;
        }

        public string Label => parentAsset.Label + " / " + entry.Name;

        public IEnumerable<string> Roles
        {
            get
            {
                var ext = Path.GetExtension(entry.Name).TrimStart('.'); 
                if ( (new string[]{ "txt", "xml", "json" }).Contains(ext)) return new string[] { "metadata" };
                return new string[] { "data" };
            }
        }

        public Uri Uri => new Uri(entry.Name, UriKind.Relative);

        public ContentType ContentType => new ContentType(System.Net.Mime.MediaTypeNames.Application.Octet);

        public ResourceType ResourceType => ResourceType.Asset;

        public ulong ContentLength => Convert.ToUInt64(entry.Size);

        public ContentDisposition ContentDisposition => new ContentDisposition() { FileName = entry.Name };

        public bool CanBeRanged => false;

        public IDictionary<string, object> Properties => new Dictionary<string, object>();

        public Task<Stream> GetStreamAsync()
        {
            return Task.FromResult(zipFile.GetInputStream(entry));
        }

        public IStreamable GetStreamable()
        {
            return this;
        }

        public Task<Stream> GetStreamAsync(long start, long end = -1)
        {
            throw new NotImplementedException();
        }
    }
}