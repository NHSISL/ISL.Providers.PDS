// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Acceptance
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            var fakePatients = this.configuration
                .GetSection("fakeFHIRProviderConfigurations:FakePatients").Get<List<FakeFHIRProviderPatientDetails>>();

            string randomString = "Smith";
            string inputSurname = randomString.DeepClone();

            PatientBundle randomPatientBundle = CreatePatientBundle(fakePatients, inputSurname);
            PatientBundle expectedResponse = randomPatientBundle.DeepClone();

            // when
            PatientBundle actualResponse =
                await this.fakeFHIRProvider.PatientLookupByDetailsAsync(null,
                    inputSurname,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
