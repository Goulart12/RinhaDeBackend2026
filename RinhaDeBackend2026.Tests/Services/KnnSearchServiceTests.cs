using RinhaDeBackend2026.Models.Dataset;
using RinhaDeBackend2026.Models.Vectors;
using RinhaDeBackend2026.Services.Search;

namespace RinhaDeBackend2026.Tests.Services;

public class KnnSearchServiceTests
{
    [Fact]
    public void Should_Return_Exactly_K_Results()
    {
        // Arrange
        var dataset = Enumerable.Range(0, 10)
            .Select(i => new ReferenceVector(
                Enumerable.Repeat((float)i, 14).ToArray(),
                0))
            .ToArray();

        var service = new KnnSearchService(dataset);

        var query = new FeatureVector(
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0);

        // Act
        var result = service.Search(query, 5);

        // Assert
        Assert.Equal(5, result.Length);
    }

    [Fact]
    public void Should_Return_Ordered_By_Distance()
    {
        // Arrange
        var dataset = new[]
        {
            new ReferenceVector(
                Enumerable.Repeat(10f, 14).ToArray(),
                0),

            new ReferenceVector(
                Enumerable.Repeat(2f, 14).ToArray(),
                0),

            new ReferenceVector(
                Enumerable.Repeat(5f, 14).ToArray(),
                0)
        };

        var service = new KnnSearchService(dataset);

        var query = new FeatureVector(
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0);

        // Act
        var result = service.Search(query, 3);

        // Assert
        Assert.True(result[0].Distance <= result[1].Distance);
        Assert.True(result[1].Distance <= result[2].Distance);
    }

    [Fact]
    public void Should_Return_Closest_Vector_First()
    {
        // Arrange
        var closest = new ReferenceVector(
            new float[14],
            0);

        var farthest = new ReferenceVector(
            Enumerable.Repeat(100f, 14).ToArray(),
            1);

        var dataset = new[]
        {
            farthest,
            closest
        };

        var service = new KnnSearchService(dataset);

        var query = new FeatureVector(
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0);

        // Act
        var result = service.Search(query, 1);

        // Assert
        Assert.Equal(0f, result[0].Distance);
        Assert.Equal(0, result[0].Vector.Label);
    }
}