// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.PDS.FHIR.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a service error occurs while using the pds provider. 
    /// For example, if there is a problem with the server or any other service failure.
    /// </summary>
    public class PdsFHIRProviderServiceException : Xeption, IPdsProviderServiceException
    {
        public PdsFHIRProviderServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
