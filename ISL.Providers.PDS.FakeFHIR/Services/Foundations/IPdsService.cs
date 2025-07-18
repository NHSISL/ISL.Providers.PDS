// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using System.Threading.Tasks;
using System;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal interface IPdsService
    {
        ValueTask<PdsResponse> PatientLookupByDetailsAsync(
            string surname, 
            string postcode, 
            DateTimeOffset dateOfBirth);

        ValueTask<PdsResponse> PatientLookupByNhsNumberAsync(string nhsNumber);
    }
}
