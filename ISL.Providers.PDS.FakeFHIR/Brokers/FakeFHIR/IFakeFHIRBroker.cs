// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Brokers.FakeFHIR
{
    internal interface IFakeFHIRBroker
    {
        ValueTask<Bundle> GetNhsNumberAsync(string searchParams);
        ValueTask<Patient> GetPdsPatientDetailsAsync(string nhsNumber);
    }
}
