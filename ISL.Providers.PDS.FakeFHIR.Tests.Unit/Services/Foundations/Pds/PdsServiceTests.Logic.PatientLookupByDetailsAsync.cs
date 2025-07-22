// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputSearchParams = randomString.DeepClone();

            Bundle randomBundle = CreateRandomBundle();
            PatientBundle randomPatientBundle = CreateRandomPatientBundle(randomBundle);
            PatientBundle output = randomPatientBundle.DeepClone();
            PatientBundle expectedResponse = output.DeepClone();

            this.fakeFHIRBrokerMock.Setup(broker =>
                broker.GetNhsNumberAsync(inputSearchParams))
                    .ReturnsAsync(randomBundle);

            // when
            PatientBundle actualResponse = await this.pdsService
                .PatientLookupByDetailsAsync(inputSearchParams);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);

            this.fakeFHIRBrokerMock.Verify(broker =>
                broker.GetNhsNumberAsync(inputSearchParams), 
                    Times.Once());

            this.fakeFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
