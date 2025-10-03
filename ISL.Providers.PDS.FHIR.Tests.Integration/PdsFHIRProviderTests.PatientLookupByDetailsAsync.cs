// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Integration
{
    public partial class PdsFHIRProviderTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            string inputSurname = "Jones26th";
            string inputPostcode = "EN3 7DG";
            string inputDateOfBirth = "1997-01-02";

            // when
            PatientBundle actualResponse =
                await this.pdsFHIRProvider.PatientLookupByDetailsAsync(
                    familyName: inputSurname,
                    postcode: inputPostcode,
                    dateOfBirth: inputDateOfBirth);
        }
    }
}
