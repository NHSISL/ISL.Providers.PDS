// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.Abstractions
{
    public interface IPdsOperations
    {
        /// <summary>
        /// Uses PDS FHIR API to obtain the NHS Number for a patient given provided search parameters
        /// </summary>
        /// <returns>
        /// A PatientBundle object containing a list of matched patients
        /// </returns>
        ValueTask<PatientBundle> PatientLookupByDetailsAsync(string searchParams);

        /// <summary>
        /// Uses PDS FHIR API to obtain the patient details
        /// for a patient given their NHS Number 
        /// </summary>
        /// <returns>
        /// A PatientBundle object containing a list with one item of corresponding patient 
        /// </returns>
        ValueTask<PatientBundle> PatientLookupByNhsNumberAsync(string nhsNumber);
    }
}
