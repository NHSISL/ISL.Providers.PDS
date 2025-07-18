// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Brokers.FakeFHIR;
using ISL.Providers.PDS.FakeFHIR.Brokers.Identifiers;
using ISL.Providers.PDS.FakeFHIR.Models;
using System;
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

        public ValueTask<PdsResponse> PatientLookupByDetailsAsync(
            string surname,
            string postcode,
            DateTimeOffset dateOfBirth) =>
            TryCatch(async () =>
            {
                ValidatePatientLookupByDetailsArguments(surname, postcode);

                Guid responseId = await identifierBroker.GetIdentifierAsync();

                PdsResponse pdsResponse = new PdsResponse
                {
                    ResponseId = responseId,
                    FirstName = "Jane",
                    Surname = surname,
                    Address = "1 Trevelyan Square, Boar Lane, City Centre,Leeds, West Yorkshire",
                    EmailAddress = "jane.smith@example.com",
                    PhoneNumber = "01632960587",
                    Postcode = postcode,
                    DateOfBirth = dateOfBirth,
                };

                string nhsNumber = await fakeFHIRBroker.GetNhsNumberAsync(surname, postcode, dateOfBirth);
                pdsResponse.NhsNumber = nhsNumber;

                return pdsResponse;
            });

        public ValueTask<PdsResponse> PatientLookupByNhsNumberAsync(string nhsNumber) =>
            TryCatch(async () =>
            {
                ValidatePatientLookupByNhsNumberArguments(nhsNumber);

                Guid responseId = await identifierBroker.GetIdentifierAsync();

                PdsResponse pdsResponse = new PdsResponse
                {
                    ResponseId = responseId,
                    NhsNumber = nhsNumber
                };

                PdsPatientDetails pdsPatientDetails = await fakeFHIRBroker.GetPdsPatientDetailsAsync(nhsNumber);

                pdsResponse.FirstName = pdsPatientDetails.FirstName;
                pdsResponse.Surname = pdsPatientDetails.Surname;
                pdsResponse.Address = pdsPatientDetails.Address;
                pdsResponse.EmailAddress = pdsPatientDetails.EmailAddress;
                pdsResponse.PhoneNumber = pdsPatientDetails.PhoneNumber;
                pdsResponse.Postcode = pdsPatientDetails.Postcode;
                pdsResponse.DateOfBirth = pdsPatientDetails.DateOfBirth;

                return pdsResponse;
            });
    }
}
