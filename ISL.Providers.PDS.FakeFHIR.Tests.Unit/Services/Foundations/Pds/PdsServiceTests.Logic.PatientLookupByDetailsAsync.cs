// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            Guid randomIdentifier = Guid.NewGuid();
            string randomString = GetRandomString();
            string inputSurname = randomString.DeepClone();
            string inputPostcode = randomString.DeepClone();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            DateTimeOffset inputDateOfBirth = randomDateTime.DeepClone();
            string randomOutputString = GenerateRandom10DigitNumber();
            string outputNhsNumber = randomOutputString.DeepClone();

            PdsResponse randomPdsResponse = CreateRandomPdsResponse(
                responseId: randomIdentifier,
                surname: inputSurname,
                postcode: inputPostcode,
                dateOfBirth: inputDateOfBirth,
                nhsNumber: outputNhsNumber);

            PdsResponse output = randomPdsResponse.DeepClone();
            PdsResponse expectedResponse = output.DeepClone();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomIdentifier);

            this.fakeFHIRBrokerMock.Setup(broker =>
                broker.GetNhsNumberAsync(inputSurname, inputPostcode, inputDateOfBirth))
                    .ReturnsAsync(outputNhsNumber);

            // when
            PdsResponse actualResponse = await this.pdsService
                .PatientLookupByDetailsAsync(inputSurname, inputPostcode, inputDateOfBirth);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once());

            this.fakeFHIRBrokerMock.Verify(broker =>
                broker.GetNhsNumberAsync(inputSurname, inputPostcode, inputDateOfBirth), 
                    Times.Once());

            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.fakeFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
