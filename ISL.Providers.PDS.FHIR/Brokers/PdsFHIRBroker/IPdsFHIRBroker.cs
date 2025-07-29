// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker
{
    internal interface IPdsFHIRBroker
    {
        ValueTask<Bundle> GetNhsNumberAsync(string path);

        ValueTask<Patient> GetPdsPatientDetailsAsync(string path);
    }
}
