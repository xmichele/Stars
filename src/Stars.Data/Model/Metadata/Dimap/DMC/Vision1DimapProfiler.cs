using Stac;
using Stac.Extensions.Eo;
using Terradue.Stars.Data.Model.Metadata.Dimap.Schemas;
using Terradue.Stars.Interface;

namespace Terradue.Stars.Data.Model.Metadata.Dimap.DMC
{
    internal class Vision1DimapProfiler : DmcDimapProfiler
    {

        public Vision1DimapProfiler(t_Dimap_Document dimap) : base(dimap)
        {
        }

        internal override string GetProcessingLevel()
        {
            return Dimap.Production.PRODUCT_TYPE;
        }

        protected override EoBandObject GetEoBandObject(Schemas.t_Spectral_Band_Info bandInfo, string description)
        {
            var eoBandObject = base.GetEoBandObject(bandInfo, description);
            ////
            if (bandInfo.BAND_DESCRIPTION.ToUpper() == "PAN")
            {
                eoBandObject.SolarIllumination = 1830.15;
            }
            if (bandInfo.BAND_DESCRIPTION.ToUpper() == "BLUE")
            {
                eoBandObject.SolarIllumination = 2002.25;
            }
            if (bandInfo.BAND_DESCRIPTION.ToUpper() == "NIR")
            {
                eoBandObject.SolarIllumination = 948.98;
            }
            if (bandInfo.BAND_DESCRIPTION.ToUpper() == "RED")
            {
                eoBandObject.SolarIllumination = 1613.83;
            }
            if (bandInfo.BAND_DESCRIPTION.ToUpper() == "GREEN")
            {
                eoBandObject.SolarIllumination = 1822.22;
            }
            ////

            return eoBandObject;
        }

        internal override string GetMission()
        {
            return "Vision-1";
        }

        internal override string GetOrbitState()
        {
            return "ascending";
        }

        public override string GetProductKey(IAsset bandAsset, t_Data_File dataFile)
        {
            return "composite";
        }

        /// <summary>
        /// https://directory.eoportal.org/web/eoportal/satellite-missions/u/uk-dmc-2
        /// </summary>
        /// <returns></returns>
        internal override string GetConstellation()
        {
            return "dmc";
        }
    }
}