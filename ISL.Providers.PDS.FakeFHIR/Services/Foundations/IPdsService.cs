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
        ValueTask<PatientBundle> PatientLookupByDetailsAsync(string givenName = null,
            string familyName = null,
            string gender = null,
            string postcode = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null);

        ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber);
    }
}
