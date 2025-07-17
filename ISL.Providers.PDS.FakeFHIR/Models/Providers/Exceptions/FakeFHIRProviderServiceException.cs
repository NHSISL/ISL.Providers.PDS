// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a service error occurs while using the provider. 
    /// For example, if there is a problem with the server or any other service failure.
    /// </summary>
    public class FakeFHIRProviderServiceException : Xeption, IPdsProviderServiceException
    {
        public FakeFHIRProviderServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
