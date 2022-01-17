﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GeoJSON.Net.Geometry;
using Microsoft.Extensions.Logging;
using Stac;
using Stac.Extensions.Eo;
using Stac.Extensions.Processing;
using Stac.Extensions.Projection;
using Stac.Extensions.Raster;
using Stac.Extensions.View;
using Terradue.Stars.Interface;
using Terradue.Stars.Interface.Supplier.Destination;
using Terradue.Stars.Services.Model.Stac;
// using Catfood.Shapefile;

namespace Terradue.Stars.Data.Model.Metadata.Resursp {
    public class ResurspMetadataExtractor : MetadataExtraction {
        public override string Label => "Resursp (Roscosmos) missions product metadata extractor";

        public ResurspMetadataExtractor(ILogger<ResurspMetadataExtractor> logger) : base(logger) {
        }

        public override bool CanProcess(IResource route, IDestination destination) {
            IItem item = route as IItem;
            if (item == null) return false;
            IAsset metadataFile = FindFirstAssetFromFileNameRegex(item, "[0-9a-zA-Z_-]*(\\.xml)$");
            if (metadataFile == null) {
                return false;
            }
             // deserialize product medatadata
            SPP_ROOT productMetadata = DeserializeProductMetadata(metadataFile.GetStreamable()).GetAwaiter().GetResult();
            if (productMetadata == null) {
                return false;
            }

            return true;
        }

        public static async Task<SPP_ROOT> DeserializeProductMetadata(IStreamable productMetadataFile) {
            XmlSerializer ser = new XmlSerializer(typeof(SPP_ROOT));
            SPP_ROOT auxiliary;
            using (var stream = new StreamReader(productMetadataFile.Uri.AbsolutePath,Encoding.UTF8,true) ) {
                using (XmlReader reader = XmlReader.Create(stream)) {
                    auxiliary = (SPP_ROOT)ser.Deserialize(reader);
                }
            }
            
            
            return auxiliary;
        }


        protected override async Task<StacNode> ExtractMetadata(IItem item, string suffix) {
            logger.LogDebug("Retrieving the metadata files in the product package");
            IAsset metadatafile = FindFirstAssetFromFileNameRegex(item, "[0-9a-zA-Z_-]*(\\.xml)$");
            if (metadatafile == null) {
                throw new FileNotFoundException("Unable to find any metadata file asset");
            }

            // deserialize product medatadata
            SPP_ROOT productMetadata = await DeserializeProductMetadata(metadatafile.GetStreamable());


            logger.LogDebug("Retrieving the shapefile in the product package");
            IAsset shapefile = FindFirstAssetFromFileNameRegex(item, "[0-9a-zA-Z_-]*(\\.shp)$");
            if (shapefile == null) {
                throw new FileNotFoundException("Unable to find any shapefile asset");
            }

            // retrieving id from filename
            // GF2_PMS1_W91.0_N17.6_20200510_L1A0004793969-MSS1.xml
            string stacItemId = Path.GetFileNameWithoutExtension(metadatafile.Uri.OriginalString).Split('-')[0];

            // retrieve lowest gsd
            double gsd = Double.Parse(productMetadata.Normal.NPixelImg);

            // to retrieve the properties, any product metadata is ok
            var stacItem = GetStacItemWithProperties(productMetadata, stacItemId, gsd, shapefile);

            await AddAssetsAsync(stacItem, item, gsd);

            FillBasicsProperties(stacItem.Properties);

            return StacItemNode.Create(stacItem, item.Uri);
            ;
        }


        private StacItem GetStacItemWithProperties(SPP_ROOT productMetadata, string stacItemId, double gsd,
            IAsset shapefile) {
            // retrieving GeometryObject from metadata
            var geometryObject = GetGeometryObjectFromProductMetadata(productMetadata);
            
            // TODO retrieve geometry from shapefile
            //var geometryObject = GetGeometryObjectFromProductShapefile(shapefile);

            // retrieving the common metadata properties (i.e. time and instruments)
            var commonMetadata = GetCommonMetadata(productMetadata, gsd);

            // initializing the stac item object
            var stacItem = new StacItem(stacItemId, geometryObject, commonMetadata);

            AddViewStacExtension(productMetadata, stacItem);
            AddProjStacExtension(productMetadata, stacItem);
            AddProcessingStacExtension(productMetadata, stacItem);

            return stacItem;
        }


        private string ConvertDegreeAngleToDegreeString(string coordinate) {
            return ConvertDegreeAngleToDegreeDouble(coordinate).ToString();
        }

        private double ConvertDegreeAngleToDegreeDouble(string coordinate) {
            // example 0:15:22.185719

            string[] coordinateArray = coordinate.Split(':');

            double degrees = Double.Parse(coordinateArray[0]);
            double minutes = Double.Parse(coordinateArray[1]);
            double seconds = Double.Parse(coordinateArray[2]);
            //Decimal degrees = 
            //   whole number of degrees, 
            //   plus minutes divided by 60, 
            //   plus seconds divided by 3600

            return degrees + (minutes / 60) + (seconds / 3600);
        }
        
        private GeoJSON.Net.Geometry.IGeometryObject GetGeometryObjectFromProductMetadata(
            SPP_ROOT productMetadata) {
            // need to convert Degrees Minutes Seconds to Decimal Degrees
            // example 0:15:22.185719


            GeoJSON.Net.Geometry.LineString lineString = new GeoJSON.Net.Geometry.LineString(
                new GeoJSON.Net.Geometry.Position[5]
                {
                    new GeoJSON.Net.Geometry.Position(ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ASWLat),
                        ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ASWLong)),
                    new GeoJSON.Net.Geometry.Position(ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ASELat),
                        ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ASELong)),
                    new GeoJSON.Net.Geometry.Position(ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ANELat),
                        ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ANELong)),
                    new GeoJSON.Net.Geometry.Position(ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ANWLat),
                        ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ANWLong)),
                    new GeoJSON.Net.Geometry.Position(ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ASWLat),
                        ConvertDegreeAngleToDegreeString(productMetadata.Geographic.ASWLong))
                }
            );

            return new GeoJSON.Net.Geometry.Polygon(new[] { lineString });
        }


        /*
        / TODO Retrieve geometry from shapefile
        private IGeometryObject GetGeometryObjectFromProductShapefile(IAsset shapefileAsset) {
            string shapeFilePath = shapefileAsset.Title;
            if (!File.Exists(shapeFilePath)) {
                throw new FileNotFoundException("Unable to find any shapefile asset");
            }

            // construct shapefile with the path to the .shp file
            using (Shapefile shapefile = new Shapefile(shapeFilePath)) {
                // enumerate all shapes
                Shape shape = shapefile.First();

                ShapePolygon shapePolygon = shape as ShapePolygon;
                foreach (PointD[] part in shapePolygon.Parts) {
                    Console.WriteLine("Polygon part:");
                    foreach (PointD point in part) {
                        Console.WriteLine("{0}, {1}", point.X, point.Y);
                    }

                    Console.WriteLine();
                }
            }


            return null;
        }*/

        private IDictionary<string, object> GetCommonMetadata(SPP_ROOT productMetadata, double gsd) {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            FillBitProperties(productMetadata, properties);
            FillDateTimeProperties(productMetadata, properties);
            FillInstrument(productMetadata, properties, gsd);

            return properties;
        }

        private void FillBitProperties(SPP_ROOT productMetadata, Dictionary<string, object> properties) {
            properties.Remove("bit");
            properties.Add("bit",productMetadata.Normal.NBitsPerPixel);
        }

        private void FillDateTimeProperties(SPP_ROOT productMetadata, Dictionary<string, object> properties) {
            CultureInfo provider = CultureInfo.InvariantCulture;
            string format = "dd/MM/yyyy HH:mm:ss.ffffff";
            DateTime.TryParseExact(
                String.Format("{0} {1}", productMetadata.Normal.DSceneDate, productMetadata.Normal.TSceneTime), format,
                provider, DateTimeStyles.AssumeUniversal, out var startDate);
            double deltaTime = Double.Parse(productMetadata.Normal.NDeltaTime);
            var endDate = startDate.AddMinutes(deltaTime);

            Itenso.TimePeriod.TimeInterval dateInterval = new Itenso.TimePeriod.TimeInterval(startDate, endDate);

            // remove previous values
            properties.Remove("datetime");
            properties.Remove("start_datetime");
            properties.Remove("end_datetime");

            // datetime, start_datetime, end_datetime
            if (dateInterval.IsAnytime) {
                properties.Add("datetime", null);
            }

            if (dateInterval.IsMoment) {
                properties.Add("datetime", dateInterval.Start.ToUniversalTime());
            }
            else {
                properties.Add("datetime", dateInterval.Start.ToUniversalTime());
                properties.Add("start_datetime", dateInterval.Start.ToUniversalTime());
                properties.Add("end_datetime", dateInterval.End.ToUniversalTime());
            }

            string dateformat = "dd/MM/yyyy";
            DateTime.TryParseExact(productMetadata.DDateHeaderFile, dateformat, provider, DateTimeStyles.AssumeUniversal,
                out var createdDate);

            if (createdDate.Ticks != 0) {
                properties.Remove("created");
                properties.Add("created", createdDate.ToUniversalTime());
            }

            properties.Remove("updated");
            properties.Add("updated", DateTime.UtcNow);
        }


        private void FillInstrument(SPP_ROOT productMetadata,
            Dictionary<string, object> properties, double gsd) {
            string platformName = "resurs-p"; //productMetadata.CCodeKA;
            if (!string.IsNullOrEmpty(platformName)) {
                properties.Remove("platform");
                properties.Add("platform", platformName);

                properties.Remove("mission");
                properties.Add("mission", platformName);
            }
            else {
                throw new InvalidDataException("Platform id not found or not recognized");
            }

            // instruments
            var instrumentName = "";

            if (productMetadata.Passport.CDeviceName.ToLower().Equals("geotonp")) {
                instrumentName = "geoton";
            }
            else {
                instrumentName = "kshmsa";
            }

            properties.Remove("instruments");
            properties.Add("instruments", new string[] { instrumentName });
            properties["sensor_type"] = "optical";
            properties.Remove("gsd");
            properties.Add("gsd", gsd);
        }


        private void AddProcessingStacExtension(SPP_ROOT productMetadata, StacItem stacItem) {
            var proc = stacItem.ProcessingExtension();
            proc.Level = productMetadata.Normal.CLevel;
        }

        private void AddViewStacExtension(SPP_ROOT productMetadata, StacItem stacItem) {
            var view = new ViewStacExtension(stacItem);

            view.Azimuth = ConvertDegreeAngleToDegreeDouble(productMetadata.Normal.AAzimutScan);
            view.SunAzimuth = ConvertDegreeAngleToDegreeDouble(productMetadata.Normal.ASunAzim);
            view.SunElevation = ConvertDegreeAngleToDegreeDouble(productMetadata.Normal.ASunElevC);
        }

        private void AddProjStacExtension(SPP_ROOT productMetaData, StacItem stacItem) {
            ProjectionStacExtension proj = stacItem.ProjectionExtension();
            proj.Epsg = long.Parse(productMetaData.CoordinateSystem.NCoordSystCode);
        }


        private async Task AddAssetsAsync(StacItem stacItem, IAssetsContainer assetsContainer, double gsd) {
            foreach (var asset in assetsContainer.Assets.Values.OrderBy(a => a.Uri.ToString())) {
                await AddAssetAsync(stacItem, asset, assetsContainer, gsd);
            }
        }

        private async Task AddAssetAsync(StacItem stacItem, IAsset asset,
            IAssetsContainer assetsContainer, double gsd) {
            string filename = Path.GetFileName(asset.Uri.ToString());
            string sensorName = filename.Split('_')[1];
            // thumbnail
            if (filename.EndsWith(".jpg", true, CultureInfo.InvariantCulture)) {
                stacItem.Assets.Add("thumbnail",
                    GetGenericAsset(stacItem, asset.Uri, "thumbnail"));
                stacItem.Assets["thumbnail"].Properties.AddRange(asset.Properties);
                return;
            }


            // metadata
            if (filename.EndsWith(".xml", true, CultureInfo.InvariantCulture)) {
                stacItem.Assets.Add("metadata",
                    GetGenericAsset(stacItem, asset.Uri, "metadata"));
                stacItem.Assets["metadata"].Properties.AddRange(asset.Properties);
                return;
            }


            if (filename.EndsWith(".tiff", true, CultureInfo.InvariantCulture)) {
                string mssBandName;

                if (asset.Uri.ToString().Contains("Geoton")) {
                    mssBandName = "MSS";
                }
                else {
                    mssBandName = "PMS";
                }
                var metadataAsset =
                    FindAssetsFromFileNameRegex(assetsContainer, ".*" + filename.Replace(".tiff", ".xml"));
                var bandAsset = GetBandAsset(stacItem, asset, gsd);
                stacItem.Assets.Add(mssBandName, bandAsset);
                return;
            }
        }

        private StacAsset GetGenericAsset(StacItem stacItem, Uri uri, string role) {
            StacAsset stacAsset = new StacAsset(stacItem, uri);
            stacAsset.Roles.Add(role);
            stacAsset.MediaType =
                new System.Net.Mime.ContentType(MimeTypes.GetMimeType(Path.GetFileName(uri.ToString())));
            return stacAsset;
        }


        private StacAsset GetBandAsset(StacItem stacItem, IAsset asset, double gsd) {
            StacAsset stacAsset = StacAsset.CreateDataAsset(stacItem, asset.Uri,
                new System.Net.Mime.ContentType(MimeTypes.GetMimeType(asset.Uri.ToString()))
            );
            stacAsset.Properties.AddRange(asset.Properties);
            stacAsset.SetProperty("gsd", gsd);

            if (asset.Uri.ToString().Contains("Geoton")) {
                //geoton
                EoBandObject b01EoBandObject =
                    CreateEoBandObject("channel-1", EoBandCommonName.blue, 0.5045, 0.045);
                EoBandObject b02EoBandObject =
                    CreateEoBandObject("channel-2", EoBandCommonName.green, 0.567, 0.052);
                EoBandObject b03EoBandObject =
                    CreateEoBandObject("channel-3", EoBandCommonName.red, 0.6475, 0.063);
                EoBandObject b04EoBandObject =
                    CreateEoBandObject("channel-4", EoBandCommonName.nir, 0.761, 0.086);
                stacAsset.EoExtension().Bands = new[]
                    { b01EoBandObject, b02EoBandObject, b03EoBandObject, b04EoBandObject };
            }
            else {
                //KShMSA
                EoBandObject b01EoBandObject =
                    CreateEoBandObject("channel-1", EoBandCommonName.blue, 0.47, 0.08);
                EoBandObject b02EoBandObject =
                    CreateEoBandObject("channel-2", EoBandCommonName.green, 0.545, 0.07);
                EoBandObject b03EoBandObject =
                    CreateEoBandObject("channel-3", EoBandCommonName.red, 0.65, 0.1);
                EoBandObject b04EoBandObject =
                    CreateEoBandObject("channel-4", EoBandCommonName.nir, 0.8, 0.2);
                EoBandObject b05EoBandObject =
                    CreateEoBandObject("channel-5", EoBandCommonName.nir08, 0.85, 0.1);
                stacAsset.EoExtension().Bands = new[]
                    { b01EoBandObject, b02EoBandObject, b03EoBandObject, b04EoBandObject, b05EoBandObject };
            }

            return stacAsset;
        }

        private EoBandObject CreateEoBandObject(string name, EoBandCommonName eoBandCommonName, double centerWaveLength,
            double fullWidthHalfMax, double? eai = null) {
            EoBandObject eoBandObject = new EoBandObject(name, eoBandCommonName);
            eoBandObject.Properties.Add("full_width_half_max", fullWidthHalfMax);
            if (eai != null) {
                eoBandObject.SolarIllumination = eai;
            }

            eoBandObject.CenterWavelength = centerWaveLength;
            return eoBandObject;
        }

        private RasterBand CreateRasterBandObject(double offset, double gain) {
            RasterBand rasterBandObject = new RasterBand();
            rasterBandObject.Offset = offset;
            rasterBandObject.Scale = gain;
            return rasterBandObject;
        }


        private void FillBasicsProperties(IDictionary<string, object> properties) {
            CultureInfo culture = CultureInfo.InvariantCulture;
            // title
            properties.Remove("title");
            properties.Add("title", string.Format("{0} {1} {2} {3}",
                StylePlatform(properties.GetProperty<string>("platform")),
                properties.GetProperty<string[]>("instruments").First().ToUpper(),
                properties.GetProperty<string>("processing:level").ToUpper(),
                properties.GetProperty<DateTime>("datetime").ToUniversalTime().ToString("G", culture)));
        }
    }
}