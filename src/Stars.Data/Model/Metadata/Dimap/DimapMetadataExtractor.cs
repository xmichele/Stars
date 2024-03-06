using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GeoJSON.Net.Geometry;
using Microsoft.Extensions.Logging;
using Stac;
using Stac.Extensions.Eo;
using Stac.Extensions.Processing;
using Stac.Extensions.Projection;
using Stac.Extensions.Sat;
using Stac.Extensions.View;
using Terradue.Stars.Data.Model.Metadata.Dimap.Schemas;
using Terradue.Stars.Interface;
using Terradue.Stars.Interface.Supplier.Destination;
using Terradue.Stars.Services;
using Terradue.Stars.Services.Model.Stac;
using Terradue.Stars.Geometry.GeoJson;

namespace Terradue.Stars.Data.Model.Metadata.Dimap
{
    public class DimapMetadataExtractor : MetadataExtraction
    {
        public static XmlSerializer metadataSerializer = new XmlSerializer(typeof(Schemas.t_Dimap_Document));
        public static XmlSerializer metadataAltSerializer = new XmlSerializer(typeof(Schemas.t_Metadata_Document));

        public override string Label => "Generic DIMAP product metadata extractor";

        public DimapMetadataExtractor(ILogger<DimapMetadataExtractor> logger, IResourceServiceProvider resourceServiceProvider) : base(logger, resourceServiceProvider)
        {
        }

        public override bool CanProcess(IResource route, IDestination destination)
        {
            IItem item = route as IItem;
            if (item == null) return false;
            try
            {
                IAsset[] metadataAssets = GetMetadataAssets(item);
                Schemas.DimapDocument[] metadata = ReadMetadata(metadataAssets).GetAwaiter().GetResult();
                var dimapProfiler = GetProfiler(metadata);
                return dimapProfiler != null;
            }
            catch
            {
                return false;
            }
        }

        private DimapProfiler GetProfiler(DimapDocument[] dimapDocuments)
        {
            int profiler = 0;
            foreach (DimapDocument dimap in dimapDocuments)
            {
                int current = 0;
                if (dimap.Metadata_Id.METADATA_PROFILE == "DMCii")
                {
                    current = 1;
                }
                else if (dimap.Dataset_Id != null && dimap.Dataset_Id.DATASET_NAME.StartsWith("vis1", true, CultureInfo.InvariantCulture))
                {
                    current = 2;
                }
                else if (dimap.Dataset_Id != null && dimap.Dataset_Id.DATASET_NAME.StartsWith("ab_", true, CultureInfo.InvariantCulture))
                {
                    current = 3;
                }
                else
                {
                    current = 4;
                }

                if (profiler == 0) profiler = current;
                else if (current != profiler) throw new Exception("Inconsistent metadata documents");
            }

            switch (profiler)
            {
                case 1:
                    return new DMC.DmcDimapProfiler(dimapDocuments);
                case 2:
                    return new DMC.Vision1DimapProfiler(dimapDocuments);
                case 3:
                    return new DMC.Alsat1BDimapProfiler(dimapDocuments);
                default:
                    return new GenericDimapProfiler(dimapDocuments);
            }
        }

        protected override async Task<StacNode> ExtractMetadata(IItem item, string suffix)
        {
            IAsset[] metadataAssets = GetMetadataAssets(item);
            Schemas.DimapDocument[] metadata = await ReadMetadata(metadataAssets);

            DimapProfiler dimapProfiler = GetProfiler(metadata);

            StacItem stacItem = CreateStacItem(dimapProfiler);

            AddAssets(stacItem, item, dimapProfiler);

            // AddEoBandPropertyInItem(stacItem);

            return StacItemNode.Create(stacItem, item.Uri);
        }

        private void AddEoBandPropertyInItem(StacItem stacItem)
        {
            var eo = stacItem.EoExtension();
            eo.Bands = stacItem.Assets.Values.Where(a => a.EoExtension().Bands != null).SelectMany(a => a.EoExtension().Bands).ToArray();
        }

        internal virtual StacItem CreateStacItem(DimapProfiler dimapProfiler)
        {
            bool multipleProducts = dimapProfiler.Dimaps.Length > 1;
            string identifier = dimapProfiler.Dimaps[0].Dataset_Id.DATASET_NAME;
            if (multipleProducts)
            {
                identifier = identifier.Replace("_MS4_", "_").Replace("_PAN_", "_");
            }

            StacItem stacItem = new StacItem(identifier, GetGeometry(dimapProfiler), GetCommonMetadata(dimapProfiler));
            AddSatStacExtension(dimapProfiler, stacItem);
            AddProjStacExtension(dimapProfiler, stacItem);
            AddViewStacExtension(dimapProfiler, stacItem);
            AddProcessingStacExtension(dimapProfiler, stacItem);
            AddEoStacExtension(dimapProfiler, stacItem);
            FillBasicsProperties(dimapProfiler, stacItem.Properties);
            AddOtherProperties(dimapProfiler, stacItem.Properties);
            return stacItem;
        }

        private void AddEoStacExtension(DimapProfiler dimapProfiler, StacItem stacItem)
        {
            EoStacExtension eo = stacItem.EoExtension();
        }

        private void AddProcessingStacExtension(DimapProfiler dimapProfiler, StacItem stacItem)
        {
            var proc = stacItem.ProcessingExtension();
            if (!string.IsNullOrEmpty(dimapProfiler.GetProcessingLevel()))
                proc.Level = dimapProfiler.GetProcessingLevel();
            dimapProfiler.AddProcessingSoftware(proc.Software);
        }

        private void AddProjStacExtension(DimapProfiler dimapProfiler, StacItem stacItem)
        {
            var epsg = dimapProfiler.GetProjection();
            ProjectionStacExtension proj = stacItem.ProjectionExtension();
            if (epsg == 0) proj.Epsg = null;
            else proj.Epsg = epsg;
            proj.Shape = dimapProfiler.GetShape();
        }

        private void AddViewStacExtension(DimapProfiler dimapProfiler, StacItem stacItem)
        {
            var view = new ViewStacExtension(stacItem);
            double viewingAngle = dimapProfiler.GetViewingAngle();
            double sunAzimuth = dimapProfiler.GetSunAngle();
            double sunElevation = dimapProfiler.GetSunElevation();
            double incidenceAngle = dimapProfiler.GetIndidenceAngle();
            if (viewingAngle != 0) view.Azimuth = viewingAngle;
            if (sunAzimuth != 0) view.SunAzimuth = sunAzimuth;
            if (sunElevation != 0) view.SunElevation = sunElevation;
            if (incidenceAngle != 0) view.IncidenceAngle = incidenceAngle;
        }

        private void AddSatStacExtension(DimapProfiler dimapProfiler, StacItem stacItem)
        {
            var sat = new SatStacExtension(stacItem);
            if (dimapProfiler.GetAbsoluteOrbit().HasValue)
            {
                sat.AbsoluteOrbit = dimapProfiler.GetAbsoluteOrbit().Value;
            }
            if (dimapProfiler.GetRelativeOrbit().HasValue)
            {
                sat.RelativeOrbit = dimapProfiler.GetRelativeOrbit().Value;
            }
            sat.OrbitState = dimapProfiler.GetOrbitState();
            string pid = dimapProfiler.GetPlatformInternationalDesignator();
            if (pid != null) sat.PlatformInternationalDesignator = pid;
        }

        private IDictionary<string, object> GetCommonMetadata(DimapProfiler dimapProfiler)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();

            FillDateTimeProperties(dimapProfiler, properties);
            // TODO Licensing
            // TODO Provider
            FillInstrument(dimapProfiler, properties);
            FillBasicsProperties(dimapProfiler, properties);

            return properties;
        }

        private void FillInstrument(DimapProfiler dimapProfiler, Dictionary<string, object> properties)
        {
            // platform & constellation
            properties.Remove("platform");
            properties.Add("platform", dimapProfiler.GetPlatform().ToLower());

            properties.Remove("constellation");
            properties.Add("constellation", dimapProfiler.GetConstellation().ToLower());

            properties.Remove("mission");
            properties.Add("mission", dimapProfiler.GetMission().ToLower());

            // instruments
            properties.Remove("instruments");
            properties.Add("instruments", dimapProfiler.GetInstruments().Select(i => i.ToLower()).ToArray());

            string spectralProcessing = dimapProfiler.GetSpectralProcessing();
            if (spectralProcessing != null) properties["spectral_processing"] = spectralProcessing;
            properties["sensor_type"] = dimapProfiler.GetSensorMode();

            properties.Remove("gsd");
            properties.Add("gsd", dimapProfiler.GetResolution());
        }

        private void FillDateTimeProperties(DimapProfiler dimapProfiler, Dictionary<string, object> properties)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime startDate = dimapProfiler.GetStartTime();
            DateTime endDate = dimapProfiler.GetEndTime();
            if (startDate.Ticks == 0 && endDate.Ticks == 0)
            {
                startDate = dimapProfiler.GetAcquisitionTime();
                endDate = startDate;
            }

            Itenso.TimePeriod.TimeInterval dateInterval = new Itenso.TimePeriod.TimeInterval(startDate, endDate);

            // remove previous values
            properties.Remove("datetime");
            properties.Remove("start_datetime");
            properties.Remove("end_datetime");

            // datetime, start_datetime, end_datetime
            if (dateInterval.IsAnytime)
            {
                properties.Add("datetime", null);
            }

            if (dateInterval.IsMoment)
            {
                properties.Add("datetime", dateInterval.Start);
            }
            else
            {
                properties.Add("datetime", dateInterval.Start);
                properties.Add("start_datetime", dateInterval.Start);
                properties.Add("end_datetime", dateInterval.End);
            }

            DateTime createdDate = dimapProfiler.GetProcessingTime();

            if (createdDate.Ticks != 0)
            {
                properties.Remove("created");
                properties.Add("created", createdDate);
            }

            properties.Remove("updated");
            properties.Add("updated", DateTime.UtcNow);
        }

        private void FillBasicsProperties(DimapProfiler dimapProfiler, IDictionary<String, object> properties)
        {
            CultureInfo culture = new CultureInfo("fr-FR");
            // title
            properties.Remove("title");
            properties.Add("title", dimapProfiler.GetTitle(properties));
        }

        private void AddOtherProperties(DimapProfiler dimapProfiler, IDictionary<string, object> properties)
        {
            if (IncludeProviderProperty)
            {
                StacProvider provider = dimapProfiler.GetStacProvider();
                if (provider != null) properties.Add("providers", new StacProvider[] { provider });
            }
        }

        private GeoJSON.Net.Geometry.IGeometryObject GetGeometry(DimapProfiler dimapProfiler)
        {
            List<GeoJSON.Net.Geometry.Position> positions = new List<Position>();
            foreach (var vertex in dimapProfiler.Dimap.Dataset_Frame)
            {
                positions.Add(new GeoJSON.Net.Geometry.Position(
                    vertex.FRAME_LAT.Value, vertex.FRAME_LON.Value
                )
                );
            }
            positions.Add(positions.First());

            GeoJSON.Net.Geometry.LineString lineString = new GeoJSON.Net.Geometry.LineString(
                positions.ToArray()
            );

            return new GeoJSON.Net.Geometry.Polygon(new GeoJSON.Net.Geometry.LineString[] { lineString }).NormalizePolygon();
        }

        protected void AddAssets(StacItem stacItem, IItem item, DimapProfiler dimapProfiler)
        {
            foreach (DimapDocument dimap in dimapProfiler.Dimaps)
            {
                t_Data_File[] dataFiles = dimap.Data_Access.Data_File;
                if (dataFiles == null && dimap.Data_Access.Data_Files != null)
                {
                    dataFiles = dimap.Data_Access.Data_Files.Data_File;
                }

                if (dataFiles == null)
                {
                    throw new Exception("No references to data files found in metadata");
                }

                foreach (var dataFile in dataFiles)
                {
                    IAsset productAsset = FindFirstAssetFromFileNameRegex(item, dataFile.DATA_FILE_PATH.href + "$");
                    if (productAsset == null)
                        throw new FileNotFoundException(string.Format("No product found '{0}'", dataFile.DATA_FILE_PATH.href));
                    var bandStacAsset = CreateRasterAsset(stacItem, productAsset, dimapProfiler, dataFile, dimap);
                    if (dimap.Data_Access.DATA_FILE_ORGANISATION == t_DATA_FILE_ORGANISATION.BAND_SEPARATE)
                        dimapProfiler.CompleteAsset(bandStacAsset.Value,
                            new t_Spectral_Band_Info[1] { dimap.Image_Interpretation.FirstOrDefault(sb => sb.BAND_INDEX == dataFile.BAND_INDEX) },
                            dimap.Raster_Encoding, dimap);
                    else
                        dimapProfiler.CompleteAsset(bandStacAsset.Value, dimap.Image_Interpretation, dimap.Raster_Encoding, dimap);
                    stacItem.Assets.Add(bandStacAsset.Key, bandStacAsset.Value);
                }
            }

            IAsset[] metadataAssets = GetMetadataAssets(item);
            foreach (IAsset metadataAsset in metadataAssets)
            {
                string prefix = dimapProfiler.GetAssetPrefix(null, metadataAsset);
                string key = String.Format("{0}metadata", prefix);
                stacItem.Assets.Add(key, StacAsset.CreateMetadataAsset(stacItem, metadataAsset.Uri,
                            new ContentType("application/xml"), "Metadata file"));
                stacItem.Assets[key].Properties.AddRange(metadataAsset.Properties);
            }
            foreach (DimapDocument dimap in dimapProfiler.Dimaps)
            {
                string prefix = dimapProfiler.GetAssetPrefix(dimap);
                try
                {
                    var overviewAsset = FindFirstAssetFromFileNameRegex(item, dimap.Dataset_Id.DATASET_QL_PATH.href);
                    if (overviewAsset != null)
                    {
                        string key = String.Format("{0}overview", prefix);
                        if (stacItem.Assets.TryAdd(key, StacAsset.CreateOverviewAsset(stacItem, overviewAsset.Uri,
                                    new ContentType(MimeTypes.GetMimeType(Path.GetFileName(overviewAsset.Uri.ToString()))))))
                            stacItem.Assets[key].Properties.AddRange(overviewAsset.Properties);
                    }
                }
                catch { }
                try
                {
                    var thumbnailAsset = FindFirstAssetFromFileNameRegex(item, dimap.Dataset_Id.DATASET_TN_PATH.href);
                    if (thumbnailAsset != null)
                    {
                        string key = String.Format("{0}thumbnail", prefix);
                        stacItem.Assets.Add(key, StacAsset.CreateThumbnailAsset(stacItem, thumbnailAsset.Uri,
                                    new ContentType(MimeTypes.GetMimeType(Path.GetFileName(thumbnailAsset.Uri.ToString())))));
                        stacItem.Assets[key].Properties.AddRange(thumbnailAsset.Properties);
                    }
                }
                catch{}
            }
        }

        private KeyValuePair<string, StacAsset> CreateRasterAsset(StacItem stacItem, IAsset bandAsset, DimapProfiler dimapProfiler, t_Data_File dataFile, Schemas.DimapDocument dimap)
        {
            string mimeType = MimeTypes.GetMimeType(Path.GetFileName(bandAsset.Uri.ToString()));
            StacAsset stacAsset = StacAsset.CreateDataAsset(stacItem, bandAsset.Uri, new ContentType(MimeTypes.GetMimeType(Path.GetFileName(bandAsset.Uri.ToString()))));
            stacAsset.Properties.AddRange(bandAsset.Properties);
            stacAsset.Title = dimapProfiler.GetAssetTitle(bandAsset, dataFile, dimap);
            return new KeyValuePair<string, StacAsset>(dimapProfiler.GetProductKey(bandAsset, dataFile), stacAsset);
        }

        protected virtual IAsset[] GetMetadataAssets(IItem item)
        {
            IEnumerable<IAsset> manifestAssets = this.FindAssetsFromFileNameRegex(item, @".*\.dim$");
            if (manifestAssets == null ||  manifestAssets.Count() == 0)
            {
                manifestAssets = FindAssetsFromFileNameRegex(item, @"(DIM.*|.*Meta)\.xml$");
                if (manifestAssets == null || manifestAssets.Count() == 0)
                    throw new FileNotFoundException(String.Format("Unable to find the metadata file asset(s)"));
            }
            return manifestAssets.ToArray();
        }

        public virtual async Task<Schemas.DimapDocument[]> ReadMetadata(IEnumerable<IAsset> metadataAssets)
        {
            List<Schemas.DimapDocument> metadata = new List<Schemas.DimapDocument>();

            foreach (IAsset metadataAsset in metadataAssets)
            {
                logger.LogDebug("Opening manifest {0}", metadataAsset.Uri);

                using (var stream = await resourceServiceProvider.GetAssetStreamAsync(metadataAsset, System.Threading.CancellationToken.None))
                {
                    var reader = XmlReader.Create(stream);
                    logger.LogDebug("Deserializing manifest {0}", metadataAsset.Uri);

                    Schemas.DimapDocument singleDimap;
                    try
                    {
                        singleDimap = (Schemas.DimapDocument)metadataSerializer.Deserialize(reader);
                    }
                    catch
                    {
                        singleDimap = (Schemas.t_Metadata_Document)metadataAltSerializer.Deserialize(reader);
                    }

                    metadata.Add(singleDimap);

                }
            }
            return metadata.ToArray();
        }


        protected void AddManifestAsset(StacItem stacItem, IAsset asset)
        {
            StacAsset stacAsset = StacAsset.CreateMetadataAsset(stacItem, asset.Uri, new ContentType("text/xml"), "SAFE Manifest");
            stacAsset.Properties.AddRange(asset.Properties);
            stacItem.Assets.Add("manifest", stacAsset);
        }
    }
}
