namespace RinhaDeBackend2026.Models.Dataset;

public sealed class ReferenceRecord
{
    public required float[] Vector { get; init; }

    public required string Label { get; init; }
}