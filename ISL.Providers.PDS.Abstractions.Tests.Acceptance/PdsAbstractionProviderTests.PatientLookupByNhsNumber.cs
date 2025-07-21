// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;

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
            Patient randomOutputPatient = CreateRandomPatient();
            Patient outputPatient = randomOutputPatient.DeepClone();
            Patient expectedPatient = outputPatient.DeepClone();

            this.pdsProviderMock.Setup(provider =>
                provider.PatientLookupByNhsNumberAsync(inputNhsNumber))
                    .ReturnsAsync(outputPatient);

            // when
            Patient actualPatient =
                await this.pdsAbstractionProvider.PatientLookupByNhsNumberAsync(inputNhsNumber);

            // then
            actualPatient.Should().BeEquivalentTo(expectedPatient);
        }
    }
}
