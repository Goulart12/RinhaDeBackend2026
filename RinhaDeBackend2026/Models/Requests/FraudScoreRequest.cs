namespace RinhaDeBackend2026.Models.Requests;

public class FraudScoreRequest
{
    public string Id { get; init; } = null!;

    public TransactionDto Transaction { get; init; } = null!;

    public CustomerDto Customer { get; init; } = null!;

    public MerchantDto Merchant { get; init; } = null!;

    public TerminalDto Terminal { get; init; } = null!;

    public LastTransactionDto? LastTransaction { get; set; }
}

public sealed class TransactionDto
{
    public decimal Amount { get; init; }

    public int Installments { get; init; }

    public DateTime RequestedAt { get; init; }
}

public sealed class CustomerDto
{
    public decimal AvgAmount { get; set; }

    public int TxCount24h { get; init; }

    public IReadOnlyList<string> KnownMerchants { get; set; }
        = [];
}

public sealed class MerchantDto
{
    public string Id { get; set; } = null!;

    public string Mcc { get; set; } = null!;

    public decimal AvgAmount { get; init; }
}

public sealed class TerminalDto
{
    public bool IsOnline { get; init; }

    public bool CardPresent { get; init; }

    public decimal KmFromHome { get; init; }
}

public sealed class LastTransactionDto
{
    public DateTime RequestedAt { get; init; }

    public decimal KmFromCurrent { get; init; }
}