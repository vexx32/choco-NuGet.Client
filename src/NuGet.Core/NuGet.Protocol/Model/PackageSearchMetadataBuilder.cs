// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.Protocol.Core.Types
{
    //////////////////////////////////////////////////////////
    // Start - Chocolatey Specific Modification
    //////////////////////////////////////////////////////////

    /// <summary>
    /// Helper class allowing creation/alteration of immutable package metadata objects.
    /// </summary>
    public partial class PackageSearchMetadataBuilder

    //////////////////////////////////////////////////////////
    // End - Chocolatey Specific Modification
    //////////////////////////////////////////////////////////

    {
        private readonly IPackageSearchMetadata _metadata;
        private AsyncLazy<IEnumerable<VersionInfo>> _lazyVersionsFactory;
        private AsyncLazy<PackageDeprecationMetadata> _lazyDeprecationFactory;

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        public partial class ClonedPackageSearchMetadata : IPackageSearchMetadata

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        {
            private static readonly AsyncLazy<IEnumerable<VersionInfo>> LazyEmptyVersionInfo =
                AsyncLazy.New(Enumerable.Empty<VersionInfo>());

            private static readonly AsyncLazy<PackageDeprecationMetadata> LazyNullDeprecationMetadata =
                AsyncLazy.New((PackageDeprecationMetadata)null);

            public string Authors { get; set; }
            public IEnumerable<PackageDependencyGroup> DependencySets { get; set; }
            public string Description { get; set; }
            public long? DownloadCount { get; set; }
            public Uri IconUrl { get; set; }
            public PackageIdentity Identity { get; set; }
            public Uri LicenseUrl { get; set; }
            public string Owners { get; set; }
            public Uri ProjectUrl { get; set; }
            public DateTimeOffset? Published { get; set; }
            public Uri ReadmeUrl { get; set; }
            public Uri ReportAbuseUrl { get; set; }
            public Uri PackageDetailsUrl { get; set; }
            public bool RequireLicenseAcceptance { get; set; }
            public string Summary { get; set; }
            public string Tags { get; set; }
            public string Title { get; set; }
            public bool PrefixReserved { get; set; }
            public LicenseMetadata LicenseMetadata { get; set; }

            internal AsyncLazy<IEnumerable<VersionInfo>> LazyVersionsFactory { get; set; }
            public async Task<IEnumerable<VersionInfo>> GetVersionsAsync() => await (LazyVersionsFactory ?? LazyEmptyVersionInfo);

            internal AsyncLazy<PackageDeprecationMetadata> LazyDeprecationFactory { get; set; }
            public async Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync() => await (LazyDeprecationFactory ?? LazyNullDeprecationMetadata);
            public IEnumerable<PackageVulnerabilityMetadata> Vulnerabilities { get; set; }
            public bool IsListed { get; set; }
            [Obsolete("PackagePath is recommended in place of PackageReader")]
            public Func<PackageReaderBase> PackageReader { get; set; }
            public string PackagePath { get; set; }
        }

        private PackageSearchMetadataBuilder(IPackageSearchMetadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
            _metadata = metadata;
        }

        public PackageSearchMetadataBuilder WithVersions(AsyncLazy<IEnumerable<VersionInfo>> lazyVersionsFactory)
        {
            _lazyVersionsFactory = lazyVersionsFactory;
            return this;
        }

        public PackageSearchMetadataBuilder WithDeprecation(AsyncLazy<PackageDeprecationMetadata> lazyDeprecationFactory)
        {
            _lazyDeprecationFactory = lazyDeprecationFactory;
            return this;
        }

        public IPackageSearchMetadata Build()
        {
            var clonedMetadata = new ClonedPackageSearchMetadata
            {
                Authors = _metadata.Authors,
                DependencySets = _metadata.DependencySets ?? Enumerable.Empty<PackageDependencyGroup>(),
                Description = _metadata.Description,
                DownloadCount = _metadata.DownloadCount,
                IconUrl = _metadata.IconUrl,
                Identity = _metadata.Identity,
                LicenseUrl = _metadata.LicenseUrl,
                Owners = _metadata.Owners,
                ProjectUrl = _metadata.ProjectUrl,
                Published = _metadata.Published,
                ReadmeUrl = _metadata.ReadmeUrl,
                ReportAbuseUrl = _metadata.ReportAbuseUrl,
                PackageDetailsUrl = _metadata.PackageDetailsUrl,
                RequireLicenseAcceptance = _metadata.RequireLicenseAcceptance,
                Summary = _metadata.Summary,
                Tags = _metadata.Tags,
                Title = _metadata.Title,
                LazyVersionsFactory = _lazyVersionsFactory,
                IsListed = _metadata.IsListed,
                PrefixReserved = _metadata.PrefixReserved,
                LicenseMetadata = _metadata.LicenseMetadata,
                LazyDeprecationFactory = _lazyDeprecationFactory ?? AsyncLazy.New(_metadata.GetDeprecationMetadataAsync),
                Vulnerabilities = _metadata.Vulnerabilities,
#pragma warning disable CS0618 // Type or member is obsolete
                PackageReader =
                    (_metadata as LocalPackageSearchMetadata)?.PackageReader ??
                    (_metadata as ClonedPackageSearchMetadata)?.PackageReader,
#pragma warning restore CS0618 // Type or member is obsolete
                PackagePath =
                    (_metadata as LocalPackageSearchMetadata)?.PackagePath ??
                    (_metadata as ClonedPackageSearchMetadata)?.PackagePath,

                //////////////////////////////////////////////////////////
                // Start - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////

                PackageHash = _metadata.PackageHash,
                PackageHashAlgorithm = _metadata.PackageHashAlgorithm,
                PackageSize = _metadata.PackageSize,
                VersionDownloadCount = _metadata.VersionDownloadCount,
                IsApproved = _metadata.IsApproved,
                PackageStatus = _metadata.PackageStatus,
                PackageSubmittedStatus = _metadata.PackageSubmittedStatus,
                PackageTestResultStatus = _metadata.PackageTestResultStatus,
                PackageTestResultStatusDate = _metadata.PackageTestResultStatusDate,
                PackageValidationResultStatus = _metadata.PackageValidationResultStatus,
                PackageValidationResultDate = _metadata.PackageValidationResultDate,
                PackageCleanupResultDate = _metadata.PackageCleanupResultDate,
                PackageReviewedDate = _metadata.PackageReviewedDate,
                PackageApprovedDate = _metadata.PackageApprovedDate,
                PackageReviewer = _metadata.PackageReviewer,
                IsDownloadCacheAvailable = _metadata.IsDownloadCacheAvailable,
                DownloadCacheDate = _metadata.DownloadCacheDate,
                ReleaseNotes = _metadata.ReleaseNotes,
                ProjectSourceUrl = _metadata.ProjectSourceUrl,
                PackageSourceUrl = _metadata.PackageSourceUrl,
                DocsUrl = _metadata.DocsUrl,
                MailingListUrl = _metadata.MailingListUrl,
                BugTrackerUrl = _metadata.BugTrackerUrl,
                DownloadCacheStatus = _metadata.DownloadCacheStatus,
                PackageScanStatus = _metadata.PackageScanStatus,
                PackageScanResultDate = _metadata.PackageScanResultDate,
                PackageScanFlagResult = _metadata.PackageScanFlagResult,

                //////////////////////////////////////////////////////////
                // End - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////
            };

            if (_metadata is PackageSearchMetadataV2Feed)
            {
                var metaDataV2 = (PackageSearchMetadataV2Feed)_metadata;
                clonedMetadata.IsLatestVersion = metaDataV2.IsLatestVersion;
                clonedMetadata.IsAbsoluteLatestVersion = metaDataV2.IsAbsoluteLatestVersion;
                clonedMetadata.IsPrerelease = metaDataV2.IsPrerelease;
            }

            return clonedMetadata;
        }

        public static PackageSearchMetadataBuilder FromMetadata(IPackageSearchMetadata metadata)
            => new PackageSearchMetadataBuilder(metadata);

        public static PackageSearchMetadataBuilder FromIdentity(PackageIdentity identity)
        {
            var metadata = new ClonedPackageSearchMetadata
            {
                Identity = identity,
                Title = identity.Id,
                Summary = string.Empty,
                Authors = string.Empty
            };
            return FromMetadata(metadata);
        }
    }

    /// <summary>
    /// Shortcut methods to create altered metadata objects with new versions.
    /// </summary>
    public static class PackageSearchMetadataExtensions
    {
        public static IPackageSearchMetadata WithVersions(this IPackageSearchMetadata metadata, IEnumerable<VersionInfo> versions)
        {
            return PackageSearchMetadataBuilder
                .FromMetadata(metadata)
                .WithVersions(AsyncLazy.New(versions))
                .Build();
        }

        public static IPackageSearchMetadata WithVersions(this IPackageSearchMetadata metadata, Func<Task<IEnumerable<VersionInfo>>> asyncValueFactory)
        {
            return PackageSearchMetadataBuilder
                .FromMetadata(metadata)
                .WithVersions(AsyncLazy.New(asyncValueFactory))
                .Build();
        }

        public static IPackageSearchMetadata WithVersions(this IPackageSearchMetadata metadata, Func<IEnumerable<VersionInfo>> valueFactory)
        {
            return PackageSearchMetadataBuilder
                .FromMetadata(metadata)
                .WithVersions(AsyncLazy.New(valueFactory))
                .Build();
        }
    }
}
