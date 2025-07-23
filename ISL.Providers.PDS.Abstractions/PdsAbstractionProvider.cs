// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
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
        public ValueTask<PatientBundle> PatientLookupByDetailsAsync(
            string givenName = null,
            string familyName = null,
            string gender = null,
            string postcode = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null) =>
        TryCatch(async () =>
        {
            return await this.pdsProvider.PatientLookupByDetailsAsync(
                givenName: givenName,
                familyName: familyName,
                gender: gender,
                postcode: postcode,
                dateOfBirth: dateOfBirth,
                dateOfDeath: dateOfDeath,
                registeredGpPractice: registeredGpPractice,
                email: email,
                phoneNumber: phoneNumber);
        });

        /// <summary>
        /// Uses PDS FHIR API to obtain the patient details
        /// for a patient given their NHS Number 
        /// </summary>
        /// <returns>
        /// A Patient object containing information on the corresponding patient 
        /// </returns>
        /// <exception cref="PdsValidationProviderException" />
        /// <exception cref="PdsDependencyProviderException" />
        /// <exception cref="PdsServiceProviderException" />
        public ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber) =>
        TryCatch(async () =>
        {
            return await this.pdsProvider.PatientLookupByNhsNumberAsync(nhsNumber);
        });
    }
}
