// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Brokers.FakeFHIR
{
    internal class FakeFHIRBroker : IFakeFHIRBroker
    {
        public FakeFHIRBroker()
        { }

        public static Bundle CreateFakePatientSearchBundle()
        {
            var periodStartFhirDateTime = new FhirDateTime("2020-01-01");
            var periodEndFhirDateTime = new FhirDateTime("2021-12-31");

            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Searchset,
                Total = 1,
                Timestamp = DateTimeOffset.Parse("2025-07-21T08:30:33.723Z")
            };

            var patient = new Patient
            {
                Id = "9000000009",
                BirthDate = "2010-10-22",
                Gender = AdministrativeGender.Female,
                Deceased = new FhirDateTime("2010-10-22T00:00:00+00:00"),
                MultipleBirth = new Integer(1),
                Meta = new Meta
                {
                    VersionId = "2",
                    Security = new List<Coding>
                {
                    new Coding("http://terminology.hl7.org/CodeSystem/v3-Confidentiality", "U", "unrestricted")
                }
                },
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "https://fhir.nhs.uk/Id/nhs-number",
                        Value = "9000000009",
                        Extension = new List<Extension>
                        {
                            new Extension()
                            {
                                Extension = new List<Extension>
                                {
                                    new Extension("valueCodeableConcept", new CodeableConcept
                                    {
                                        Coding = new List<Coding>
                                        {
                                            new Coding("https://fhir.hl7.org.uk/CodeSystem/UKCore-NHSNumberVerificationStatus", "01", "Number present and verified")
                                            {
                                                Version = "1.0.0"
                                            }
                                        }
                                    })
                                }
                            }
                        }
                    }
                },
                Name = new List<HumanName>
                {
                    new HumanName
                    {
                        ElementId = "123",
                        Use = HumanName.NameUse.Usual,
                        Family = "Smith",
                        Given = new[] { "Jane" },
                        Prefix = new[] { "Mrs" },
                        Suffix = new[] { "MBE" },
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime)
                    }
                },
                Telecom = new List<ContactPoint>
                {
                    new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Home, "01632960587")
                    {
                        ElementId = "789",
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime)
                    },
                    new ContactPoint(ContactPoint.ContactPointSystem.Email, ContactPoint.ContactPointUse.Home, "jane.smith@example.com")
                    {
                        ElementId = "790",
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime)
                    },
                    new ContactPoint(ContactPoint.ContactPointSystem.Other, ContactPoint.ContactPointUse.Home, "01632960587")
                    {
                        ElementId = "OC789",
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime),
                        Extension = new List<Extension>
                        {
                            new Extension("https://fhir.hl7.org.uk/StructureDefinition/Extension-UKCore-OtherContactSystem",
                                new Coding("https://fhir.hl7.org.uk/CodeSystem/UKCore-OtherContactSystem", "textphone", "Minicom (Textphone)"))
                        }
                    }
                },
                Address = new List<Address>
                {
                    new Address
                    {
                        ElementId = "456",
                        Use = Address.AddressUse.Home,
                        Line = new List<string>
                        {
                            "1 Trevelyan Square",
                            "Boar Lane",
                            "City Centre",
                            "Leeds",
                            "West Yorkshire"
                        },
                        PostalCode = "LS1 6AE",
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime),
                        Extension = new List<Extension>
                        {
                            CreateAddressKeyExtension("PAF", "12345678"),
                            CreateAddressKeyExtension("UPRN", "123456789012")
                        }
                    }
                },
                Contact = new List<Patient.ContactComponent>
                {
                    new Patient.ContactComponent
                    {
                        ElementId = "C123",
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime),
                        Relationship = new List<CodeableConcept>
                        {
                            new CodeableConcept("http://terminology.hl7.org/CodeSystem/v2-0131", "C", "Emergency Contact")
                        },
                        Telecom = new List<ContactPoint>
                        {
                            new ContactPoint(ContactPoint.ContactPointSystem.Phone, null, "01632960587")
                        }
                    }
                },
                Extension = new List<Extension>
                {
                    new Extension()
                    {
                        Extension = new List<Extension>
                        {
                            new Extension("deathNotificationStatus", new CodeableConcept
                            {
                                Coding = new List<Coding>
                                {
                                    new Coding
                                    {
                                        System = "https://fhir.hl7.org.uk/CodeSystem/UKCore-DeathNotificationStatus",
                                        Code = "2",
                                        Display = "Formal - death notice received from Registrar of Deaths",
                                        Version = "1.0.0"
                                    }
                                }
                            }),
                            new Extension("systemEffectiveDate", new FhirDateTime("2010-10-22T00:00:00+00:00"))
                        }
                    }
                },
                GeneralPractitioner = new List<ResourceReference>
                {
                    new ResourceReference
                    {
                        ElementId = "254406A3",
                        Type = "Organization",
                        Identifier = new Identifier
                        {
                            System = "https://fhir.nhs.uk/Id/ods-organization-code",
                            Value = "Y12345",
                            Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime)
                        }
                    }
                }
            };

            bundle.Entry = new List<Bundle.EntryComponent>
            {
                new Bundle.EntryComponent
                {
                    FullUrl = "https://api.service.nhs.uk/personal-demographics/FHIR/R4/Patient/9000000009",
                    Search = new Bundle.SearchComponent { Score = 1 },
                    Resource = patient
                }
            };

            return bundle;
        }

        public static Patient CreateFakePatient()
        {
            var patient = new Patient
            {
                Id = "9000000009",
                Gender = AdministrativeGender.Female,
                BirthDate = "2010-10-22",
                MultipleBirth = new Integer(1),
                Deceased = new FhirDateTime("2010-10-22T00:00:00+00:00"),
                Meta = new Meta
                {
                    VersionId = "2",
                    Security = new List<Coding>
                {
                    new Coding("http://terminology.hl7.org/CodeSystem/v3-Confidentiality", "U", "unrestricted")
                }
                },

                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "https://fhir.nhs.uk/Id/nhs-number",
                        Value = "9000000009",
                        Extension = new List<Extension>
                        {
                            new Extension()
                            {
                                Value = new CodeableConcept
                                {
                                    Coding = new List<Coding>
                                    {
                                        new Coding
                                        {
                                            System = "https://fhir.hl7.org.uk/CodeSystem/UKCore-NHSNumberVerificationStatus",
                                            Version = "1.0.0",
                                            Code = "01",
                                            Display = "Number present and verified"
                                        }
                                    }
                                }
                            }
                        }
                    }
                },

                Name = new List<HumanName>
                {
                    new HumanName
                    {
                        ElementId = "123",
                        Use = HumanName.NameUse.Usual,
                        Family = "Smith",
                        Given = new[] { "Jane" },
                        Prefix = new[] { "Mrs" },
                        Period = new Period(new FhirDateTime("2020-01-01"), new FhirDateTime("2021-12-31"))
                    }
                },

                Telecom = new List<ContactPoint>
                {
                    new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Home, "01632960587")
                    {
                        ElementId = "789",
                        Period = new Period(new FhirDateTime("2020-01-01"), new FhirDateTime("2021-12-31"))
                    },
                    new ContactPoint(ContactPoint.ContactPointSystem.Email, ContactPoint.ContactPointUse.Home, "jane.smith@example.com")
                    {
                        ElementId = "790",
                        Period = new Period(new FhirDateTime("2019-01-01"), new FhirDateTime("2022-12-31"))
                    },
                    new ContactPoint(ContactPoint.ContactPointSystem.Other, ContactPoint.ContactPointUse.Home, "01632960587")
                    {
                        ElementId = "OC789",
                        Period = new Period(new FhirDateTime("2020-01-01"), new FhirDateTime("2021-12-31")),
                        Extension = new List<Extension>
                        {
                            new Extension("https://fhir.hl7.org.uk/StructureDefinition/Extension-UKCore-OtherContactSystem",
                                new Coding("https://fhir.hl7.org.uk/CodeSystem/UKCore-OtherContactSystem", "textphone", "Minicom (Textphone)"))
                        }
                    }
                },

                Contact = new List<Patient.ContactComponent>
                {
                    new Patient.ContactComponent
                    {
                        ElementId = "C123",
                        Period = new Period(new FhirDateTime("2020-01-01"), new FhirDateTime("2021-12-31")),
                        Relationship = new List<CodeableConcept>
                        {
                            new CodeableConcept("http://terminology.hl7.org/CodeSystem/v2-0131", "C", "Emergency Contact")
                        },
                        Telecom = new List<ContactPoint>
                        {
                            new ContactPoint(ContactPoint.ContactPointSystem.Phone, null, "01632960587")
                        }
                    }
                },

                Address = new List<Address>
                {
                    CreateAddress("456", Address.AddressUse.Home),
                    CreateAddress("T456", Address.AddressUse.Temp, "Student Accommodation")
                },

                GeneralPractitioner = new List<ResourceReference>
                {
                    new ResourceReference
                    {
                        ElementId = "254406A3",
                        Type = "Organization",
                        Identifier = new Identifier
                        {
                            System = "https://fhir.nhs.uk/Id/ods-organization-code",
                            Value = "Y12345",
                            Period = new Period(new FhirDateTime("2020-01-01"), new FhirDateTime("2021-12-31"))
                        }
                    }
                },

                ManagingOrganization = new ResourceReference
                {
                    Type = "Organization",
                    Identifier = new Identifier
                    {
                        System = "https://fhir.nhs.uk/Id/ods-organization-code",
                        Value = "Y12345",
                        Period = new Period(new FhirDateTime("2020-01-01"), new FhirDateTime("2021-12-31"))
                    }
                },

                Extension = new List<Extension>
                {
                    CreateReferenceExtension("https://fhir.hl7.org.uk/StructureDefinition/Extension-UKCore-NominatedPharmacy", "Y12345"),
                    CreateReferenceExtension("https://fhir.hl7.org.uk/StructureDefinition/Extension-UKCore-PreferredDispenserOrganization", "Y23456"),
                    CreateReferenceExtension("https://fhir.hl7.org.uk/StructureDefinition/Extension-UKCore-MedicalApplianceSupplier", "Y34567"),
                    new Extension()
                    {
                        Extension = new List<Extension>
                        {
                            new Extension("deathNotificationStatus", new CodeableConcept
                            {
                                Coding = new List<Coding>
                                {
                                    new Coding("https://fhir.hl7.org.uk/CodeSystem/UKCore-DeathNotificationStatus", "2", "Formal - death notice received from Registrar of Deaths")
                                    {
                                        Version = "1.0.0"
                                    }
                                }
                            }),
                            new Extension("systemEffectiveDate", new FhirDateTime("2010-10-22T00:00:00+00:00"))
                        }
                    },
                    new Extension()
                    {
                        Extension = new List<Extension>
                        {
                            new Extension("language", new CodeableConcept
                            {
                                Coding = new List<Coding>
                                {
                                    new Coding("https://fhir.hl7.org.uk/CodeSystem/UKCore-HumanLanguage", "fr", "French")
                                    {
                                        Version = "1.0.0"
                                    }
                                }
                            }),
                            new Extension("interpreterRequired", new FhirBoolean(true))
                        }
                    },
                    new Extension()
                    {
                        Extension = new List<Extension>
                        {
                            new Extension("PreferredWrittenCommunicationFormat", new CodeableConcept
                            {
                                Coding = new List<Coding>
                                {
                                    new Coding("https://fhir.hl7.org.uk/CodeSystem/UKCore-PreferredWrittenCommunicationFormat", "12", "Braille")
                                }
                            }),
                            new Extension("PreferredContactMethod", new CodeableConcept
                            {
                                Coding = new List<Coding>
                                {
                                    new Coding("https://fhir.hl7.org.uk/CodeSystem/UKCore-PreferredContactMethod", "1", "Letter")
                                }
                            }),
                            new Extension("PreferredContactTimes", new FhirString("Not after 7pm"))
                        }
                    },
                    new Extension()
                    {
                        Value = new Address
                        {
                            City = "Manchester",
                            District = "Greater Manchester",
                            Country = "GBR"
                        }
                    },
                    new Extension()
                    {
                        Extension = new List<Extension>
                        {
                            new Extension("removalFromRegistrationCode", new CodeableConcept
                            {
                                Coding = new List<Coding>
                                {
                                    new Coding("https://fhir.nhs.uk/CodeSystem/PDS-RemovalReasonExitCode", "SCT", "Transferred to Scotland")
                                }
                            }),

                            new Extension("effectiveTime", new Period(
                                new FhirDateTime("2020-01-01T00:00:00+00:00"), 
                                new FhirDateTime("2021-12-31T00:00:00+00:00")))
                        }
                    }
                }
            };

            return patient;
        }

        private static Extension CreateReferenceExtension(string url, string value)
        {
            return new Extension(url,
                new ResourceReference
                {
                    Identifier = new Identifier("https://fhir.nhs.uk/Id/ods-organization-code", value)
                });
        }

        private static Address CreateAddress(string id, Address.AddressUse use, string text = null)
        {
            return new Address
            {
                ElementId = id,
                Use = use,
                Text = text,
                Line = new List<string>
            {
                "1 Trevelyan Square",
                "Boar Lane",
                "City Centre",
                "Leeds",
                "West Yorkshire"
            },
                PostalCode = "LS1 6AE",
                Period = new Period(new FhirDateTime("2020-01-01"), new FhirDateTime("2021-12-31")),
                Extension = new List<Extension>
            {
                CreateAddressKeyExtension("PAF", "12345678"),
                CreateAddressKeyExtension("UPRN", "123456789012")
            }
            };
        }

        private static Extension CreateAddressKeyExtension(string typeCode, string valueString)
        {
            return new Extension("https://fhir.hl7.org.uk/StructureDefinition/Extension-UKCore-AddressKey",
                new FhirString(valueString))
            {
                Extension = new List<Extension>
            {
                new Extension("type", new Coding("https://fhir.hl7.org.uk/CodeSystem/UKCore-AddressKeyType", typeCode)),
                new Extension("value", new FhirString(valueString))
            }
            };
        }

        public async ValueTask<Bundle> GetNhsNumberAsync(string searchParams) {
            Bundle bundle = CreateFakePatientSearchBundle();

            return bundle;
        }

        public async ValueTask<Patient> GetPdsPatientDetailsAsync(string nhsNumber) {
            var patient = CreateFakePatient();

            return patient;
        }
    }
}
