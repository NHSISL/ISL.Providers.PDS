// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;

namespace ISL.Providers.PDS.FHIR.Services.Foundations.Pds
{
    internal partial class PdsService
    {
        private static void ValidatePatientLookupByNhsNumberArguments(string nhsNumber)
        {
            Validate(
                (Rule: IsInvalidIdentifier(nhsNumber),
                Parameter: nameof(nhsNumber)));
        }

        private static dynamic IsInvalidIdentifier(string name) => new
        {
            Condition = String.IsNullOrWhiteSpace(name) || IsValidNhsNumber(name) is false,
            Message = "Text must be a valid NHS Number."
        };

        private static bool IsValidNhsNumber(string nhsNumber)
        {
            // Remove spaces
            nhsNumber = nhsNumber.Replace(" ", string.Empty);

            // Must be 10 digits
            if (nhsNumber.Length != 10 || !nhsNumber.All(char.IsDigit))
                return false;

            // Split into first 9 digits + check digit
            var digits = nhsNumber.Select(c => c - '0').ToArray();
            int checkDigit = digits[9];

            // Weighting factors 10 to 2
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += digits[i] * (10 - i);
            }

            int remainder = sum % 11;
            int expectedCheckDigit = 11 - remainder;

            if (expectedCheckDigit == 11) expectedCheckDigit = 0;
            if (expectedCheckDigit == 10) return false; // invalid number

            return checkDigit == expectedCheckDigit;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPdsException =
                new InvalidPdsArgumentException(
                    message: "Invalid PDS argument. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPdsException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPdsException.ThrowIfContainsErrors();
        }
    }
}
