﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.PDS.FHIR.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a dependency error occurs while using the pds provider.
    /// For example, if a required dependency is unavailable or incompatible.
    /// </summary>
    public class PdsFHIRProviderDependencyException
        : Xeption, IPdsProviderDependencyException
    {
        public PdsFHIRProviderDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
