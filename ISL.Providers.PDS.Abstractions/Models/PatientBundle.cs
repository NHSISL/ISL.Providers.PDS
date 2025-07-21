// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using System.Collections.Generic;

namespace ISL.Providers.PDS.Abstractions.Models
{
    public class PatientBundle
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        public int? Total { get; set; }
        public string? Timestamp { get; set; }

        public List<Bundle.LinkComponent> Links { get; set; } = new();
        public List<Patient> Patients { get; set; } = new();
    }
}
