using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Stac;
using Stac.Catalog;
using Stac.Collection;
using Terradue.Stars.Interface;
using Terradue.Stars.Interface.Router;
using Terradue.Stars.Services.Router;

namespace Terradue.Stars.Services.Model.Stac
{
    public class StacCatalogNode : StacNode, ICatalog
    {
        private readonly ICredentials credentials;

        public StacCatalogNode(IStacCatalog stacCatalog, System.Net.ICredentials credentials = null) : base(stacCatalog)
        {
            this.credentials = credentials;
        }

        public StacCatalog StacCatalog => stacObject as StacCatalog;

        public override ResourceType ResourceType
        {
            get
            {
                if (stacObject is StacCollection)
                    return ResourceType.Collection;
                else
                    return ResourceType.Catalog;
            }
        }

        public override IReadOnlyList<IResource> GetRoutes()
        {
            StacRouter stacRouter = new StacRouter(credentials);
            return StacCatalog.GetChildren().Select(child => new StacCatalogNode(child.Value)).Cast<IResource>()
                    .Concat(StacCatalog.GetItems().Select(item => new StacItemNode(item.Value)))
                    .ToList();
        }

    }
}