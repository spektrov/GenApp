using System.IO.Compression;
using GenApp.Domain.Interfaces;

namespace GenApp.DomainServices.Services;
internal class ArchiveGenService : IArchiveGenService, IDisposable
{
    private readonly MemoryStream _memoryStream = new();
    private ZipArchive _archive;
    private bool _disposed = false;

    public MemoryStream MemoryStream => _memoryStream;

    public ZipArchive CreateArchive()
    {
        _archive = new ZipArchive(_memoryStream, ZipArchiveMode.Create, true);
        return _archive;
    }

    public void ResetPosition()
    {
        _memoryStream.Seek(0, SeekOrigin.Begin);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed) return;

        if (disposing)
        {
            _memoryStream.Dispose();
            _archive.Dispose();
        }

        _disposed = true;
    }
}
