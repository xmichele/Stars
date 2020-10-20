using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stac;
using Stac.Catalog;
using Stac.Item;
using Terradue.Stars.Interface.Router;
using Terradue.Stars.Interface.Supplier;
using Terradue.Stars.Services.Router;

namespace Terradue.Stars.Services.Model.Stac
{
    public abstract class StacNode : IRoute, IStreamable
    {
        protected IStacObject stacObject;
        protected ContentType contentType;

        protected StacNode(IStacObject stacObject)
        {
            this.stacObject = stacObject;
            this.contentType = new ContentType("application/json");
        }

        public string Label => stacObject.Id;

        public ContentType ContentType => contentType;

        public Uri Uri => stacObject.Uri == null ? new Uri(stacObject.Id + ".json", UriKind.Relative) : stacObject.Uri;

        public abstract ResourceType ResourceType { get; }

        public string Id => stacObject.Id.CleanIdentifier();

        public virtual ulong ContentLength => Convert.ToUInt64(Encoding.Default.GetBytes(JsonConvert.SerializeObject(stacObject)).Length);

        public bool IsCatalog => (stacObject is IStacCatalog);

        public ContentDisposition ContentDisposition => new ContentDisposition() { FileName = FileName };

        public string FileName
        {
            get
            {
                if ( IsRoot )
                    return "catalog.json";
                return stacObject.Id + ".json";
            }
        }

        internal static StacNode Create(IStacObject stacObject)
        {
            if (stacObject is IStacCatalog)
                return new StacCatalogNode(stacObject as IStacCatalog);

            if (stacObject is IStacItem)
                return new StacItemNode(stacObject as IStacItem);

            return null;
        }



        public IStacObject StacObject => stacObject;

        public bool IsRoot { get; set; }

        public bool CanBeRanged => false;

        public abstract IList<IRoute> GetRoutes();

        public async Task<Stream> GetStreamAsync()
        {
            MemoryStream ms = new MemoryStream();
            return await Task<Stream>.Run(() =>
            {
                var sw = new StreamWriter(ms);
                JsonSerializer.Create().Serialize(sw, stacObject);
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                return ms as Stream;
            });

        }

        public Task<Stream> GetStreamAsync(long start, long end = -1)
        {
            throw new NotImplementedException();
        }
    }
}