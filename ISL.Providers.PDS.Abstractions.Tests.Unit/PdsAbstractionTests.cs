// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Moq;

namespace ISL.Providers.PDS.Abstractions.Tests.Unit
{
    public partial class PdsAbstractionTests
    {
        private readonly Mock<IPdsProvider> pdsMock;
        private readonly PdsAbstractionProvider pdsAbstractionProvider;

        public PdsAbstractionTests()
        {
            this.pdsMock = new Mock<IPdsProvider>();

            this.pdsAbstractionProvider =
                new PdsAbstractionProvider(this.pdsMock.Object);
        }
    }
}
