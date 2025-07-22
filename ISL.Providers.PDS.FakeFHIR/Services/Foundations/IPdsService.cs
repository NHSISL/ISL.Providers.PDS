// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal interface IPdsService
    {
        ValueTask<PatientBundle> PatientLookupByDetailsAsync(string searchParams);

        ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber);
    }
}
