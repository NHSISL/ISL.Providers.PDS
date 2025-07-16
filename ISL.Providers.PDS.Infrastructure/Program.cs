// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Infrastructure.Services;

namespace ISL.Providers.PDS.Infrastructure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var scriptGenerationService = new ScriptGenerationService();

            scriptGenerationService.GenerateBuildScript(
                branchName: "main",
                projectName: "ISL.Providers.PDS.Abstractions",
                dotNetVersion: "9.0.100");

            scriptGenerationService.GeneratePrLintScript("main");
        }
    }
}
