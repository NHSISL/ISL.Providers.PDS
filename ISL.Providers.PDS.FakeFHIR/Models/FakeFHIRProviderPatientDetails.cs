// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace ISL.Providers.PDS.FakeFHIR.Models
{
    public class FakeFHIRProviderPatientDetails
    {
        public string Title { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string NhsNumber { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string DateOfDeath { get; set; } = string.Empty;
        public string RegisteredGpPractice { get; set; } = string.Empty;
    }
}
