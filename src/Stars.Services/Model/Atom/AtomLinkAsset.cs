using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Terradue.Stars.Interface.Router;

using Terradue.Stars.Services.Router;
using Terradue.ServiceModel.Syndication;
using System.Net;

namespace Terradue.Stars.Services.Model.Atom
{
    public class AtomLinkAsset : IAsset
    {
        private SyndicationLink link;
        private SyndicationItem item;
        private readonly ICredentials credentials;
        private readonly WebRoute webRoute;

        public AtomLinkAsset(SyndicationLink link, SyndicationItem item, System.Net.ICredentials credentials = null)
        {
            this.link = link;
            this.item = item;
            this.credentials = credentials;
            this.webRoute = WebRoute.Create(Uri, Convert.ToUInt64(link.Length), credentials);
        }

        public Uri Uri => link.Uri;

        public ContentType ContentType
        {
            get
            {
                try
                {
                    return new ContentType(link.MediaType);
                }
                catch
                {
                    return null;
                }
            }
        }

        public ulong ContentLength => link.Length == 0 ? webRoute.ContentLength : Convert.ToUInt64(link.Length);

        public string Label => string.Format("[{0}] {1}", string.Join(",", link.RelationshipType), string.IsNullOrEmpty(link.Title) ? Path.GetFileName(link.Uri.AbsolutePath) : link.Title);

        public ResourceType ResourceType => ResourceType.Asset;

        public IEnumerable<string> Roles => new string[] { link.RelationshipType };

        public ContentDisposition ContentDisposition => webRoute.ContentDisposition;

        public IStreamable GetStreamable()
        {
            return webRoute;
        }

    }
}