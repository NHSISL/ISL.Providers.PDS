// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;

namespace ISL.Providers.PDS.FakeFHIR.Mappers
{
    public static class BundleMapper
    {
        public static Bundle FromListOfPatients(List<Patient> patients)
        {
            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Searchset,
                Total = patients.Count,
                Timestamp = DateTimeOffset.UtcNow
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            foreach (Patient patient in patients) {
                var bundleEntry = new Bundle.EntryComponent
                {
                    FullUrl = $"https://api.service.nhs.uk/personal-demographics/FHIR/R4/Patient/{patient.Id}",
                    Search = new Bundle.SearchComponent { Score = 1 },
                    Resource = patient
                };

                bundle.Entry.Add(bundleEntry);
            }

            return bundle;
        }
    }
}
