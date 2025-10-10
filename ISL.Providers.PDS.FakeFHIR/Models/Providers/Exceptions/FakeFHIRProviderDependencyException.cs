// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a dependency error occurs while using the provider.
    /// For example, if a required dependency is unavailable or incompatible.
    /// </summary>
    public class FakeFHIRProviderDependencyException : Xeption, IPdsProviderDependencyException
    {
        public FakeFHIRProviderDependencyException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
