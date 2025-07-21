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
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            string randomInputNhsNumber = GetRandomString();
            string inputNhsNumber = randomInputNhsNumber.DeepClone();
            PatientBundle randomOutputPdsResponse = CreateRandomPatientBundle();
            PatientBundle outputPdsResponse = randomOutputPdsResponse.DeepClone();
            PatientBundle expectedPdsResponse = outputPdsResponse.DeepClone();

            this.pdsProviderMock.Setup(provider =>
                provider.PatientLookupByNhsNumberAsync(inputNhsNumber))
                    .ReturnsAsync(outputPdsResponse);

            // when
            PatientBundle actualPdsResponse =
                await this.pdsAbstractionProvider.PatientLookupByNhsNumberAsync(inputNhsNumber);

            // then
            actualPdsResponse.Should().BeEquivalentTo(expectedPdsResponse);
        }
    }
}
