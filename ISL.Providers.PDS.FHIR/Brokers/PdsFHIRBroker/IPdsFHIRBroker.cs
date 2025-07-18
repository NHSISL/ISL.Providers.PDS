// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker
{
    internal interface IPdsFHIRBroker
    {
        ValueTask<string> GetNhsNumberAsync(string surname, string postcode, DateTimeOffset dateOfBirth);
        ValueTask<PdsPatientDetails> GetPdsPatientDetailsAsync(string nhsNumber);
    }
}
