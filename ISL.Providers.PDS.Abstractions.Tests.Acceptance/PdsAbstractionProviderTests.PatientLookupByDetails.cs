// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ISL.Providers.PDS.Abstractions.Tests.Acceptance
{
    public partial class PdsAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            string randomInputSearchParams = GetRandomString();
            string inputSearchParams = randomInputSearchParams.DeepClone();
            PatientBundle randomOutputPatientBundle = CreateRandomPatientBundle();
            PatientBundle outputPatientBundle = randomOutputPatientBundle.DeepClone();
            PatientBundle expectedPatientBundle = outputPatientBundle.DeepClone();

            this.pdsProviderMock.Setup(provider =>
                provider.PatientLookupByDetailsAsync(inputSearchParams))
                    .ReturnsAsync(outputPatientBundle);

            // when
            PatientBundle actualPatientBundle =
                await this.pdsAbstractionProvider.PatientLookupByDetailsAsync(inputSearchParams);

            // then
            actualPatientBundle.Should().BeEquivalentTo(expectedPatientBundle);
        }
    }
}
