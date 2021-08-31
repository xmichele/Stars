using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ionic.Zip;
using Microsoft.Extensions.Logging;
using Terradue.Stars.Interface;
using Terradue.Stars.Interface.Supplier.Destination;
using Terradue.Stars.Services.Router;
using Terradue.Stars.Services.Supplier;
using Terradue.Stars.Services.Supplier.Carrier;
using Terradue.Stars.Services.Supplier.Destination;

namespace Terradue.Stars.Services.Processing
{
    internal class ZipArchiveAsset : Archive
    {
        private ZipFile zipFile;
        private readonly IAsset asset;
        private readonly ILogger logger;

        private IStreamable localStreamable;

        public ZipArchiveAsset(IAsset asset, ILogger logger)
        {
            this.asset = asset;
            this.logger = logger;
        }

        protected async Task<Stream> GetZipStreamAsync(IAsset asset, CarrierManager carrierManager)
        {
            if ( asset.Uri.Scheme == "file" )
                return await asset.GetStreamable().GetStreamAsync();
            var tmpDestination = LocalFileDestination.Create("/tmp", asset);
            var tmpArchiveAssetDestination = tmpDestination.To(asset, Guid.NewGuid().ToString());
            tmpArchiveAssetDestination.PrepareDestination();
            var localZipDelivery = carrierManager.GetSingleDeliveryQuotations(asset, tmpArchiveAssetDestination).First();
            localStreamable = await localZipDelivery.Carrier.Deliver(localZipDelivery) as LocalFileSystemResource;
            return await localStreamable.GetStreamAsync();
        }

        public IReadOnlyDictionary<string, IAsset> Assets
        {
            get
            {
                Dictionary<string, IAsset> assets = new Dictionary<string, IAsset>();
                if (zipFile == null) return assets;
                foreach (ZipEntry entry in zipFile)
                {
                    if (entry.IsDirectory) continue;

                    assets.Add(entry.FileName, new ZipEntryAsset(entry, zipFile, asset));
                }
                return assets;
            }
        }

        public override System.Uri Uri => asset.Uri;

        public string AutodetectSubfolder()
        {
            List<string> names = new List<string>();
            foreach (ZipEntry entry in zipFile)
            {
                names.Add(entry.FileName);
            }
            var commonfolder = Findstem(names.ToArray());
            if (commonfolder.IndexOf('/') > 1)
                return "";
            return Path.GetFileNameWithoutExtension(asset.Uri.ToString());
        }

        internal async override Task<IAssetsContainer> ExtractToDestination(IDestination destination, CarrierManager carrierManager)
        {
            Dictionary<string, IAsset> assetsExtracted = new Dictionary<string, IAsset>();
            zipFile = Ionic.Zip.ZipFile.Read(await GetZipStreamAsync(asset, carrierManager));
            string subFolder = AutodetectSubfolder();

            foreach (var archiveAsset in Assets)
            {
                var archiveAssetDestination = destination.To(archiveAsset.Value, subFolder);
                archiveAssetDestination.PrepareDestination();
                var assetDeliveries = carrierManager.GetSingleDeliveryQuotations(archiveAsset.Value, archiveAssetDestination);
                logger.LogDebug(archiveAsset.Key);
                foreach (var delivery in assetDeliveries)
                {
                    var assetExtracted = await delivery.Carrier.Deliver(delivery);
                    if (assetExtracted != null)
                    {
                        assetsExtracted.Add(asset.ContentDisposition.FileName + "!" + archiveAsset.Key, new GenericAsset(assetExtracted, archiveAsset.Value.Title, archiveAsset.Value.Roles));
                        break;
                    }
                }
            }
            DisposeLocalStreamable();
            return new GenericAssetContainer(this, assetsExtracted);
        }

        private void DisposeLocalStreamable()
        {
            if ( localStreamable != null )
                File.Delete(localStreamable.Uri.LocalPath);
            localStreamable = null;
        }
    }
}