// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker;
using ISL.Providers.PDS.FHIR.Mappers;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FHIR.Services.Foundations.Pds
{
    internal class PdsService : IPdsService
    {
        private readonly IPdsFHIRBroker pdsFHIRBroker;

        public PdsService(IPdsFHIRBroker pdsFHIRBroker, PdsFHIRConfigurations pdsFHIRConfigurations)
        {
            this.pdsFHIRBroker = pdsFHIRBroker;
        }

        public async ValueTask<PatientBundle> PatientLookupByDetailsAsync(
            string givenName = null,
            string familyName = null,
            string gender = null,
            string postcode = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null)
        {
            Bundle bundle = await pdsFHIRBroker.GetNhsNumberAsync(
                givenName,
                familyName,
                gender,
                postcode,
                dateOfBirth,
                dateOfDeath,
                registeredGpPractice,
                email,
                phoneNumber);

            PatientBundle patientBundle = PatientBundleMapper.FromBundle(bundle);

            return patientBundle;
        }

        public async ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber)
        {
            Patient patient = await pdsFHIRBroker.GetPdsPatientDetailsAsync(nhsNumber);

            return patient;
        }
    }
}
