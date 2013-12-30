using System;

namespace Authink.Data.ResourceFileStorage
{
    public interface IFileStorageAdapter
    {
        Uri Upload(ResourceFileStorageAdapter.ResourceFile resourceFile, string baseFoldername );
        bool Remove(Uri resourceFileDownloadUrl);
    }
}
