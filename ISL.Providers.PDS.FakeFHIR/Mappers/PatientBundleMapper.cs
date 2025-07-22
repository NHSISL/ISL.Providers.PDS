// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using System.Collections.Generic;

namespace ISL.Providers.PDS.FakeFHIR.Mappers
{
    public static class PatientBundleMapper
    {
        public static PatientBundle FromBundle(Bundle bundle)
        {
            return new PatientBundle
            {
                Id = bundle.Id,
                Type = bundle.Type?.ToString(),
                Total = bundle.Total,
                Timestamp = bundle.Timestamp.ToString(),
                Links = bundle.Link ?? new List<Bundle.LinkComponent>(),
                Patients = ExtractPatients(bundle)
            };
        }

        private static List<Patient> ExtractPatients(Bundle bundle)
        {
            var patients = new List<Patient>();

            foreach (var entry in bundle.Entry)
            {
                if (entry.Resource is Patient patient)
                {
                    patients.Add(patient);
                }
            }

            return patients;
        }
    }
}
