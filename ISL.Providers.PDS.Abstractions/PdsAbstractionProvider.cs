// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.Abstractions
{
    public partial class PdsAbstractionProvider : IPdsAbstractionProvider
    {
        private readonly IPdsProvider pdsProvider;

        public PdsAbstractionProvider(IPdsProvider pdsProvider) =>
            this.pdsProvider = pdsProvider;

        /// <summary>
        /// Uses PDS FHIR API to obtain the NHS Number for a patient given their Surname, DOB and postcode
        /// </summary>
        /// <returns>
        /// A PDS response where the NHS Number has been replaced by the real NHS Number and additional details populated.
        /// If the PDS search could not happen due to search parameters being invalid, the Nhs Number will be
        /// replaced by 0000000000.
        /// </returns>
        /// <exception cref="PdsValidationProviderException" />
        /// <exception cref="PdsDependencyProviderException" />
        /// <exception cref="PdsServiceProviderException" />
        public ValueTask<PdsResponse> PatientLookupByDetailsAsync(string surname, string postcode, DateTimeOffset dateOfBirth) =>
        TryCatch(async () =>
        {
            return await this.pdsProvider.PatientLookupByDetailsAsync(surname, postcode, dateOfBirth);
        });

        /// <summary>
        /// Uses PDS FHIR API to obtain the name, DOB, Postcode and contact information
        /// for a patient given their NHS Number 
        /// </summary>
        /// <returns>
        /// A PDS response where the name, DOB, Postcode and contact information are populated.
        /// If the PDS search could not happen due to search parameters being invalid, the Nhs Number will be
        /// DOB, Postcode and contact information will be empty.
        /// </returns>
        /// <exception cref="PdsValidationProviderException" />
        /// <exception cref="PdsDependencyProviderException" />
        /// <exception cref="PdsServiceProviderException" />
        public ValueTask<PdsResponse> PatientLookupByNhsNumberAsync(string nhsNumber) =>
        TryCatch(async () =>
        {
            return await this.pdsProvider.PatientLookupByNhsNumberAsync(nhsNumber);
        });
    }
}
