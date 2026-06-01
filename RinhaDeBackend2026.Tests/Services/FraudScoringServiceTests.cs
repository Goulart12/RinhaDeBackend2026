using Moq;
using RinhaDeBackend2026.Models.Dataset;
using RinhaDeBackend2026.Models.Vectors;
using RinhaDeBackend2026.Services.Fraud;
using RinhaDeBackend2026.Services.Search;

namespace RinhaDeBackend2026.Tests.Services;

public class FraudScoringServiceTests
{
    [Fact]
    public void Should_Return_Zero_Percent_Fraud()
    {
        // Arrange
        var knn = CreateKnnMock(
            Result(0),
            Result(0),
            Result(0),
            Result(0),
            Result(0));

        var service = new FraudScoringService(knn.Object);

        // Act
        var response = service.Calculate(default);

        // Assert
        Assert.Equal(0f, response.FraudScore);
        Assert.True(response.Approved);
    }

    [Fact]
    public void Should_Return_Forty_Percent_Fraud()
    {
        // Arrange
        var knn = CreateKnnMock(
            Result(1),
            Result(1),
            Result(0),
            Result(0),
            Result(0));

        var service = new FraudScoringService(knn.Object);

        // Act
        var response = service.Calculate(default);

        // Assert
        Assert.Equal(0.4f, response.FraudScore);
        Assert.True(response.Approved);
    }

    [Fact]
    public void Should_Return_Sixty_Percent_Fraud()
    {
        // Arrange
        var knn = CreateKnnMock(
            Result(1),
            Result(1),
            Result(1),
            Result(0),
            Result(0));

        var service = new FraudScoringService(knn.Object);

        // Act
        var response = service.Calculate(default);

        // Assert
        Assert.Equal(0.6f, response.FraudScore);
        Assert.False(response.Approved);
    }

    private static Mock<IKnnSearchService> CreateKnnMock(
        params SearchResult[] results)
    {
        var mock = new Mock<IKnnSearchService>();

        mock.Setup(x => x.Search(
                It.IsAny<FeatureVector>(),
                It.IsAny<int>()))
            .Returns(results);

        return mock;
    }

    private static SearchResult Result(byte label)
    {
        return new SearchResult(
            new ReferenceVector(
                new float[14],
                label),
            0f);
    }
}