using RinhaDeBackend2026.Models;
using RinhaDeBackend2026.Models.Dataset;
using RinhaDeBackend2026.Models.Vectors;

namespace RinhaDeBackend2026.Services.Search;

public class KnnSearchService : IKnnSearchService
{
    private readonly ReferenceVector[] _dataset;

    public KnnSearchService(ReferenceVector[] dataset)
    {
        _dataset = dataset;
    }

    public SearchResult[] Search(
        FeatureVector query,
        int k = 5)
    {
        var top = new List<SearchResult>(k);

        foreach (var candidate in _dataset)
        {
            var distance =
                Distance(
                    query,
                    candidate.Values);

            top.Add(
                new SearchResult(
                    candidate,
                    distance));

            top.Sort(
                static (a, b) =>
                    a.Distance.CompareTo(b.Distance));

            if (top.Count > k)
            {
                top.RemoveAt(k);
            }
        }

        return top.ToArray();
    }
    
    private static float Distance(
        FeatureVector query,
        float[] candidate)
    {
        var values = query.ToArray();

        float distance = 0;

        for (var i = 0; i < 14; i++)
        {
            var diff = values[i] - candidate[i];
            distance += diff * diff;
        }

        return distance;
    }
}