using System.IO.Compression;
using GenApp.Domain.Models;

namespace GenApp.Domain.Interfaces;

public interface IGenCommand
{
    Task ExecuteAsync(ZipArchive archive, ApplicationDataModel model, CancellationToken token);
}
