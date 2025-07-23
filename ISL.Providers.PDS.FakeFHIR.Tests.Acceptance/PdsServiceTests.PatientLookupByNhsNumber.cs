// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.FakeFHIR.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Acceptance
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            var fakePatients = this.configuration
                .GetSection("fakeFHIRProviderConfigurations:FakePatients").Get<List<FakeFHIRProviderPatientDetails>>();

            string someNhsNumber = "9876543210";
            string inputNhsNumber = someNhsNumber.DeepClone();
            Patient randomPatient = CreatePatient(fakePatients, inputNhsNumber);
            Patient expectedResponse = randomPatient.DeepClone();

            // when
            Patient actualResponse =
                await this.fakeFHIRProvider.PatientLookupByNhsNumberAsync(inputNhsNumber);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
