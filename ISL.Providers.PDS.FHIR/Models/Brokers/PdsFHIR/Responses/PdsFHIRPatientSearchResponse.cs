// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Newtonsoft.Json;
using System.Collections.Generic;

namespace ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR.Responses
{
    public class PdsFHIRPatientSearchResponse
    {
        [JsonProperty("entry")]
        public List<PdsFHIREntry> Entries { get; set; }
    }
}
