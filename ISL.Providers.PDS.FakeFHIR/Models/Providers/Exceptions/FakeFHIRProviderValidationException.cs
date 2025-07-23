// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using System.Collections;
using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a validation error occurs while using the provider.
    /// For example, if required data is missing or invalid.
    /// </summary>
    public class FakeFHIRProviderValidationException : Xeption, IPdsProviderValidationException
    {
        public FakeFHIRProviderValidationException(string message, Xeption innerException, IDictionary data)
            : base(message: message, innerException, data)
        { }
    }
}
