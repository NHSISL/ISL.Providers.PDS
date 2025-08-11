// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;

namespace ISL.Providers.PDS.FakeFHIR.Models
{
    public class FakeFHIRProviderPatientDetails
    {
        public string Title { get; set; } = string.Empty;
        public List<string> GivenNames { get; set; } = new List<string>();
        public string Surname { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string NhsNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DateOfDeath { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RegisteredGpPractice { get; set; } = string.Empty;
    }
}
