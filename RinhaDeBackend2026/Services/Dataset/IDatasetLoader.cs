using RinhaDeBackend2026.Models.Dataset;

namespace RinhaDeBackend2026.Services.Dataset;

public interface IDatasetLoader
{
    Task<ReferenceVector[]> LoadAsync(
        CancellationToken cancellationToken = default);
}