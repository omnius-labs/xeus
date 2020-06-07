using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Omnius.Core;
using Omnius.Core.Cryptography;
using Omnius.Core.Network;
using Omnius.Core.Network.Connections;
using Omnius.Xeus.Service.Drivers;
using Omnius.Xeus.Service.Engines.Internal;

namespace Omnius.Xeus.Service.Engines
{
    public sealed partial class BlockExchanger : AsyncDisposableBase, IBlockExchanger
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly string _configPath;
        private readonly BlockExchangerOptions _options;
        private readonly IObjectStoreFactory _objectStoreFactory;
        private readonly IConnectionController _connectionController;
        private readonly INodeExplorer _nodeExplorer;
        private readonly IPublishStorage _publishStorage;
        private readonly IWantStorage _wantStorage;
        private readonly IBytesPool _bytesPool;

        private IObjectStore _objectStore;



        private Task _connectLoopTask;
        private Task _acceptLoopTask;
        private Task _sendLoopTask;
        private Task _receiveLoopTask;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly AsyncLock _asyncLock = new AsyncLock();

        public const string ServiceName = "BlockExchanger`";

        internal sealed class BlockExchangerFactory : IBlockExchangerFactory
        {
            public async ValueTask<IBlockExchanger> CreateAsync(string configPath, BlockExchangerOptions options,
                IObjectStoreFactory objectStoreFactory, IConnectionController connectionController,
                INodeExplorer nodeExplorer, IPublishStorage publishStorage, IWantStorage wantStorage, IBytesPool bytesPool)
            {
                var result = new BlockExchanger(configPath, options, objectStoreFactory, connectionController, nodeExplorer, publishStorage, wantStorage, bytesPool);
                await result.InitAsync();

                return result;
            }
        }

        public static IBlockExchangerFactory Factory { get; } = new BlockExchangerFactory();

        internal BlockExchanger(string configPath, BlockExchangerOptions options,
                IObjectStoreFactory objectStoreFactory, IConnectionController connectionController,
                INodeExplorer nodeExplorer, IPublishStorage publishStorage, IWantStorage wantStorage, IBytesPool bytesPool)
        {
            _configPath = configPath;
            _options = options;
            _objectStoreFactory = objectStoreFactory;
            _connectionController = connectionController;
            _nodeExplorer = nodeExplorer;
            _publishStorage = publishStorage;
            _wantStorage = wantStorage;
            _bytesPool = bytesPool;
        }

        internal async ValueTask InitAsync()
        {
            _objectStore = await _objectStoreFactory.CreateAsync(_configPath, _bytesPool);

            await this.LoadAsync();

            _connectLoopTask = this.ConnectLoopAsync(_cancellationTokenSource.Token);
            _acceptLoopTask = this.AcceptLoopAsync(_cancellationTokenSource.Token);
            _sendLoopTask = this.SendLoopAsync(_cancellationTokenSource.Token);
            _receiveLoopTask = this.ReceiveLoopAsync(_cancellationTokenSource.Token);
        }

        protected override async ValueTask OnDisposeAsync()
        {

        }

        private async ValueTask LoadAsync()
        {

        }

        private async ValueTask SaveAsync()
        {

        }

        private async Task ConnectLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                for (; ; )
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException e)
            {
                _logger.Debug(e);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        private Task AcceptLoopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task SendLoopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task ReceiveLoopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private enum ConnectionHandshakeType
        {
            Connected,
            Accepted,
        }

        private sealed class ConnectionStatus : ISynchronized
        {
            public ConnectionStatus(IConnection connection, OmniAddress address,
                ConnectionHandshakeType handshakeType, NodeProfile nodeProfile, OmniHash tag)
            {
                this.Connection = connection;
                this.Address = address;
                this.HandshakeType = handshakeType;
                this.NodeProfile = nodeProfile;
                this.Tag = tag;
            }

            public object LockObject { get; } = new object();

            public IConnection Connection { get; }
            public OmniAddress Address { get; }
            public ConnectionHandshakeType HandshakeType { get; }

            public NodeProfile NodeProfile { get; }
            public OmniHash Tag { get; }

            public VolatileSet<OmniHash> ReceivedWantContentLocations { get; } = new VolatileSet<OmniHash>(TimeSpan.FromMinutes(30));

            public void Refresh()
            {
                this.ReceivedWantContentLocations.Refresh();
            }
        }
    }
}