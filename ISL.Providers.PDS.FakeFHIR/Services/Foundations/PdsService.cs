// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal class PdsService : IPdsService
    {
        public ValueTask<PatientBundle> PatientLookupByDetailsAsync(string givenName = null,
            string familyName = null,
            string gender = null,
            string postCode = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber)
        {
            throw new NotImplementedException();
        }
    }
}
