// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ISL.Providers.PDS.FakeFHIR.Models
{
    internal class PdsPatientDetails
    {
        public string Title { get; set; } = string.Empty;
        public List<string> GivenNames { get; set; } = new List<string>();
        public string Surname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string NhsNumber { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset DateOfDeath { get; set; }
        public string RegisteredGpPractice { get; set; } = string.Empty;
        public bool IsSensitive { get; set; }
    }
}
