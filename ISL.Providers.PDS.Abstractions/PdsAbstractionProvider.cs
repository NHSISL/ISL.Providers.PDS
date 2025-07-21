// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.Abstractions
{
    public partial class PdsAbstractionProvider : IPdsAbstractionProvider
    {
        private readonly IPdsProvider pdsProvider;

        public PdsAbstractionProvider(IPdsProvider pdsProvider) =>
            this.pdsProvider = pdsProvider;

        /// <summary>
        /// Uses PDS FHIR API to obtain the NHS Number for a patient given provided search parameters
        /// </summary>
        /// <returns>
        /// A PatientBundle object containing a list of matched patients
        /// </returns>
        /// <exception cref="PdsValidationProviderException" />
        /// <exception cref="PdsDependencyProviderException" />
        /// <exception cref="PdsServiceProviderException" />
        public ValueTask<PatientBundle> PatientLookupByDetailsAsync(string searchParams) =>
        TryCatch(async () =>
        {
            return await this.pdsProvider.PatientLookupByDetailsAsync(searchParams);
        });

        /// <summary>
        /// Uses PDS FHIR API to obtain the patient details
        /// for a patient given their NHS Number 
        /// </summary>
        /// <returns>
        /// A PatientBundle object containing a list with one item of corresponding patient 
        /// </returns>
        /// <exception cref="PdsValidationProviderException" />
        /// <exception cref="PdsDependencyProviderException" />
        /// <exception cref="PdsServiceProviderException" />
        public ValueTask<PatientBundle> PatientLookupByNhsNumberAsync(string nhsNumber) =>
        TryCatch(async () =>
        {
            return await this.pdsProvider.PatientLookupByNhsNumberAsync(nhsNumber);
        });
    }
}
