// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR
{
    public class PdsFHIRConfigurations
    {
        public string KeyId { get; set; }
        public string AuthorisationUrl { get; set; }
        public string JwtSigningKey { get; set; }
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string PatientLookupPath { get; set; }
        public string PatientSearchPath { get; set; }
        public string RequestId { get; set; }
    }
}
