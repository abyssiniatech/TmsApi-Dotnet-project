/// <summary>
/// Configuration options for payment processing settings
/// </summary>
public class PaymentOptions
{
    public string? ApiKey { get; set; }
    public string? MerchantId { get; set; }
    public decimal TransactionFee { get; set; }
    public bool EnableProcessing { get; set; }
}
