using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Omnius.Core;
using Omnius.Core.Cryptography;
using Omnius.Xeus.Service.Connectors;

namespace Omnius.Xeus.Service.Engines
{
    public interface INodeFinderFactory
    {
        ValueTask<INodeFinder> CreateAsync(NodeFinderOptions options, IEnumerable<IConnector> connectors,
            IEnumerable<IPublishStorage> publishStorages, IEnumerable<IWantStorage> wantStorages,
            IBytesPool bytesPool);
    }

    public interface INodeFinder
    {
        ValueTask<NodeProfile> GetMyNodeProfile(CancellationToken cancellationToken = default);
        ValueTask AddCloudNodeProfiles(IEnumerable<NodeProfile> nodeProfiles, CancellationToken cancellationToken = default);
        ValueTask<NodeProfile[]> FindNodeProfiles(ResourceTag tag, CancellationToken cancellationToken = default);
    }
}