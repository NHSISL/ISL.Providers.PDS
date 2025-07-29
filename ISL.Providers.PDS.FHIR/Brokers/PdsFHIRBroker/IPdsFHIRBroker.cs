// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker
{
    internal interface IPdsFHIRBroker
    {
        ValueTask<Patient> GetNhsNumberAsync(string path);
        ValueTask<Bundle> GetPdsPatientDetailsAsync(string path);
    }
}
