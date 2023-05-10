namespace Domain.Entities;

public class PaymentInfo
{
    public string DebtorName { get; set; }
    public string DebtorAddressLine { get; set; }
    public string DebtorCity { get; set; }
    public string CreditorName { get; set; }
    public string CreditorAddressLine { get; set; }
    public string CreditorCity { get; set; }
    public string PaymentDescription { get; set; }
    public string PurposeCode { get; set; }
    public string Amount { get; set; }
    public string CurrencyCode { get; set; }
    public string DebtorAccount { get; set; }
    public string CreditorAccount { get; set; }
    public string CreditorReference { get; set; }
    public string CreditorReferenceModel { get; set; }
}
