//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
//This source code was auto-generated by MonoXSD
//
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Terradue.Stars.Data.Model.Metadata.Airbus.Schemas
{

  [XmlRoot(ElementName = "METADATA_FORMAT")]
    public class METADATA_FORMAT {

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "Metadata_Identification")]
    public class Metadata_Identification {

        [XmlElement(ElementName = "METADATA_FORMAT")]
        public METADATA_FORMAT METADATA_FORMAT { get; set; }

        [XmlElement(ElementName = "METADATA_PROFILE")]
        public string METADATA_PROFILE { get; set; }

        [XmlElement(ElementName = "METADATA_SUBPROFILE")]
        public string METADATA_SUBPROFILE { get; set; }

        [XmlElement(ElementName = "METADATA_LANGUAGE")]
        public string METADATA_LANGUAGE { get; set; }

    }

    [XmlRoot(ElementName = "DATASET_NAME")]
    public class DATASET_NAME {

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "DATASET_QL_PATH")]
    public class DATASET_QL_PATH {

        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }

    }

    [XmlRoot(ElementName = "Legal_Constraints")]
    public class Legal_Constraints {

        [XmlElement(ElementName = "COPYRIGHT")]
        public string COPYRIGHT { get; set; }

        [XmlElement(ElementName = "TRADEMARK")]
        public string TRADEMARK { get; set; }

        [XmlElement(ElementName = "LICENSE")]
        public string LICENSE { get; set; }

        [XmlElement(ElementName = "RESTRICTIONS")]
        public string RESTRICTIONS { get; set; }

    }

    [XmlRoot(ElementName = "Security_Constraints")]
    public class Security_Constraints {

        [XmlElement(ElementName = "CLASSIFICATION_LEVEL")]
        public string CLASSIFICATION_LEVEL { get; set; }

        [XmlElement(ElementName = "CLASSIFICATION_COMMENTS")]
        public string CLASSIFICATION_COMMENTS { get; set; }

        [XmlElement(ElementName = "RESTRICTIONS")]
        public string RESTRICTIONS { get; set; }

    }

    [XmlRoot(ElementName = "Dataset_Identification")]
    public class Dataset_Identification {

        [XmlElement(ElementName = "DATASET_ID")]
        public string DATASET_ID { get; set; }

        [XmlElement(ElementName = "DATASET_TYPE")]
        public string DATASET_TYPE { get; set; }

        [XmlElement(ElementName = "DATASET_NAME")]
        public DATASET_NAME DATASET_NAME { get; set; }

        [XmlElement(ElementName = "DATASET_TN_PATH")]
        public DATASET_TN_PATH DATASET_TN_PATH { get; set; }

        [XmlElement(ElementName = "DATASET_QL_PATH")]
        public DATASET_QL_PATH DATASET_QL_PATH { get; set; }

        [XmlElement(ElementName = "DATASET_QL_FORMAT")]
        public string DATASET_QL_FORMAT { get; set; }

        [XmlElement(ElementName = "Legal_Constraints")]
        public Legal_Constraints Legal_Constraints { get; set; }

        [XmlElement(ElementName = "Security_Constraints")]
        public Security_Constraints Security_Constraints { get; set; }

    }

    [XmlRoot(ElementName = "SURFACE_AREA")]
    public class SURFACE_AREA {

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "CLOUD_COVERAGE")]
    public class CLOUD_COVERAGE {

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "SNOW_COVERAGE")]
    public class SNOW_COVERAGE {

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "COMPONENT_PATH")]
    public class COMPONENT_PATH {

        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }

    }

    [XmlRoot(ElementName = "Component")]
    public class Component {

        [XmlElement(ElementName = "COMPONENT_TITLE")]
        public string COMPONENT_TITLE { get; set; }

        [XmlElement(ElementName = "COMPONENT_CONTENT")]
        public string COMPONENT_CONTENT { get; set; }

        [XmlElement(ElementName = "COMPONENT_TYPE")]
        public string COMPONENT_TYPE { get; set; }

        [XmlElement(ElementName = "COMPONENT_PATH")]
        public COMPONENT_PATH COMPONENT_PATH { get; set; }

    }


    [XmlRoot(ElementName = "Dataset_Components")]
    public class Dataset_Components {

        [XmlElement(ElementName = "Component")]
        public List<Component> Component { get; set; }

    }



    [XmlRoot(ElementName = "Vertex")]
    public class Vertex {

        [XmlElement(ElementName = "LON")]
        public double LON { get; set; }

        [XmlElement(ElementName = "LAT")]
        public double LAT { get; set; }

        [XmlElement(ElementName = "X")]
        public double X { get; set; }

        [XmlElement(ElementName = "Y")]
        public double Y { get; set; }

        [XmlElement(ElementName = "COL")]
        public double COL { get; set; }

        [XmlElement(ElementName = "ROW")]
        public double ROW { get; set; }

    }

    [XmlRoot(ElementName = "Center")]
    public class Center {

        [XmlElement(ElementName = "LON")]
        public string LON { get; set; }

        [XmlElement(ElementName = "LAT")]
        public string LAT { get; set; }

        [XmlElement(ElementName = "X")]
        public string X { get; set; }

        [XmlElement(ElementName = "Y")]
        public string Y { get; set; }

        [XmlElement(ElementName = "COL")]
        public string COL { get; set; }

        [XmlElement(ElementName = "ROW")]
        public string ROW { get; set; }

    }

    [XmlRoot(ElementName = "Dataset_Extent")]
    public class Dataset_Extent {

        [XmlElement(ElementName = "EXTENT_TYPE")]
        public string EXTENT_TYPE { get; set; }

        [XmlElement(ElementName = "Vertex")]
        public List<Vertex> Vertex { get; set; }

        [XmlElement(ElementName = "Center")]
        public Center Center { get; set; }

    }

    [XmlRoot(ElementName = "Dataset_Content")]
    public class Dataset_Content {

        [XmlElement(ElementName = "SURFACE_AREA")]
        public SURFACE_AREA SURFACE_AREA { get; set; }

        [XmlElement(ElementName = "CLOUD_COVERAGE")]
        public CLOUD_COVERAGE CLOUD_COVERAGE { get; set; }

        [XmlElement(ElementName = "SNOW_COVERAGE")]
        public SNOW_COVERAGE SNOW_COVERAGE { get; set; }

        [XmlElement(ElementName = "Dataset_Components")]
        public Dataset_Components Dataset_Components { get; set; }

        [XmlElement(ElementName = "Dataset_Extent")]
        public Dataset_Extent Dataset_Extent { get; set; }

    }

    [XmlRoot(ElementName = "PRODUCER_URL")]
    public class PRODUCER_URL {

        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }

    }

    [XmlRoot(ElementName = "Producer_Information")]
    public class Producer_Information {

        [XmlElement(ElementName = "PRODUCER_NAME")]
        public string PRODUCER_NAME { get; set; }

        [XmlElement(ElementName = "PRODUCER_URL")]
        public PRODUCER_URL PRODUCER_URL { get; set; }

        [XmlElement(ElementName = "PRODUCER_CONTACT")]
        public string PRODUCER_CONTACT { get; set; }

        [XmlElement(ElementName = "PRODUCER_ADDRESS")]
        public string PRODUCER_ADDRESS { get; set; }

    }

    [XmlRoot(ElementName = "DISTRIBUTOR_URL")]
    public class DISTRIBUTOR_URL {

        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }

    }

    [XmlRoot(ElementName = "Distributor_Information")]
    public class Distributor_Information {

        [XmlElement(ElementName = "DISTRIBUTOR_NAME")]
        public string DISTRIBUTOR_NAME { get; set; }

        [XmlElement(ElementName = "DISTRIBUTOR_URL")]
        public DISTRIBUTOR_URL DISTRIBUTOR_URL { get; set; }

        [XmlElement(ElementName = "DISTRIBUTOR_CONTACT")]
        public string DISTRIBUTOR_CONTACT { get; set; }

        [XmlElement(ElementName = "DISTRIBUTOR_ADDRESS")]
        public string DISTRIBUTOR_ADDRESS { get; set; }

    }

    [XmlRoot(ElementName = "Order_Identification")]
    public class Order_Identification {

        [XmlElement(ElementName = "CUSTOMER_REFERENCE")]
        public string CUSTOMER_REFERENCE { get; set; }

        [XmlElement(ElementName = "INTERNAL_REFERENCE")]
        public string INTERNAL_REFERENCE { get; set; }

        [XmlElement(ElementName = "COMMERCIAL_REFERENCE")]
        public string COMMERCIAL_REFERENCE { get; set; }

        [XmlElement(ElementName = "COMMERCIAL_ITEM")]
        public string COMMERCIAL_ITEM { get; set; }

        [XmlElement(ElementName = "COMMENT")]
        public string COMMENT { get; set; }

    }

    [XmlRoot(ElementName = "Delivery_Identification")]
    public class Delivery_Identification {

        [XmlElement(ElementName = "PRODUCTION_DATE")]
        public string PRODUCTION_DATE { get; set; }

        [XmlElement(ElementName = "JOB_ID")]
        public string JOB_ID { get; set; }

        [XmlElement(ElementName = "PRODUCT_CODE")]
        public string PRODUCT_CODE { get; set; }

        [XmlElement(ElementName = "PRODUCT_TYPE")]
        public string PRODUCT_TYPE { get; set; }

        [XmlElement(ElementName = "PRODUCT_INFO")]
        public string PRODUCT_INFO { get; set; }

        [XmlElement(ElementName = "DELIVERY_TYPE")]
        public string DELIVERY_TYPE { get; set; }

        [XmlElement(ElementName = "Order_Identification")]
        public Order_Identification Order_Identification { get; set; }

    }

    [XmlRoot(ElementName = "Product_Information")]
    public class Product_Information {

        [XmlElement(ElementName = "Producer_Information")]
        public Producer_Information Producer_Information { get; set; }

        [XmlElement(ElementName = "Distributor_Information")]
        public Distributor_Information Distributor_Information { get; set; }

        [XmlElement(ElementName = "Delivery_Identification")]
        public Delivery_Identification Delivery_Identification { get; set; }

    }

    [XmlRoot(ElementName = "Projected_CRS")]
    public class Projected_CRS {

        [XmlElement(ElementName = "PROJECTED_CRS_NAME")]
        public string PROJECTED_CRS_NAME { get; set; }

        [XmlElement(ElementName = "PROJECTED_CRS_CODE")]
        public string PROJECTED_CRS_CODE { get; set; }

    }

    [XmlRoot(ElementName = "Geodetic_CRS")]
    public class Geodetic_CRS {

        [XmlElement(ElementName = "CRS_TABLES")]
        public string CRS_TABLES { get; set; }

        [XmlElement(ElementName = "GEODETIC_CRS_TYPE")]
        public string GEODETIC_CRS_TYPE { get; set; }

        [XmlElement(ElementName = "GEODETIC_CRS_NAME")]
        public string GEODETIC_CRS_NAME { get; set; }

        [XmlElement(ElementName = "GEODETIC_CRS_CODE")]
        public string GEODETIC_CRS_CODE { get; set; }

    }

    [XmlRoot(ElementName = "CRS_TABLES")]
    public class CRS_TABLES {

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "Temporal_CRS")]
    public class Temporal_CRS {

        [XmlElement(ElementName = "CRS_TABLES")]
        public CRS_TABLES CRS_TABLES { get; set; }

        [XmlElement(ElementName = "TEMPORAL_CRS_NAME")]
        public string TEMPORAL_CRS_NAME { get; set; }

    }

    [XmlRoot(ElementName = "Coordinate_Reference_System")]
    public class Coordinate_Reference_System {

        [XmlElement(ElementName = "Projected_CRS")]
        public Projected_CRS Projected_CRS { get; set; }

        [XmlElement(ElementName = "Geodetic_CRS")]
        public Geodetic_CRS Geodetic_CRS { get; set; }

        [XmlElement(ElementName = "Temporal_CRS")]
        public Temporal_CRS Temporal_CRS { get; set; }

    }

    [XmlRoot(ElementName = "Raster_CRS")]
    public class Raster_CRS {

        [XmlElement(ElementName = "RASTER_GEOMETRY")]
        public string RASTER_GEOMETRY { get; set; }

        [XmlElement(ElementName = "PIXEL_ORIENTATION")]
        public string PIXEL_ORIENTATION { get; set; }

        [XmlElement(ElementName = "PIXEL_CRS_TYPE")]
        public string PIXEL_CRS_TYPE { get; set; }

        [XmlElement(ElementName = "PIXEL_ORIGIN")]
        public string PIXEL_ORIGIN { get; set; }

    }

    [XmlRoot(ElementName = "Geoposition_Insert")]
    public class Geoposition_Insert {

        [XmlElement(ElementName = "ULXMAP")]
        public string ULXMAP { get; set; }

        [XmlElement(ElementName = "ULYMAP")]
        public string ULYMAP { get; set; }

        [XmlElement(ElementName = "XDIM")]
        public string XDIM { get; set; }

        [XmlElement(ElementName = "YDIM")]
        public string YDIM { get; set; }

    }

    [XmlRoot(ElementName = "Geoposition")]
    public class Geoposition {

        [XmlElement(ElementName = "Raster_CRS")]
        public Raster_CRS Raster_CRS { get; set; }

        [XmlElement(ElementName = "Geoposition_Insert")]
        public Geoposition_Insert Geoposition_Insert { get; set; }

    }

    [XmlRoot(ElementName = "SOFTWARE")]
    public class SOFTWARE {

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "Production_Facility")]
    public class Production_Facility {

        [XmlElement(ElementName = "SOFTWARE")]
        public SOFTWARE SOFTWARE { get; set; }

        [XmlElement(ElementName = "PROCESSING_CENTER")]
        public string PROCESSING_CENTER { get; set; }

    }

    [XmlRoot(ElementName = "Geometric_Settings")]
    public class Geometric_Settings {

        [XmlElement(ElementName = "GEOMETRIC_PROCESSING")]
        public string GEOMETRIC_PROCESSING { get; set; }

        [XmlElement(ElementName = "EPHEMERIS_USED")]
        public string EPHEMERIS_USED { get; set; }

        [XmlElement(ElementName = "ATTITUDES_USED")]
        public string ATTITUDES_USED { get; set; }

        [XmlElement(ElementName = "GROUND_SETTING")]
        public string GROUND_SETTING { get; set; }

        [XmlElement(ElementName = "GROUND_DESC")]
        public string GROUND_DESC { get; set; }

        [XmlElement(ElementName = "VERTICAL_SETTING")]
        public string VERTICAL_SETTING { get; set; }

        [XmlElement(ElementName = "VERTICAL_DESC")]
        public string VERTICAL_DESC { get; set; }

    }

    [XmlRoot(ElementName = "Radiometric_Settings")]
    public class Radiometric_Settings {

        [XmlElement(ElementName = "RADIOMETRIC_PROCESSING")]
        public string RADIOMETRIC_PROCESSING { get; set; }

    }

    [XmlRoot(ElementName = "RESAMPLING_SPACING")]
    public class RESAMPLING_SPACING {

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "Sampling_Settings")]
    public class Sampling_Settings {

        [XmlElement(ElementName = "RESAMPLING_SPACING")]
        public RESAMPLING_SPACING RESAMPLING_SPACING { get; set; }

        [XmlElement(ElementName = "RESAMPLING_KERNEL")]
        public string RESAMPLING_KERNEL { get; set; }

    }

    [XmlRoot(ElementName = "MTF_Settings")]
    public class MTF_Settings {

        public string PAN_RESTORATION { get; set; }

        [XmlElement(ElementName = "MS_RESTORATION")]
        public string MS_RESTORATION { get; set; }

    }

    [XmlRoot(ElementName = "Product_Settings")]
    public class Product_Settings {

        [XmlElement(ElementName = "PROCESSING_LEVEL")]
        public string PROCESSING_LEVEL { get; set; }

        [XmlElement(ElementName = "SPECTRAL_PROCESSING")]
        public string SPECTRAL_PROCESSING { get; set; }

        [XmlElement(ElementName = "Geometric_Settings")]
        public Geometric_Settings Geometric_Settings { get; set; }

        [XmlElement(ElementName = "Radiometric_Settings")]
        public Radiometric_Settings Radiometric_Settings { get; set; }

        [XmlElement(ElementName = "Sampling_Settings")]
        public Sampling_Settings Sampling_Settings { get; set; }

        [XmlElement(ElementName = "MTF_Settings")]
        public MTF_Settings MTF_Settings { get; set; }

    }

    [XmlRoot(ElementName = "Processing_Lineage")]
    public class Processing_Lineage {

        [XmlElement(ElementName = "Component")]
        public Component Component { get; set; }

    }

    [XmlRoot(ElementName = "Processing_Information")]
    public class Processing_Information {

        [XmlElement(ElementName = "Production_Facility")]
        public Production_Facility Production_Facility { get; set; }

        [XmlElement(ElementName = "Product_Settings")]
        public Product_Settings Product_Settings { get; set; }

        [XmlElement(ElementName = "Processing_Lineage")]
        public Processing_Lineage Processing_Lineage { get; set; }
    }

    [XmlRoot(ElementName = "DATA_FILE_PATH")]
    public class DATA_FILE_PATH {

        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }

    }

    [XmlRoot(ElementName = "Data_File")]
    public class Data_File {

        [XmlElement(ElementName = "DATA_FILE_PATH")]
        public DATA_FILE_PATH DATA_FILE_PATH { get; set; }

        [XmlAttribute(AttributeName = "tile_R")]
        public string Tile_R { get; set; }

        [XmlAttribute(AttributeName = "tile_C")]
        public string Tile_C { get; set; }

    }

    [XmlRoot(ElementName = "Data_Files")]
    public class Data_Files {

        [XmlElement(ElementName = "Data_File")]
        public List<Data_File> Data_File { get; set; }

    }

    [XmlRoot(ElementName = "Data_Access")]
    public class Data_Access {

        [XmlElement(ElementName = "DATA_FILE_ORGANISATION")]
        public string DATA_FILE_ORGANISATION { get; set; }

        [XmlElement(ElementName = "DATA_FILE_FORMAT")]
        public string DATA_FILE_FORMAT { get; set; }

        [XmlElement(ElementName = "DATA_FILE_TILES")]
        public string DATA_FILE_TILES { get; set; }

        [XmlElement(ElementName = "Data_Files")]
        public Data_Files Data_Files { get; set; }

    }

    [XmlRoot(ElementName = "NTILES_SIZE")]
    public class NTILES_SIZE {

        [XmlAttribute(AttributeName = "nrows")]
        public string Nrows { get; set; }

        [XmlAttribute(AttributeName = "ncols")]
        public string Ncols { get; set; }

    }

    [XmlRoot(ElementName = "NTILES_COUNT")]
    public class NTILES_COUNT {

        [XmlAttribute(AttributeName = "ntiles_R")]
        public string Ntiles_R { get; set; }

        [XmlAttribute(AttributeName = "ntiles_C")]
        public string Ntiles_C { get; set; }

    }

    [XmlRoot(ElementName = "Regular_Tiling")]
    public class Regular_Tiling {

        [XmlElement(ElementName = "NTILES_SIZE")]
        public NTILES_SIZE NTILES_SIZE { get; set; }

        [XmlElement(ElementName = "NTILES_COUNT")]
        public NTILES_COUNT NTILES_COUNT { get; set; }

        [XmlElement(ElementName = "OVERLAP_ROW")]
        public string OVERLAP_ROW { get; set; }

        [XmlElement(ElementName = "OVERLAP_COL")]
        public string OVERLAP_COL { get; set; }

    }

    //just found in PNEO
    [XmlRoot(ElementName = "Tile_Set")]
    public class Tile_Set {
        
        [XmlElement(ElementName = "NTILES")]
        public string NTILES { get; set; }

        [XmlElement(ElementName = "Regular_Tiling")]
        public Regular_Tiling Regular_Tiling { get; set; }

    }

    [XmlRoot(ElementName = "Raster_Dimensions")]
    public class Raster_Dimensions {

        [XmlElement(ElementName = "NROWS")]
        public string NROWS { get; set; }

        [XmlElement(ElementName = "NCOLS")]
        public string NCOLS { get; set; }

        [XmlElement(ElementName = "NBANDS")]
        public string NBANDS { get; set; }

        [XmlElement(ElementName = "Tile_Set")]
        public Tile_Set Tile_Set { get; set; }

    }

    [XmlRoot(ElementName = "Raster_Encoding")]
    public class Raster_Encoding {

        [XmlElement(ElementName = "DATA_TYPE")]
        public string DATA_TYPE { get; set; }

        [XmlElement(ElementName = "NBITS")]
        public string NBITS { get; set; }

        [XmlElement(ElementName = "SIGN")]
        public string SIGN { get; set; }

        [XmlElement(ElementName = "COMPRESSION_TYPE")]
        public string COMPRESSION_TYPE { get; set; }

        [XmlElement(ElementName = "COMPRESSION_RATIO")]
        public string COMPRESSION_RATIO { get; set; }

    }

    [XmlRoot(ElementName = "Band_Display_Order")]
    public class Band_Display_Order {

        [XmlElement(ElementName = "RED_CHANNEL")]
        public string RED_CHANNEL { get; set; }

        [XmlElement(ElementName = "GREEN_CHANNEL")]
        public string GREEN_CHANNEL { get; set; }

        [XmlElement(ElementName = "BLUE_CHANNEL")]
        public string BLUE_CHANNEL { get; set; }

        [XmlElement(ElementName = "ALPHA_CHANNEL")]
        public string ALPHA_CHANNEL { get; set; }

    }

    [XmlRoot(ElementName = "Special_Value")]
    public class Special_Value {

        [XmlElement(ElementName = "SPECIAL_VALUE_TEXT")]
        public string SPECIAL_VALUE_TEXT { get; set; }

        [XmlElement(ElementName = "SPECIAL_VALUE_COUNT")]
        public string SPECIAL_VALUE_COUNT { get; set; }

    }

    [XmlRoot(ElementName = "Raster_Display")]
    public class Raster_Display {

        [XmlElement(ElementName = "Band_Display_Order")]
        public Band_Display_Order Band_Display_Order { get; set; }

        [XmlElement(ElementName = "Special_Value")]
        public List<Special_Value> Special_Value { get; set; }

    }

    [XmlRoot(ElementName = "Raster_Data")]
    public class Raster_Data {

        [XmlElement(ElementName = "Data_Access")]
        public Data_Access Data_Access { get; set; }

        [XmlElement(ElementName = "Raster_Dimensions")]
        public Raster_Dimensions Raster_Dimensions { get; set; }

        [XmlElement(ElementName = "Raster_Encoding")]
        public Raster_Encoding Raster_Encoding { get; set; }

        [XmlElement(ElementName = "Raster_Display")]
        public Raster_Display Raster_Display { get; set; }

    }

    [XmlRoot(ElementName = "Dynamic_Range")]
    public class Dynamic_Range {

        [XmlElement(ElementName = "ACQUISITION_RANGE")]
        public string ACQUISITION_RANGE { get; set; }

        [XmlElement(ElementName = "PRODUCT_RANGE")]
        public string PRODUCT_RANGE { get; set; }

    }

    [XmlRoot(ElementName = "Dynamic_Adjustment")]
    public class Dynamic_Adjustment {

        [XmlElement(ElementName = "ADJUSTMENT_TYPE")]
        public string ADJUSTMENT_TYPE { get; set; }

    }

    [XmlRoot(ElementName = "Histogram_Band")]
    public class Histogram_Band {

        [XmlElement(ElementName = "BAND_ID")]
        public string BAND_ID { get; set; }

        [XmlElement(ElementName = "VALUES")]
        public string VALUES { get; set; }

        [XmlElement(ElementName = "STEP")]
        public string STEP { get; set; }

        [XmlElement(ElementName = "MIN")]
        public string MIN { get; set; }

        [XmlElement(ElementName = "MAX")]
        public string MAX { get; set; }

        [XmlElement(ElementName = "MEAN")]
        public string MEAN { get; set; }

        [XmlElement(ElementName = "STDV")]
        public string STDV { get; set; }

    }

    [XmlRoot(ElementName = "Histogram_Band_List")]
    public class Histogram_Band_List {

        [XmlElement(ElementName = "Histogram_Band")]
        public List<Histogram_Band> Histogram_Band { get; set; }

    }

    [XmlRoot(ElementName = "Straylight")]
    public class Straylight {

        [XmlElement(ElementName = "PAN_RAW_STRAYLIGHT_MIN_SIZE")]
        public string PAN_RAW_STRAYLIGHT_MIN_SIZE { get; set; }

        [XmlElement(ElementName = "XS_RAW_STRAYLIGHT_MIN_SIZE")]
        public string XS_RAW_STRAYLIGHT_MIN_SIZE { get; set; }

    }

    [XmlRoot(ElementName = "Band_Spectral_Range")]
    public class Band_Spectral_Range {

        [XmlElement(ElementName = "BAND_ID")]
        public string BAND_ID { get; set; }

        [XmlElement(ElementName = "CALIBRATION_DATE")]
        public string CALIBRATION_DATE { get; set; }

        [XmlElement(ElementName = "MEASURE_DESC")]
        public string MEASURE_DESC { get; set; }

        [XmlElement(ElementName = "MEASURE_UNIT")]
        public string MEASURE_UNIT { get; set; }

        [XmlElement(ElementName = "MEASURE_UNCERTAINTY")]
        public string MEASURE_UNCERTAINTY { get; set; }

        [XmlElement(ElementName = "MIN")]
        public string MIN { get; set; }

        [XmlElement(ElementName = "MAX")]
        public string MAX { get; set; }

    }

    [XmlRoot(ElementName = "Band_Radiance")]
    public class Band_Radiance {

        [XmlElement(ElementName = "BAND_ID")]
        public string BAND_ID { get; set; }

        [XmlElement(ElementName = "CALIBRATION_DATE")]
        public string CALIBRATION_DATE { get; set; }

        [XmlElement(ElementName = "MEASURE_DESC")]
        public string MEASURE_DESC { get; set; }

        [XmlElement(ElementName = "MEASURE_UNIT")]
        public string MEASURE_UNIT { get; set; }

        [XmlElement(ElementName = "MEASURE_UNCERTAINTY")]
        public string MEASURE_UNCERTAINTY { get; set; }

        [XmlElement(ElementName = "GAIN")]
        public string GAIN { get; set; }

        [XmlElement(ElementName = "BIAS")]
        public string BIAS { get; set; }

    }

    [XmlRoot(ElementName = "Band_Solar_Irradiance")]
    public class Band_Solar_Irradiance {

        [XmlElement(ElementName = "BAND_ID")]
        public string BAND_ID { get; set; }

        [XmlElement(ElementName = "CALIBRATION_DATE")]
        public string CALIBRATION_DATE { get; set; }

        [XmlElement(ElementName = "MEASURE_DESC")]
        public string MEASURE_DESC { get; set; }

        [XmlElement(ElementName = "MEASURE_UNIT")]
        public string MEASURE_UNIT { get; set; }

        [XmlElement(ElementName = "MEASURE_UNCERTAINTY")]
        public string MEASURE_UNCERTAINTY { get; set; }

        [XmlElement(ElementName = "VALUE")]
        public string VALUE { get; set; }

    }

    [XmlRoot(ElementName = "Band_Measurement_List")]
    public class Band_Measurement_List {

        [XmlElement(ElementName = "Band_Spectral_Range")]
        public List<Band_Spectral_Range> Band_Spectral_Range { get; set; }

        [XmlElement(ElementName = "Band_Radiance")]
        public List<Band_Radiance> Band_Radiance { get; set; }

        [XmlElement(ElementName = "Band_Solar_Irradiance")]
        public List<Band_Solar_Irradiance> Band_Solar_Irradiance { get; set; }

    }

    [XmlRoot(ElementName = "Instrument_Calibration")]
    public class Instrument_Calibration {

        [XmlElement(ElementName = "Band_Measurement_List")]
        public Band_Measurement_List Band_Measurement_List { get; set; }

    }

    [XmlRoot(ElementName = "Radiometric_Calibration")]
    public class Radiometric_Calibration {

        [XmlElement(ElementName = "Instrument_Calibration")]
        public Instrument_Calibration Instrument_Calibration { get; set; }

    }

    [XmlRoot(ElementName = "Radiometric_Data")]
    public class Radiometric_Data {

        [XmlElement(ElementName = "Dynamic_Range")]
        public Dynamic_Range Dynamic_Range { get; set; }

        [XmlElement(ElementName = "Dynamic_Adjustment")]
        public Dynamic_Adjustment Dynamic_Adjustment { get; set; }

        [XmlElement(ElementName = "Histogram_Band_List")]
        public Histogram_Band_List Histogram_Band_List { get; set; }

        [XmlElement(ElementName = "Straylight")]
        public Straylight Straylight { get; set; }

        [XmlElement(ElementName = "Radiometric_Calibration")]
        public Radiometric_Calibration Radiometric_Calibration { get; set; }

    }

    [XmlRoot(ElementName = "Acquisition_Angles")]
    public class Acquisition_Angles {

        [XmlElement(ElementName = "AZIMUTH_ANGLE")]
        public string AZIMUTH_ANGLE { get; set; }

        [XmlElement(ElementName = "VIEWING_ANGLE_ACROSS_TRACK")]
        public string VIEWING_ANGLE_ACROSS_TRACK { get; set; }

        [XmlElement(ElementName = "VIEWING_ANGLE_ALONG_TRACK")]
        public string VIEWING_ANGLE_ALONG_TRACK { get; set; }

        [XmlElement(ElementName = "VIEWING_ANGLE")]
        public string VIEWING_ANGLE { get; set; }

        [XmlElement(ElementName = "INCIDENCE_ANGLE_ALONG_TRACK")]
        public string INCIDENCE_ANGLE_ALONG_TRACK { get; set; }

        [XmlElement(ElementName = "INCIDENCE_ANGLE_ACROSS_TRACK")]
        public string INCIDENCE_ANGLE_ACROSS_TRACK { get; set; }

        [XmlElement(ElementName = "INCIDENCE_ANGLE")]
        public string INCIDENCE_ANGLE { get; set; }

    }

    [XmlRoot(ElementName = "Solar_Incidences")]
    public class Solar_Incidences {

        [XmlElement(ElementName = "SUN_AZIMUTH")]
        public string SUN_AZIMUTH { get; set; }

        [XmlElement(ElementName = "SUN_ELEVATION")]
        public string SUN_ELEVATION { get; set; }

    }

    [XmlRoot(ElementName = "Ground_Sample_Distance")]
    public class Ground_Sample_Distance {

        [XmlElement(ElementName = "GSD_ACROSS_TRACK")]
        public string GSD_ACROSS_TRACK { get; set; }

        [XmlElement(ElementName = "GSD_ALONG_TRACK")]
        public string GSD_ALONG_TRACK { get; set; }

    }

    [XmlRoot(ElementName = "Located_Geometric_Values")]
    public class Located_Geometric_Values {

        [XmlElement(ElementName = "LOCATION_TYPE")]
        public string LOCATION_TYPE { get; set; }

        [XmlElement(ElementName = "COL")]
        public string COL { get; set; }

        [XmlElement(ElementName = "ROW")]
        public string ROW { get; set; }

        [XmlElement(ElementName = "TIME")]
        public string TIME { get; set; }

        [XmlElement(ElementName = "SATELLITE_ALTITUDE")]
        public string SATELLITE_ALTITUDE { get; set; }

        [XmlElement(ElementName = "Acquisition_Angles")]
        public Acquisition_Angles Acquisition_Angles { get; set; }

        [XmlElement(ElementName = "Solar_Incidences")]
        public Solar_Incidences Solar_Incidences { get; set; }

        [XmlElement(ElementName = "Ground_Sample_Distance")]
        public Ground_Sample_Distance Ground_Sample_Distance { get; set; }

    }

    [XmlRoot(ElementName = "Use_Area")]
    public class Use_Area {

        [XmlElement(ElementName = "Located_Geometric_Values")]
        public List<Located_Geometric_Values> Located_Geometric_Values { get; set; }

    }

    [XmlRoot(ElementName = "Geometric_Data")]
    public class Geometric_Data {

        [XmlElement(ElementName = "Use_Area")]
        public Use_Area Use_Area { get; set; }

    }

    [XmlRoot(ElementName = "MEASURE_UNIT")]
    public class MEASURE_UNIT {

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

    }

    [XmlRoot(ElementName = "ACCURACY_MEAN")]
    public class ACCURACY_MEAN {

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "ACCURACY_STDV")]
    public class ACCURACY_STDV {

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "ACCURACY_CE68")]
    public class ACCURACY_CE68 {

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "ACCURACY_CE90")]
    public class ACCURACY_CE90 {

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlText]
        public string Text { get; set; }

    }

    [XmlRoot(ElementName = "Quality_Values")]
    public class Quality_Values {

        [XmlElement(ElementName = "ACCURACY_MEAN")]
        public ACCURACY_MEAN ACCURACY_MEAN { get; set; }

        [XmlElement(ElementName = "ACCURACY_STDV")]
        public ACCURACY_STDV ACCURACY_STDV { get; set; }

        [XmlElement(ElementName = "ACCURACY_CE68")]
        public ACCURACY_CE68 ACCURACY_CE68 { get; set; }

        [XmlElement(ElementName = "ACCURACY_CE90")]
        public ACCURACY_CE90 ACCURACY_CE90 { get; set; }

    }

    [XmlRoot(ElementName = "Planimetric_Accuracy_Measurement")]
    public class Planimetric_Accuracy_Measurement {

        [XmlElement(ElementName = "QUALITY_TABLES")]
        public string QUALITY_TABLES { get; set; }

        [XmlElement(ElementName = "MEASURE_NAME")]
        public string MEASURE_NAME { get; set; }

        [XmlElement(ElementName = "MEASURE_ID")]
        public string MEASURE_ID { get; set; }

        [XmlElement(ElementName = "MEASURE_DESC")]
        public string MEASURE_DESC { get; set; }

        [XmlElement(ElementName = "MEASURE_TYPE")]
        public string MEASURE_TYPE { get; set; }

        [XmlElement(ElementName = "MEASURE_UNIT")]
        public MEASURE_UNIT MEASURE_UNIT { get; set; }

        [XmlElement(ElementName = "Quality_Values")]
        public Quality_Values Quality_Values { get; set; }

    }

    [XmlRoot(ElementName = "Quality_Mask")]
    public class Quality_Mask {

        [XmlElement(ElementName = "Component")]
        public Component Component { get; set; }

    }

    [XmlRoot(ElementName = "Imaging_Quality_Measurement")]
    public class Imaging_Quality_Measurement {

        [XmlElement(ElementName = "QUALITY_TABLES")]
        public string QUALITY_TABLES { get; set; }

        [XmlElement(ElementName = "MEASURE_NAME")]
        public string MEASURE_NAME { get; set; }

        [XmlElement(ElementName = "MEASURE_DESC")]
        public string MEASURE_DESC { get; set; }

        [XmlElement(ElementName = "MEASURE_TYPE")]
        public string MEASURE_TYPE { get; set; }

        [XmlElement(ElementName = "Quality_Mask")]
        public Quality_Mask Quality_Mask { get; set; }

    }

    [XmlRoot(ElementName = "Quality_Assessment")]
    public class Quality_Assessment {

        [XmlElement(ElementName = "Planimetric_Accuracy_Measurement")]
        public Planimetric_Accuracy_Measurement Planimetric_Accuracy_Measurement { get; set; }

        [XmlElement(ElementName = "Imaging_Quality_Measurement")]
        public List<Imaging_Quality_Measurement> Imaging_Quality_Measurement { get; set; }

    }

    [XmlRoot(ElementName = "Strip_Source")]
    public class Strip_Source {

        [XmlElement(ElementName = "MISSION")]
        public string MISSION { get; set; }

        [XmlElement(ElementName = "MISSION_INDEX")]
        public string MISSION_INDEX { get; set; }

        [XmlElement(ElementName = "IMAGING_DATE")]
        public string IMAGING_DATE { get; set; }

        [XmlElement(ElementName = "IMAGING_TIME")]
        public string IMAGING_TIME { get; set; }

        [XmlElement(ElementName = "BAND_MODE")]
        public string BAND_MODE { get; set; }

    }

    [XmlRoot(ElementName = "Source_Identification")]
    public class Source_Identification {

        [XmlElement(ElementName = "SOURCE_ID")]
        public string SOURCE_ID { get; set; }

        [XmlElement(ElementName = "SOURCE_TYPE")]
        public string SOURCE_TYPE { get; set; }

        [XmlElement(ElementName = "SOURCE_DESCRIPTION")]
        public string SOURCE_DESCRIPTION { get; set; }

        [XmlElement(ElementName = "Strip_Source")]
        public Strip_Source Strip_Source { get; set; }

        [XmlElement(ElementName = "Component")]
        public Component Component { get; set; }

    }

    [XmlRoot(ElementName = "Dataset_Sources")]
    public class Dataset_Sources {

        [XmlElement(ElementName = "Source_Identification")]
        public List<Source_Identification> Source_Identification { get; set; }

    }

    [XmlRoot(ElementName = "Dimap_Document")]
    public class Dimap_Document {

        [XmlElement(ElementName = "Metadata_Identification")]
        public Metadata_Identification Metadata_Identification { get; set; }

        [XmlElement(ElementName = "Dataset_Identification")]
        public Dataset_Identification Dataset_Identification { get; set; }

        [XmlElement(ElementName = "Dataset_Content")]
        public Dataset_Content Dataset_Content { get; set; }

        [XmlElement(ElementName = "Product_Information")]
        public Product_Information Product_Information { get; set; }

        [XmlElement(ElementName = "Coordinate_Reference_System")]
        public Coordinate_Reference_System Coordinate_Reference_System { get; set; }

        [XmlElement(ElementName = "Geoposition")]
        public Geoposition Geoposition { get; set; }

        [XmlElement(ElementName = "Processing_Information")]
        public Processing_Information Processing_Information { get; set; }

        [XmlElement(ElementName = "Raster_Data")]
        public Raster_Data Raster_Data { get; set; }

        [XmlElement(ElementName = "Radiometric_Data")]
        public Radiometric_Data Radiometric_Data { get; set; }

        [XmlElement(ElementName = "Geometric_Data")]
        public Geometric_Data Geometric_Data { get; set; }

        [XmlElement(ElementName = "Quality_Assessment")]
        public Quality_Assessment Quality_Assessment { get; set; }

        [XmlElement(ElementName = "Dataset_Sources")]
        public Dataset_Sources Dataset_Sources { get; set; }

        [XmlAttribute(AttributeName = "xlink", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xlink { get; set; }

        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string NoNamespaceSchemaLocation { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

    }
}
