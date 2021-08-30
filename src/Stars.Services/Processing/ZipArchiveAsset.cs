using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ionic.Zip;
using Microsoft.Extensions.Logging;
using Terradue.Stars.Interface;
using Terradue.Stars.Interface.Supplier.Destination;
using Terradue.Stars.Services.Supplier;
using Terradue.Stars.Services.Supplier.Carrier;

namespace Terradue.Stars.Services.Processing
{
    internal class ZipArchiveAsset : Archive
    {
        private ZipFile zipFile;
        private readonly IAsset asset;
        private readonly ILogger logger;

        public ZipArchiveAsset(IAsset asset, ILogger logger)
        {
            this.asset = asset;
            this.logger = logger;
        }

        protected BlockingStream GetZipStream(IAsset asset)
        {
            const int chunk = 4096;
            BlockingStream blockingStream = new BlockingStream(1000);
            asset.GetStreamable().GetStreamAsync()
                .ContinueWith(task =>
                {
                    var stream = task.GetAwaiter().GetResult();
                    Task.Factory.StartNew(() =>
                    {
                        int read;
                        var buffer = new byte[chunk];
                        do
                        {
                            read = stream.Read(buffer, 0, chunk);
                            blockingStream.Write(buffer, 0, read);
                        } while (read == chunk);
                        blockingStream.Close();
                    });
                });
            return blockingStream;
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
            zipFile = Ionic.Zip.ZipFile.Read(GetZipStream(asset));
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
            return new GenericAssetContainer(this, assetsExtracted);
        }
    }
}