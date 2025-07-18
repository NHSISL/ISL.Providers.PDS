// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV3s;

namespace ISL.Providers.PDS.Infrastructure.Services
{
    internal class ScriptGenerationService
    {
        private readonly ADotNetClient adotNetClient;

        public ScriptGenerationService() =>
            adotNetClient = new ADotNetClient();

        public void GenerateBuildScript(string branchName, string projectName, string dotNetVersion)
        {
            var githubPipeline = new GithubPipeline
            {
                Name = ".Net",

                OnEvents = new Events
                {
                    Push = new PushEvent { Branches = [branchName] },

                    PullResponse = new PullResponseEvent
                    {
                        Types = ["opened", "synchronize", "reopened", "closed"],
                        Branches = [branchName]
                    }
                },

                Jobs = new Dictionary<string, Job>
                {
                    {
                        "build",
                        new Job
                        {
                            Name = "Build",
                            RunsOn = BuildMachines.WindowsLatest,

                            EnvironmentVariables = new Dictionary<string, string>
                            { },

                            Steps = new List<GithubTask>
                            {
                                new CheckoutTaskV3
                                {
                                    Name = "Check out"
                                },

                                new SetupDotNetTaskV3
                                {
                                    Name = "Setup .Net",

                                    With = new TargetDotNetVersionV3
                                    {
                                        DotNetVersion = "9.0.100"
                                    }
                                },

                                new RestoreTask
                                {
                                    Name = "Restore"
                                },

                                new DotNetBuildTask
                                {
                                    Name = "Build"
                                },

                                new TestTask
                                {
                                    Name = "Unit Tests - Abstraction",
                                    Run = "dotnet test ISL.Providers.PDS.Abstractions.Tests.Unit/ISL.Providers.PDS.Abstractions.Tests.Unit.csproj --no-build --verbosity normal"
                                },

                                new TestTask
                                {
                                    Name = "Unit Tests - Fake FHIR Provider",
                                    Run = "dotnet test ISL.Providers.PDS.FakeFHIR.Tests.Unit/ISL.Providers.PDS.FakeFHIR.Tests.Unit.csproj --no-build --verbosity normal"
                                },

                                new TestTask
                                {
                                    Name = "Unit Tests - FHIR Provider",
                                    Run = "dotnet test ISL.Providers.PDS.FHIR.Tests.Unit/ISL.Providers.PDS.FHIR.Tests.Unit.csproj --no-build --verbosity normal"
                                },

                                new TestTask
                                {
                                    Name = "Acceptance Tests - Abstraction",
                                    Run = "dotnet test ISL.Providers.PDS.Abstractions.Tests.Acceptance/ISL.Providers.PDS.Abstractions.Tests.Acceptance.csproj --no-build --verbosity normal"
                                },

                                new TestTask
                                {
                                    Name = "Acceptance Tests - Fake FHIR Provider",
                                    Run = "dotnet test ISL.Providers.PDS.FakeFHIR.Tests.Acceptance/ISL.Providers.PDS.FakeFHIR.Tests.Acceptance.csproj --no-build --verbosity normal"
                                },

                                new TestTask
                                {
                                    Name = "Acceptance Tests - FHIR Provider",
                                    Run = "dotnet test ISL.Providers.PDS.FHIR.Tests.Unit/ISL.Providers.PDS.FHIR.Tests.Unit.csproj --no-build --verbosity normal"
                                },
                            }
                        }
                    },
                    {
                        "add_tag",
                        new TagJob(
                            runsOn: BuildMachines.UbuntuLatest,
                            dependsOn: "build",
                            projectRelativePath: $"{projectName}/{projectName}.csproj",
                            githubToken: "${{ secrets.PAT_FOR_TAGGING }}",
                            branchName: branchName)
                        {
                            Name = "Tag and Release"
                        }
                    },
                    {
                        "publish",
                        new PublishJobV2(
                            runsOn: BuildMachines.UbuntuLatest,
                            dependsOn: "add_tag",
                            dotNetVersion: dotNetVersion,
                            nugetApiKey: "${{ secrets.NUGET_ACCESS }}")
                        {
                            Name = "Publish to NuGet"
                        }
                    }
                }
            };

            string buildScriptPath = "../../../../.github/workflows/build.yml";
            string directoryPath = Path.GetDirectoryName(buildScriptPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            adotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: buildScriptPath);
        }

        public void GeneratePrLintScript(string branchName)
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "PR Linter",

                OnEvents = new Events
                {
                    PullResponse = new PullResponseEvent
                    {
                        Types = ["opened", "edited", "synchronize", "reopened", "closed"],
                        Branches = [branchName]
                    }
                },

                Jobs = new Dictionary<string, Job>
                {
                    {
                        "label",
                        new LabelJobV2(runsOn: BuildMachines.UbuntuLatest)
                        {
                            Name = "Add Label(s)",
                        }
                    },
                    {
                        "requireIssueOrTask",
                        new RequireIssueOrTaskJob()
                        {
                            Name = "Require Issue Or Task Association",
                        }
                    },
                }
            };

            string buildScriptPath = "../../../../.github/workflows/prLinter.yml";
            string directoryPath = Path.GetDirectoryName(buildScriptPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            adotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: buildScriptPath);
        }
    }
}
