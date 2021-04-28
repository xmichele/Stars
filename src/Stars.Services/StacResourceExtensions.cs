using System.Collections.Generic;
using System.Linq;
using Stac;
using Terradue.Stars.Interface;
using Terradue.Stars.Services.Model.Stac;

namespace Terradue.Stars.Services
{
    public static class StacResourceExtensions
    {

        public static void MergeAssets(this StacItem stacItem, IAssetsContainer assetContainer)
        {
            foreach (var asset in assetContainer.Assets)
            {
                if (!stacItem.Assets.ContainsKey(asset.Key))
                {
                    stacItem.Assets.Add(asset.Key, asset.Value.CreateStacAsset(stacItem));
                    continue;
                }
                stacItem.Assets[asset.Key].Uri = asset.Value.Uri;
                stacItem.Assets[asset.Key].ContentLength = asset.Value.ContentLength;
                stacItem.Assets[asset.Key].MediaType = asset.Value.ContentType;
                stacItem.Assets[asset.Key].Title = asset.Value.Title;
            }
        }

        public static StacAsset CreateStacAsset(this IAsset asset, StacItem stacItem)
        {
            if ( asset is StacAssetAsset ) return new StacAsset((asset as StacAssetAsset).StacAsset, stacItem);
            return new StacAsset(stacItem, asset.Uri, asset.Roles, asset.Title, asset.ContentType, asset.ContentLength);
        }

        public static void UpdateLinks(this IStacCatalog catalogNode, IEnumerable<IResource> resources)
        {
            foreach (var resource in resources)
            {
                if (resource == null) continue;
                foreach (var link in catalogNode.Links.Where(a => a != null && a.Uri.Equals(resource.Uri)).ToArray())
                    catalogNode.Links.Remove(link);

                if (resource is ICatalog)
                    catalogNode.Links.Add(StacLink.CreateChildLink(resource.Uri, resource.ContentType.ToString()));
                if (resource is IItem)
                    catalogNode.Links.Add(StacLink.CreateItemLink(resource.Uri, resource.ContentType.ToString()));
            }
        }

    }

}