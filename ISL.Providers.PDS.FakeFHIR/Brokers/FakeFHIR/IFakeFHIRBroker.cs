// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.FakeFHIR.Models;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Brokers.FakeFHIR
{
    internal interface IFakeFHIRBroker
    {
        ValueTask<string> GetNhsNumberAsync(string surname, string postcode, DateTimeOffset dateOfBirth);
        ValueTask<PdsPatientDetails> GetPdsPatientDetailsAsync(string nhsNumber);
    }
}
