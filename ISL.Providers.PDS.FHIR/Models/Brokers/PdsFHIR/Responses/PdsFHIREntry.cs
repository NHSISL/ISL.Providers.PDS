// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Newtonsoft.Json;

namespace ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR.Responses
{
    public class PdsFHIREntry
    {
        [JsonProperty("resource")]
        public PdsFHIRResource Resource { get; set; }
    }
}
