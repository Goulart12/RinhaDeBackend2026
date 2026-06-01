using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RinhaDeBackend2026.Models.Dataset;

namespace RinhaDeBackend2026.Services.Dataset;

public class DatasetLoader : IDatasetLoader
{
    private readonly ILogger<DatasetLoader> _logger;
    private readonly DatasetOptions _options;

    public DatasetLoader(
        ILogger<DatasetLoader> logger,
        IOptions<DatasetOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<ReferenceVector[]> LoadAsync(
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_options.DatasetPath))
        {
            throw new FileNotFoundException(
                $"Dataset não encontrado: {_options.DatasetPath}");
        }

        _logger.LogInformation(
            "Iniciando carregamento do dataset: {Path}",
            _options.DatasetPath);

        var stopwatch = Stopwatch.StartNew();

        var vectors = new List<ReferenceVector>();

        await using var file =
            File.OpenRead(_options.DatasetPath);

        await using var gzip =
            new GZipStream(
                file,
                CompressionMode.Decompress);

        await foreach (var record in JsonSerializer
                           .DeserializeAsyncEnumerable<ReferenceRecord>(
                               gzip,
                               cancellationToken: cancellationToken))
        {
            if (record is null)
                continue;

            if (record.Vector.Length != 14)
            {
                throw new InvalidDataException(
                    $"Vetor inválido encontrado. Esperado: 14 dimensões. Atual: {record.Vector.Length}");
            }

            var label = record.Label.Equals(
                "fraud",
                StringComparison.OrdinalIgnoreCase)
                    ? (byte)1
                    : (byte)0;

            vectors.Add(
                new ReferenceVector(
                    record.Vector,
                    label));
        }

        stopwatch.Stop();

        _logger.LogInformation(
            """
            Dataset carregado com sucesso.
            Vetores: {Count}
            Tempo: {Elapsed}
            """,
            vectors.Count,
            stopwatch.Elapsed);

        return vectors.ToArray();
    }
}