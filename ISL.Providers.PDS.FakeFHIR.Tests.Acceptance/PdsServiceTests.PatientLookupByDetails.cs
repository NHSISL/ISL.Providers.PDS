// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Acceptance
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            string randomString = GetRandomString();
            string searchParams = randomString.DeepClone();

            PatientBundle randomPatientBundle = CreateRandomPatientBundle();
            PatientBundle expectedResponse = randomPatientBundle.DeepClone();

            // when
            PatientBundle actualResponse =
                await this.fakeFHIRProvider.PatientLookupByDetailsAsync(searchParams);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
