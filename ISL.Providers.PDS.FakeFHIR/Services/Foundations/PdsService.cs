// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Brokers.FakeFHIR;
using ISL.Providers.PDS.FakeFHIR.Mappers;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal partial class PdsService : IPdsService
    {
        private readonly IFakeFHIRBroker fakeFHIRBroker;

        public PdsService(IFakeFHIRBroker fakeFHIRBroker)
        {
            this.fakeFHIRBroker = fakeFHIRBroker;
        }

        public ValueTask<PatientBundle> PatientLookupByDetailsAsync(string searchParams) =>
            TryCatch(async () =>
            {
                ValidatePatientLookupByDetailsArguments(searchParams);

                Bundle bundle = await fakeFHIRBroker.GetNhsNumberAsync(searchParams);
                PatientBundle patientBundle = PatientBundleMapper.FromBundle(bundle);

                return patientBundle;
            });

        public ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber) =>
            TryCatch(async () =>
            {
                ValidatePatientLookupByNhsNumberArguments(nhsNumber);

                Patient patient = await fakeFHIRBroker.GetPdsPatientDetailsAsync(nhsNumber);

                return patient;
            });
    }
}
