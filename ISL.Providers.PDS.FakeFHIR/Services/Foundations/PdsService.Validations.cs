// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.FakeFHIR.Models;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using System;
using System.Linq;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal partial class PdsService
    {
        private static void ValidatePatientLookupByNhsNumberArguments(string nhsNumber)
        {
            Validate(
                (Rule: IsInvalidIdentifier(nhsNumber),
                Parameter: nameof(nhsNumber)));
        }

        private static void ValidatePdsPatientDetails(PdsPatientDetails maybePdsPatientDetails, string nhsNumber)
        {
            if (maybePdsPatientDetails is null)
            {
                throw new NotFoundPdsPatientDetailsException(nhsNumber);
            }
        }

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
