using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Omnius.Core;
using Omnius.Xeus.Engines.Connectors.Primitives;
using Omnius.Xeus.Engines.Models;

namespace Omnius.Xeus.Engines.Mediators
{
    public interface ICkadMediatorFactory
    {
        ValueTask<ICkadMediator> CreateAsync(CkadMediatorOptions options, IEnumerable<IConnector> connectors, IBytesPool bytesPool);
    }

    public delegate void GetResourceTags(Action<ResourceTag> append);

    public interface ICkadMediator : IAsyncDisposable
    {
        event GetResourceTags? GetPublishResourceTags;

        event GetResourceTags? GetWantResourceTags;

        ValueTask<NodeProfile> GetMyNodeProfileAsync(CancellationToken cancellationToken = default);

        ValueTask AddCloudNodeProfilesAsync(IEnumerable<NodeProfile> nodeProfiles, CancellationToken cancellationToken = default);

        ValueTask<NodeProfile[]> FindNodeProfilesAsync(ResourceTag tag, CancellationToken cancellationToken = default);
    }
}
