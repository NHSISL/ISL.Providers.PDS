// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR
{
    public class PdsPatientDetails
    {
        public string Prefix { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Postcode { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
