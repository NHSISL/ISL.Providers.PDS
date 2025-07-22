// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Brokers.FakeFHIR;
using ISL.Providers.PDS.FakeFHIR.Brokers.Identifiers;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal partial class PdsService : IPdsService
    {
        private readonly IFakeFHIRBroker fakeFHIRBroker;
        private readonly IIdentifierBroker identifierBroker;

        public PdsService(IFakeFHIRBroker fakeFHIRBroker, IIdentifierBroker identifierBroker)
        {
            this.fakeFHIRBroker = fakeFHIRBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<PatientBundle> PatientLookupByDetailsAsync(string searchParams) =>
            TryCatch(async () =>
            {
                ValidatePatientLookupByDetailsArguments(searchParams);

                PatientBundle patientBundle = new PatientBundle();

                Bundle bundle = await fakeFHIRBroker.GetNhsNumberAsync(searchParams);

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
