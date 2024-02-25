using System.IO.Compression;

namespace GenApp.Domain.Interfaces;
public interface IArchiveGenService
{
    MemoryStream MemoryStream { get; }

    ZipArchive CreateArchive();

    void ResetPosition();
}
