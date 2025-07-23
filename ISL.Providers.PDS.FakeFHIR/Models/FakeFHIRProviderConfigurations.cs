// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;

namespace ISL.Providers.PDS.FakeFHIR.Models
{
    public class FakeFHIRProviderConfigurations
    {
        public List<FakeFHIRProviderPatientDetails> FakePatients { get; set; } = new List<FakeFHIRProviderPatientDetails>();
    }
}
