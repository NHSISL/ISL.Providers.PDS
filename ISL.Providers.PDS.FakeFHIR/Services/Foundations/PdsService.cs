// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal class PdsService : IPdsService
    {
        public ValueTask<PdsResponse> PatientLookupByDetailsAsync(
            string surname, 
            string postcode, 
            DateTimeOffset dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ValueTask<PdsResponse> PatientLookupByNhsNumberAsync(string nhsNumber)
        {
            throw new NotImplementedException();
        }
    }
}
