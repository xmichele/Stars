using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Terradue.Stars.Services.ThirdParty.Egms
{
    [DataContract]
    public class EgmsTimeSeriesImportTask
    {
        private string nsId;

        [DataMember(Name = "job_id")]
        public string JobId { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "namespace_id")]
        public string NamespaceId { get => nsId; set => nsId = value; }

        [DataMember(Name = "timeseries_id")]
        public string TimeSeriesId { get; set; }

        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
    }
}