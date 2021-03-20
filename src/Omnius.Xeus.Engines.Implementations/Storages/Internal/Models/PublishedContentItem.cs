using Omnius.Core.Collections;
using Omnius.Core.Cryptography;

namespace Omnius.Xeus.Engines.Storages.Internal.Models
{
    internal record PublishedContentItem
    {
        public PublishedContentItem(OmniHash contentHash, string? filePath, string registrant, MerkleTreeSection[] merkleTreeSections)
        {
            this.ContentHash = contentHash;
            this.FilePath = filePath;
            this.Registrant = registrant;
            this.MerkleTreeSections = new ReadOnlyListSlim<MerkleTreeSection>(merkleTreeSections);
        }

        public OmniHash ContentHash { get; }

        public string? FilePath { get; }

        public string Registrant { get; }

        public ReadOnlyListSlim<MerkleTreeSection> MerkleTreeSections { get; }
    }
}
