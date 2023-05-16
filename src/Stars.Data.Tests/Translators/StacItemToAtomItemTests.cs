using System.Linq;
using Newtonsoft.Json;
using Xunit;
using Stac.Extensions.Projection;
using Terradue.Stars.Data.Translators;
using Stac;
using Terradue.Stars.Services.Model.Stac;
using Terradue.Stars.Services.Model.Atom;
using System.Threading;
using System.Xml;
using Terradue.ServiceModel.Ogc.Owc.AtomEncoding;
using System;

namespace Terradue.Data.Tests.Translators
{
    public class StacItemToAtomItemTests : TestBase
    {
        [Fact]
        public async System.Threading.Tasks.Task S2A_MSIL2A_20191216T004701_N0213_R102_T53HPA_20191216T024808()
        {
            string json = GetJson("Translators");

            ValidateJson(json);

            StacItem stacItem = StacConvert.Deserialize<StacItem>(json);

            StacItemToAtomItemTranslator stacItemToAtomItemTranslator = new StacItemToAtomItemTranslator(ServiceProvider);

            StacItemNode stacItemNode = new StacItemNode(stacItem, new System.Uri("s3://eoepca-ades/wf-d0534740-b97b-11eb-82cf-0a580a830350/S2A_MSIL2A_20191216T004701_N0213_R102_T53HPA_20191216T024808/S2A_MSIL2A_20191216T004701_N0213_R102_T53HPA_20191216T024808.json"));

            AtomItemNode atomItemNode = await stacItemToAtomItemTranslator.TranslateAsync<AtomItemNode>(stacItemNode, CancellationToken.None);

            Assert.Equal("S2A_MSIL2A_20191216T004701_N0213_R102_T53HPA_20191216T024808", atomItemNode.AtomItem.Identifier);

        }

        [Fact]
        public async System.Threading.Tasks.Task VectorResults()
        {
            string json = GetJson("Translators");

            ValidateJson(json);

            StacItem stacItem = StacConvert.Deserialize<StacItem>(json);

            StacItemToAtomItemTranslator stacItemToAtomItemTranslator = new StacItemToAtomItemTranslator(ServiceProvider);

            StacItemNode stacItemNode = new StacItemNode(stacItem, new System.Uri("s3://eoepca-ades/wf-test/test.json"));

            AtomItemNode atomItemNode = await stacItemToAtomItemTranslator.TranslateAsync<AtomItemNode>(stacItemNode, CancellationToken.None);

            // Get the vector offering
            var offerings = atomItemNode.AtomItem.ElementExtensions.ReadElementExtensions<OwcOffering>("offering", OwcNamespaces.Owc, new System.Xml.Serialization.XmlSerializer(typeof(OwcOffering)));
            var vectorOfferings = offerings.Where(r => r.Code == "http://www.terradue.com/fgb");

            Assert.NotNull(vectorOfferings);
            Assert.Equal(2, vectorOfferings.Count());
            Assert.Equal("GetMap", vectorOfferings.ElementAt(0).Operations.First().Code);
            Assert.Equal("application/flatgeobuf", vectorOfferings.ElementAt(0).Operations.First().Type);
            Assert.Equal("GetMap", vectorOfferings.ElementAt(1).Operations.First().Code);
            Assert.Equal("application/geo+json", vectorOfferings.ElementAt(1).Operations.First().Type);
        }

        [Fact]
        public async System.Threading.Tasks.Task LegendTest()
        {
            string json = GetJson("Translators");

            ValidateJson(json);

            StacItem stacItem = StacConvert.Deserialize<StacItem>(json);

            StacItemToAtomItemTranslator stacItemToAtomItemTranslator = new StacItemToAtomItemTranslator(ServiceProvider);

            StacItemNode stacItemNode = new StacItemNode(stacItem, new System.Uri("s3://eoepca-ades/wf-test/test.json"));

            AtomItemNode atomItemNode = await stacItemToAtomItemTranslator.TranslateAsync<AtomItemNode>(stacItemNode, CancellationToken.None);

            Terradue.ServiceModel.Syndication.SyndicationLink legendLink = atomItemNode.AtomItem.Links.FirstOrDefault(r => r.RelationshipType == "legend");

            Assert.NotNull(legendLink);
            Assert.Equal("https://test.com/legend.png", legendLink.Uri.AbsoluteUri);
        }

        [Fact]
        public async System.Threading.Tasks.Task TWMTest()
        {
            string json = GetJson("Translators");

            ValidateJson(json);

            StacItem stacItem = StacConvert.Deserialize<StacItem>(json);

            StacItemToAtomItemTranslator stacItemToAtomItemTranslator = new StacItemToAtomItemTranslator(ServiceProvider);

            StacItemNode stacItemNode = new StacItemNode(stacItem, new System.Uri("s3://eoepca-ades/wf-test/test.json"));

            AtomItemNode atomItemNode = await stacItemToAtomItemTranslator.TranslateAsync<AtomItemNode>(stacItemNode, CancellationToken.None);

            // Get the vector offering
            var offerings = atomItemNode.AtomItem.ElementExtensions.ReadElementExtensions<OwcOffering>("offering", OwcNamespaces.Owc, new System.Xml.Serialization.XmlSerializer(typeof(OwcOffering)));
            var twmOfferings = offerings.Where(r => r.Code == "http://www.terradue.com/twm");

            Assert.NotNull(twmOfferings);
            Assert.Equal(1, twmOfferings.Count());
            Assert.Equal("GetMap", twmOfferings.ElementAt(0).Operations.First().Code);
            Assert.Equal("image/png", twmOfferings.ElementAt(0).Operations.First().Type);
        }

    }

}
