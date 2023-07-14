using System;
using System.Collections.Generic;
using Stac;
using Stac.Extensions.Eo;
using Terradue.Stars.Data.Model.Metadata.Airbus.Schemas;

namespace Terradue.Stars.Data.Model.Metadata.Airbus
{
    internal class PleiadesNEODimapProfiler : AirbusProfiler
    {
        public PleiadesNEODimapProfiler(Dimap_Document dimap) : base(dimap)
        {
        }

        internal override string GetConstellation()
        {
            return "pleiades";
        }

        internal override string GetMission()
        {
            return base.GetMission().Replace("PNEO", "PleiadesNEO");
        }

        protected override IDictionary<EoBandCommonName?, int> BandOrders
        {
            get
            {
                Dictionary<EoBandCommonName?, int> bandOrders = new Dictionary<EoBandCommonName?, int>();
                bandOrders.Add(EoBandCommonName.pan, 0);
                bandOrders.Add(EoBandCommonName.red, 1);
                bandOrders.Add(EoBandCommonName.green, 2);
                bandOrders.Add(EoBandCommonName.blue, 3);
                bandOrders.Add(EoBandCommonName.nir, 4);
                return bandOrders;
            }
        }

        public override string GetPlatformInternationalDesignator()
        {
            string mission = GetMission().ToLower();
            switch (mission)
            {
                //TODO check changes, e.g. by gdoc
                case "pleiades-1a":
                    return "2011-076F";
                case "pleiades-1b":
                    return "2012-068A";
            }
            return null;
        }

        internal override StacProvider GetStacProvider()
        {
            StacProvider provider = new StacProvider("Airbus", new StacProviderRole[] { StacProviderRole.producer, StacProviderRole.processor, StacProviderRole.licensor });
            provider.Description = "Pléiades Neo is our most advanced optical constellation, with two identical 30cm resolution satellites with ultimate reactivity. Entirely funded, manufactured, owned and operated by Airbus, Pléiades Neo is a breakthrough in the Earth Observation domain.";
            provider.Uri = new Uri("https://www.intelligence-airbusds.com/imagery/constellation/pleiades-neo/");
            return provider;
        }
    }
}