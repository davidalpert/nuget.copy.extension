﻿namespace NuGet.Copy.Tests
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using Console = Common.Console;


    public class CopySearchSpecs
    {
        public abstract class CopyTagsSpecsBase : TinySpec
        {
            protected CopySearch command;
            protected string sourceDir1 = @".\source1";
            protected string sourceDir2 = @".\source2";
            protected string destDir1 = @".\dest1";
            protected string destDir2 = @".\dest2";
            protected string tagId = "chocolatey";

            public override void Context()
            {
                RemoveAndCreateDirectory(sourceDir1);
                RemoveAndCreateDirectory(sourceDir2);
                RemoveAndCreateDirectory(destDir1);
                RemoveAndCreateDirectory(destDir2);

                var defaultPackageSource = new PackageSource(NuGetConstants.DefaultFeedUrl);
                IPackageSourceProvider sourceProvider = new PackageSourceProvider(Settings.UserSettings, new[] { defaultPackageSource });
                IPackageRepositoryFactory repositoryFactory = new NuGet.Common.CommandLineRepositoryFactory();

                command = new CopySearch(repositoryFactory, sourceProvider);
                command.Console = new Console();
            }

            protected void RemoveAndCreateDirectory(string directory)
            {
                if (Directory.Exists(directory)) { Directory.Delete(directory,true); }
                Directory.CreateDirectory(directory);
            }

            protected void CopyPackageToSource(string packagePath, string directory)
            {
                if (string.IsNullOrWhiteSpace(packagePath))
                {
                    throw new ApplicationException(string.Format("'{0}' doesn't exist.", packagePath));
                }
                string destination = Path.Combine(directory, Path.GetFileName(packagePath));
                if (!File.Exists(destination)) { File.Copy(packagePath, destination); }
            }
        }

        [Category("integration")]
        public class when_copying_tags_from_one_local_source_to_one_local_destination : CopyTagsSpecsBase
        {
            public override void Context()
            {
                base.Context();

                CopyPackageToSource(@".\testpackages\grepwin.1.5.1.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\lockhunter.1.0.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\lockhunter.1.0.2.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\MyPackageasdfa.1.0.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\roundhouse.refreshdatabase.0.8.0.301.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\roundhouse.refreshdatabase.0.8.0.304.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\stexbar.1.8.2.nupkg", sourceDir1);

                command.Arguments.Add(tagId);
                command.Source.Add(sourceDir1);
                command.Destination.Add(destDir1);
            }

            public override void Because()
            {
                command.Execute();
            }

            [Fact]
            public void should_run_successfully()
            {

            }
        }

        [Category("integration")]
        public class when_copying_tags_from_two_local_sources_to_two_local_destinations : CopyTagsSpecsBase
        {
            public override void Context()
            {
                base.Context();

                CopyPackageToSource(@".\testpackages\grepwin.1.5.1.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\lockhunter.1.0.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\lockhunter.1.0.2.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\MyPackageasdfa.1.0.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\roundhouse.refreshdatabase.0.8.0.301.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\roundhouse.refreshdatabase.0.8.0.304.nupkg", sourceDir1);
                CopyPackageToSource(@".\testpackages\stexbar.1.8.2.nupkg", sourceDir1);

                command.Arguments.Add(tagId);
                command.Source.Add(sourceDir1);
                command.Source.Add(sourceDir2);
                command.Destination.Add(destDir1);
                command.Destination.Add(destDir2);
            }

            public override void Because()
            {
                command.Execute();
            }

            [Fact]
            public void should_run_successfully()
            {
            }
        }
    }
}