// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.Abstractions
{
    public interface IPdsOperations
    {
        /// <summary>
        /// Uses PDS FHIR API to obtain the NHS Number for a patient given their Surname, DOB and postcode
        /// </summary>
        /// <returns>
        /// A PDS response where the NHS Number has been replaced by the real NHS Number and additional details populated.
        /// If the PDS search could not happen due to search parameters being invalid, the Nhs Number will be
        /// replaced by 0000000000.
        /// </returns>
        ValueTask<PdsResponse> PatientLookupByDetailsAsync(string surname, string postcode, DateTimeOffset dateOfBirth);

        /// <summary>
        /// Uses PDS FHIR API to obtain the name, DOB, Postcode and contact information
        /// for a patient given their NHS Number 
        /// </summary>
        /// <returns>
        /// A PDS response where the name, DOB, Postcode and contact information are populated.
        /// If the PDS search could not happen due to search parameters being invalid, the Nhs Number will be
        /// DOB, Postcode and contact information will be empty.
        /// </returns>
        ValueTask<PdsResponse> PatientLookupByNhsNumberAsync(string nhsNumber);
    }
}
