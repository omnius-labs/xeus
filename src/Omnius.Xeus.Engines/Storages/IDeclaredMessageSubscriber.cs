using System;
using System.Threading;
using System.Threading.Tasks;
using Omnius.Core;
using Omnius.Core.Cryptography;
using Omnius.Core.Storages;
using Omnius.Xeus.Engines.Models;
using Omnius.Xeus.Engines.Storages.Primitives;

namespace Omnius.Xeus.Engines.Storages
{
    public interface IDeclaredMessageSubscriberFactory
    {
        ValueTask<IDeclaredMessageSubscriber> CreateAsync(DeclaredMessageSubscriberOptions options, IBytesStorageFactory bytesStorageFactory, IBytesPool bytesPool, CancellationToken cancellationToken = default);
    }

    public interface IDeclaredMessageSubscriber : IWritableDeclaredMessages, IAsyncDisposable
    {
        ValueTask<DeclaredMessageSubscriberReport> GetReportAsync(CancellationToken cancellationToken = default);

        ValueTask SubscribeMessageAsync(OmniSignature signature, string registrant, CancellationToken cancellationToken = default);

        ValueTask UnsubscribeMessageAsync(OmniSignature signature, string registrant, CancellationToken cancellationToken = default);
    }
}
