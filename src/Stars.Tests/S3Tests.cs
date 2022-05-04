using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stac;
using Terradue.Stars.Interface;
using Terradue.Stars.Services;
using Terradue.Stars.Services.Model.Stac;
using Terradue.Stars.Services.Resources;
using Terradue.Stars.Services.Router;
using Terradue.Stars.Services.Supplier;
using Terradue.Stars.Services.Supplier.Carrier;
using Terradue.Stars.Services.Supplier.Destination;
using Xunit;

namespace Stars.Tests
{
    [Collection(nameof(S3TestCollection))]
    public class S3Tests : S3BaseTest
    {
        private readonly AssetService assetService;
        private readonly IServiceProvider serviceProvider;

        public S3Tests(AssetService assetService, IServiceProvider sp, IOptions<S3Options> options) : base(options)
        {
            this.assetService = assetService;
            this.serviceProvider = sp;
        }

        [Fact]
        public void Test1()
        {
            S3ObjectDestination s3ObjectDestination = S3ObjectDestination.Create("s3://cpe-production-catalog/test.json");
            StacCatalogNode node = (StacCatalogNode)StacCatalogNode.Create(new StacCatalog("test", "test"), s3ObjectDestination.Uri);
            Assert.Equal("s3://cpe-production-catalog/test.json", node.Uri.ToString());
        }

        [Fact]
        public async Task ImportAssetsS3toS3()
        {
            await CreateBucketAsync("s3://cpe-acceptance-catalog/test");
            // await CreateBucketAsync("s3://dest");
            await CopyLocalDataToBucketAsync(Path.Join(Environment.CurrentDirectory, "../../../In/assets/test.tif"), "s3://cpe-acceptance-catalog/users/evova11/uploads/0HMD4AJ2DCT0E/500x477.tif");
            System.Net.S3.S3WebRequest s3WebRequest = (System.Net.S3.S3WebRequest)WebRequest.Create("s3://cpe-acceptance-catalog/users/evova11/uploads/0HMD4AJ2DCT0E/500x477.tif");
            s3WebRequest.Method = "GET";
            System.Net.S3.S3WebResponse s3WebResponse = (System.Net.S3.S3WebResponse)await s3WebRequest.GetResponseAsync();
            StacItem item = StacConvert.Deserialize<StacItem>(File.ReadAllText(Path.Join(Environment.CurrentDirectory, "../../../In/items/test502.json")));
            S3ObjectDestination s3ObjectDestination = S3ObjectDestination.Create("s3://cpe-acceptance-catalog/calls/857/notifications/test502.json");
            StacItemNode itemNode = (StacItemNode)StacItemNode.Create(item, s3ObjectDestination.Uri);
            await assetService.ImportAssets(itemNode, s3ObjectDestination, AssetFilters.SkipRelative);
        }

        [Fact]
        public async Task ImportUnlimitedStreamabletoS3()
        {
            await CreateBucketAsync("s3://unlimited");
            S3Resource s3Route = await S3Resource.CreateAsync(S3Url.Parse("s3://unlimited/test.bin"), s3Options.Value, null);
            BlockingStream stream = new BlockingStream(0, 100);
            S3StreamingCarrier s3StreamingCarrier = serviceProvider.GetRequiredService<S3StreamingCarrier>();
            s3StreamingCarrier.StartSourceCopy(
                                    File.OpenRead(Path.Join(Environment.CurrentDirectory, "../../../In/items/test502.json")),
                                    stream);
            IStreamResource streamable = new TestStreamable(stream, 0);
            var newRoute = await s3StreamingCarrier.StreamToS3Object(streamable, s3Route);
            System.Net.S3.S3WebRequest s3WebRequest = (System.Net.S3.S3WebRequest)WebRequest.Create("s3://unlimited/test.bin");
            s3WebRequest.Method = "GET";
            System.Net.S3.S3WebResponse s3WebResponse = (System.Net.S3.S3WebResponse)await s3WebRequest.GetResponseAsync();
            Assert.Equal(new FileInfo(Path.Join(Environment.CurrentDirectory, "../../../In/items/test502.json")).Length, s3WebResponse.ContentLength);
        }

        [Fact]
        public async Task ImportHttpStreamabletoS3()
        {
            await CreateBucketAsync("s3://http");
            S3Resource s3Route = await S3Resource.CreateAsync(S3Url.Parse("s3://http/S2B_MSIL2A_20211022T045839_N0301_R119_T44NLN_20211022T071547.jpg"), s3Options.Value, null);
            BlockingStream stream = new BlockingStream(0, 100);
            S3StreamingCarrier s3StreamingCarrier = serviceProvider.GetRequiredService<S3StreamingCarrier>();
            var httpRoute = WebRoute.Create(new Uri("https://store.terradue.com/api/scihub/sentinel2/S2MSI2A/2021/10/22/quicklooks/v1/S2B_MSIL2A_20211022T045839_N0301_R119_T44NLN_20211022T071547.jpg"));
            var newRoute = await s3StreamingCarrier.StreamToS3Object(httpRoute, s3Route);
            System.Net.S3.S3WebRequest s3WebRequest = (System.Net.S3.S3WebRequest)WebRequest.Create("s3://http/S2B_MSIL2A_20211022T045839_N0301_R119_T44NLN_20211022T071547.jpg");
            s3WebRequest.Method = "GET";
            System.Net.S3.S3WebResponse s3WebResponse = (System.Net.S3.S3WebResponse)await s3WebRequest.GetResponseAsync();
            Assert.Equal(httpRoute.ContentLength, Convert.ToUInt64(s3WebResponse.ContentLength));
        }
    }
}
