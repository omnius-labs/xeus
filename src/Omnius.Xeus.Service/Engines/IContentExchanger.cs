using System.Collections.Generic;
using System.Threading.Tasks;
using Omnius.Core;
using Omnius.Xeus.Service.Connectors;
using Omnius.Xeus.Service.Models;

namespace Omnius.Xeus.Service.Engines
{
    public interface IContentExchangerFactory
    {
        ValueTask<IContentExchanger> CreateAsync(ContentExchangerOptions options, IEnumerable<IConnector> connectors,
            INodeFinder nodeFinder, IPublishContentStorage publishStorage, IWantContentStorage wantStorage, IBytesPool bytesPool);
    }

    public interface IContentExchanger
    {
    }
}
