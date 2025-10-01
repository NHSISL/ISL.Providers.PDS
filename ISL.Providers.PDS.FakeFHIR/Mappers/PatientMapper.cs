// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.FakeFHIR.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISL.Providers.PDS.FakeFHIR.Mappers
{
    public static class PatientMapper
    {
        internal static Patient FromPdsPatientDetails(PdsPatientDetails pdsPatientDetails)
        {
            var periodStartFhirDateTime = new FhirDateTime(pdsPatientDetails.DateOfBirth);
            var periodEndFhirDateTime = new FhirDateTime(pdsPatientDetails.DateOfDeath);
            var metaSecurityCode = pdsPatientDetails.IsSensitive ? "R" : "U";
            var metaSecurityDisplay = pdsPatientDetails.IsSensitive ? "restricted" : "unrestricted";

            var patient = new Patient
            {
                Id = pdsPatientDetails.NhsNumber,
                BirthDate = pdsPatientDetails.DateOfBirth.ToString("yyyy-MM-dd"),

                Gender = pdsPatientDetails.Gender.ToLower() switch
                {
                    "male" => AdministrativeGender.Male,
                    "female" => AdministrativeGender.Female,
                    "other" => AdministrativeGender.Other,
                    "unknown" => AdministrativeGender.Unknown,
                    _ => AdministrativeGender.Unknown
                },

                Deceased = new FhirDateTime(pdsPatientDetails.DateOfDeath),
                MultipleBirth = new Integer(1),

                Meta = new Meta
                {
                    VersionId = "2",
                    Security = new List<Coding>
                    {
                        new Coding(
                            system: "http://terminology.hl7.org/CodeSystem/v3-Confidentiality",
                            code: metaSecurityCode,
                            display: metaSecurityDisplay)
                    }
                },

                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "https://fhir.nhs.uk/Id/nhs-number",
                        Value = pdsPatientDetails.NhsNumber,
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
                                            new Coding(
                                                system: "https://fhir.hl7.org.uk/CodeSystem/UKCore-NHSNumberVerificationStatus",
                                                code: "01",
                                                display: "Number present and verified")
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
                        Family = pdsPatientDetails.Surname,
                        Given = pdsPatientDetails.GivenNames,
                        Prefix = new[] { pdsPatientDetails.Title },
                        Suffix = new[] { "MBE" },
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime)
                    }
                },

                Telecom = new List<ContactPoint>
                {
                    new ContactPoint(
                        system: ContactPoint.ContactPointSystem.Phone,
                        use: ContactPoint.ContactPointUse.Home,
                        value: pdsPatientDetails.PhoneNumber)
                    {
                        ElementId = "789",
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime)
                    },

                    new ContactPoint(
                        system: ContactPoint.ContactPointSystem.Email,
                        use: ContactPoint.ContactPointUse.Home,
                        value: pdsPatientDetails.EmailAddress)
                    {
                        ElementId = "790",
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime)
                    },

                    new ContactPoint(
                        system : ContactPoint.ContactPointSystem.Other,
                        use : ContactPoint.ContactPointUse.Home,
                        value : pdsPatientDetails.PhoneNumber)
                    {
                        ElementId = "OC789",
                        Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime),
                        Extension = new List<Extension>
                        {
                            new Extension(
                                url: "https://fhir.hl7.org.uk/StructureDefinition/Extension-UKCore-OtherContactSystem",

                                value: new Coding(
                                    system: "https://fhir.hl7.org.uk/CodeSystem/UKCore-OtherContactSystem",
                                    code: "textphone",
                                    display: "Minicom (Textphone)"))
                        }
                    }
                },

                Address = new List<Address>
                {
                    new Address
                    {
                        ElementId = "456",
                        Use = Address.AddressUse.Home,
                        Line = pdsPatientDetails.Address.Split(",").SkipLast(1).Select(a => a.Trim()),
                        PostalCode = pdsPatientDetails.Address.Split(",").LastOrDefault().Trim(),
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
                            new CodeableConcept(
                                system: "http://terminology.hl7.org/CodeSystem/v2-0131",
                                code: "C",
                                text: "Emergency Contact")
                        },

                        Telecom = new List<ContactPoint>
                        {
                            new ContactPoint(
                                system: ContactPoint.ContactPointSystem.Phone,
                                use: null,
                                value: pdsPatientDetails.PhoneNumber)
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
                            new Extension("systemEffectiveDate", new FhirDateTime(pdsPatientDetails.DateOfDeath))
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
                            Value = pdsPatientDetails.RegisteredGpPractice,
                            Period = new Period(periodStartFhirDateTime, periodEndFhirDateTime)
                        }
                    }
                }
            };

            return patient;
        }

        private static Extension CreateAddressKeyExtension(string typeCode, string valueString)
        {
            return new Extension("https://fhir.hl7.org.uk/StructureDefinition/Extension-UKCore-AddressKey",
                new FhirString(valueString))
            {
                Extension = new List<Extension>
                    {
                        new Extension("type", new Coding(
                            system: "https://fhir.hl7.org.uk/CodeSystem/UKCore-AddressKeyType",
                            code: typeCode)),

                        new Extension("value", new FhirString(valueString))
                    }
            };
        }
    }
}
