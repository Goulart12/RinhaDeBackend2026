using RinhaDeBackend2026.Models.Dataset;

namespace RinhaDeBackend2026.Services.Search;

public readonly struct SearchResult
{
    public readonly ReferenceVector Vector;

    public readonly float Distance;

    public SearchResult(
        ReferenceVector vector,
        float distance)
    {
        Vector = vector;
        Distance = distance;
    }
}