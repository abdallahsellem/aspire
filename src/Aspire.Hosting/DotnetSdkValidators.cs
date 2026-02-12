// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Utils;

namespace Aspire.Hosting;
#pragma warning disable ASPIRECOMMAND001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
internal static class DotnetSdkValidators
{
    /// <summary>
    /// Creates a validation callback that ensures the detected dotnet SDK is at least major version 10.
    /// If <paramref name="projectDirectory"/> is supplied the SDK version will be resolved from that directory,
    /// otherwise the current working directory is used.
    /// </summary>
    public static Func<RequiredCommandValidationContext, Task<RequiredCommandValidationResult>> CreateDotnet10ValidationCallback(string? projectDirectory)
    {
        return async context =>
        {
            var dir = projectDirectory ?? Directory.GetCurrentDirectory();
            var version = await DotnetSdkUtils.TryGetVersionAsync(dir).ConfigureAwait(false);

            if (version is null)
            {
                // If we cannot determine the SDK version treat as success so the runtime can report the actual error.
                return RequiredCommandValidationResult.Success();
            }

            if (version.Major < 10)
            {
                var pathValue = projectDirectory is null ? string.Empty : $" in '{projectDirectory}'";
                return RequiredCommandValidationResult.Failure($"Dotnet SDK 10 or later is required{pathValue}. Detected version: {version}.");
            }

            return RequiredCommandValidationResult.Success();
        };
    }
}
#pragma warning restore ASPIRECOMMAND001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.