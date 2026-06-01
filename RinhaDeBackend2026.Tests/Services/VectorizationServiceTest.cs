using RinhaDeBackend2026.Models.Dataset;
using RinhaDeBackend2026.Models.Requests;
using RinhaDeBackend2026.Services.Vectorization;

namespace RinhaDeBackend2026.Tests.Services;

public class VectorizationServiceEdgeCasesTests
{
    private readonly VectorizationService _service;

    public VectorizationServiceEdgeCasesTests()
    {
        var settings = new NormalizationSettings
        {
            MaxAmount = 10000,
            MaxInstallments = 12,
            AmountVsAvgRatio = 10,
            MaxMinutes = 1440,
            MaxKm = 1000,
            MaxTxCount24h = 20,
            MaxMerchantAvgAmount = 10000
        };

        var mccRisk = new Dictionary<string, float>
        {
            ["5411"] = 0.15f
        };

        _service = new VectorizationService(
            settings,
            mccRisk);
    }

    [Fact]
    public void Should_Set_Unknown_Merchant_To_One()
    {
        // Arrange
        var request = CreateRequest();

        request.Customer.KnownMerchants =
        [
            "OTHER-MERCHANT"
        ];

        request.Merchant.Id = "NEW-MERCHANT";

        // Act
        var vector = _service.CreateVector(request);

        // Assert
        Assert.Equal(1f, vector.UnknownMerchant);
    }

    [Fact]
    public void Should_Use_Default_Mcc_Risk_When_Mcc_Not_Found()
    {
        // Arrange
        var request = CreateRequest();

        request.Merchant.Mcc = "999999";

        // Act
        var vector = _service.CreateVector(request);

        // Assert
        Assert.Equal(0.5f, vector.MccRisk);
    }

    [Fact]
    public void Should_Return_Minus_One_When_LastTransaction_Is_Null()
    {
        // Arrange
        var request = CreateRequest();

        request.LastTransaction = null;

        // Act
        var vector = _service.CreateVector(request);

        // Assert
        Assert.Equal(-1f, vector.MinutesSinceLastTx);
        Assert.Equal(-1f, vector.KmFromLastTx);
    }

    [Fact]
    public void Should_Handle_Zero_Average_Amount()
    {
        // Arrange
        var request = CreateRequest();

        request.Customer.AvgAmount = 0;

        // Act
        var vector = _service.CreateVector(request);

        // Assert
        Assert.Equal(1f, vector.AmountVsAvg);
    }

    private static FraudScoreRequest CreateRequest()
    {
        return new FraudScoreRequest
        {
            Id = "tx-1",

            Transaction = new TransactionDto
            {
                Amount = 100,
                Installments = 1,
                RequestedAt = DateTime.UtcNow
            },

            Customer = new CustomerDto
            {
                AvgAmount = 100,
                TxCount24h = 1,
                KnownMerchants =
                [
                    "MERC-001"
                ]
            },

            Merchant = new MerchantDto
            {
                Id = "MERC-001",
                Mcc = "5411",
                AvgAmount = 100
            },

            Terminal = new TerminalDto
            {
                IsOnline = false,
                CardPresent = true,
                KmFromHome = 10
            },

            LastTransaction = new LastTransactionDto
            {
                RequestedAt = DateTime.UtcNow.AddMinutes(-10),
                KmFromCurrent = 5
            }
        };
    }
}