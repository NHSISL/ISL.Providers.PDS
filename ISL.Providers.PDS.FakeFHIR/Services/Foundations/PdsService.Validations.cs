// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using System;
using System.Linq;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal partial class PdsService
    {
        private static void ValidatePatientLookupByDetailsArguments(string searchParams)
        {
            Validate(
                (Rule: IsInvalid(searchParams),
                Parameter: nameof(searchParams)));
        }

        private static void ValidatePatientLookupByNhsNumberArguments(string nhsNumber)
        {
            Validate(
                (Rule: IsInvalidIdentifier(nhsNumber),
                Parameter: nameof(nhsNumber)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidIdentifier(string name) => new
        {
            Condition = String.IsNullOrWhiteSpace(name) || IsExactTenDigits(name) is false,
            Message = "Text must be exactly 10 digits."
        };

        private static bool IsExactTenDigits(string input)
        {
            bool result = input.Length == 10 && input.All(char.IsDigit);

            return result;
        }


        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidIdentificationRequestException =
                new InvalidArgumentPdsException(
                    message: "Invalid Pds argument. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidIdentificationRequestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidIdentificationRequestException.ThrowIfContainsErrors();
        }
    }
}
