// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            Guid randomIdentifier = Guid.NewGuid();
            string randomInputString = GenerateRandom10DigitNumber();
            string inputNhsNumber = randomInputString.DeepClone();
            PdsPatientDetails randomPdsPatientDetails = CreateRandomPdsPatientDetails();
            PdsPatientDetails outputPdsPatientDetails = randomPdsPatientDetails.DeepClone();

            PdsResponse randomPdsResponse = CreateRandomPdsResponse(
                responseId: randomIdentifier,
                nhsNumber: inputNhsNumber,
                pdsPatientDetails: outputPdsPatientDetails);

            PdsResponse output = randomPdsResponse.DeepClone();
            PdsResponse expectedResponse = output.DeepClone();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomIdentifier);

            this.fakeFHIRBrokerMock.Setup(broker =>
                broker.GetPdsPatientDetailsAsync(inputNhsNumber))
                    .ReturnsAsync(outputPdsPatientDetails);

            // when
            PdsResponse actualResponse = await this.pdsService
                .PatientLookupByNhsNumberAsync(inputNhsNumber);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once());

            this.fakeFHIRBrokerMock.Verify(broker =>
                broker.GetPdsPatientDetailsAsync(inputNhsNumber), 
                    Times.Once());

            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.fakeFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
