// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            string randomInputString = GenerateRandom10DigitNumber();
            string inputNhsNumber = randomInputString.DeepClone();
            Patient randomPatient = CreateRandomPatient();
            Patient outputPatient = randomPatient.DeepClone();
            Patient expectedResponse = outputPatient.DeepClone();

            this.fakeFHIRBrokerMock.Setup(broker =>
                broker.GetPdsPatientDetailsAsync(inputNhsNumber))
                    .ReturnsAsync(outputPatient);

            // when
            Patient actualResponse = await this.pdsService
                .PatientLookupByNhsNumberAsync(inputNhsNumber);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);

            this.fakeFHIRBrokerMock.Verify(broker =>
                broker.GetPdsPatientDetailsAsync(inputNhsNumber), 
                    Times.Once());

            this.fakeFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
