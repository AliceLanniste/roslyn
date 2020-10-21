﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Remote;

namespace Microsoft.CodeAnalysis.ProjectTelemetry
{
    /// <summary>
    /// Interface to allow host (VS) to inform the OOP service to start incrementally analyzing and
    /// reporting results back to the host.
    /// </summary>
    internal interface IRemoteProjectTelemetryService
    {
        internal interface ICallback
        {
            ValueTask ReportProjectTelemetryDataAsync(RemoteServiceCallbackId callbackId, ProjectTelemetryData data, CancellationToken cancellationToken);
        }

        ValueTask ComputeProjectTelemetryAsync(RemoteServiceCallbackId callbackId, CancellationToken cancellation);
    }

    internal sealed class RemoteProjectTelemetryServiceCallbackDispatcher : RemoteServiceCallbackDispatcher, IRemoteProjectTelemetryService.ICallback
    {
        private IProjectTelemetryListener GetLogService(RemoteServiceCallbackId callbackId)
            => (IProjectTelemetryListener)GetCallback(callbackId);

        public ValueTask ReportProjectTelemetryDataAsync(RemoteServiceCallbackId callbackId, ProjectTelemetryData data, CancellationToken cancellationToken)
            => GetLogService(callbackId).ReportProjectTelemetryDataAsync(data, cancellationToken);
    }
}
