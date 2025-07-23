// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions
{
    public class NotFoundPdsPatientDetailsException : Xeption
    {
        public NotFoundPdsPatientDetailsException(string nhsNumber)
            : base(message: $"Couldn't find pds patient with nhsNumber: {nhsNumber}.")
        { }
    }
}
