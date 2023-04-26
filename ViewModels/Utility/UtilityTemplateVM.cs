using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using XCoreAssignment.Models;

namespace XCoreAssignment.ViewModels.Utility
{
    public class UtilityTemplateVM
    {

        public UtilityTemplateVM()
        {

        }

        public UtilityTemplateVM(PaymentInfo paymentInfo)
        {
            CreditorName = paymentInfo.CreditorName;
            CreditorAddressLine = paymentInfo.CreditorAddressLine;
            CreditorCity = paymentInfo.CreditorCity;
            PaymentDescription = paymentInfo.PaymentDescription;
            PurposeCode = paymentInfo.PurposeCode;
            Amount = paymentInfo.Amount;
            CurrencyCode = paymentInfo.CurrencyCode;
            CreditorAccount = paymentInfo.CreditorAccount;
            CreditorReference = paymentInfo.CreditorReference;
            CreditorReferenceModel = paymentInfo.CreditorReferenceModel;
        }

        public string? ErrorMessage { get; set; }

        [Display(Name = "Account")]
        public int? DebtorAccountId { get; set; }
        public List<AccountDTO> DebtorAccounts { get; } = DummyData.Accounts.DummyAccounts;


        [Display(Name = "Name")]
        public string DebtorName { get; } = "John Do";

        [Display(Name = "Address line")]
        public string DebtorAddressLine { get; } = "Joke Street 69";

        [Display(Name = "City")]
        public string DebtorCity { get; } = "10009 Joke City";



        [Display(Name = "Amount")]
        [Required]
        public string? Amount { get; set; }



        [Display(Name = "Name")]
        public string? CreditorName { get; set; }

        [Display(Name = "Address line")]
        public string? CreditorAddressLine { get; set; }

        [Display(Name = "City")]
        public string? CreditorCity { get; set; }

        [Display(Name = "Payment description")]
        public string? PaymentDescription { get; set; }

        [Display(Name = "Purpose code")]
        public string? PurposeCode { get; set; }

        [Display(Name = "Currency code")]
        public string? CurrencyCode { get; set; }

        [Display(Name = "Account")]
        public string? CreditorAccount { get; set; }

        [Display(Name = "Reference")]
        public string? CreditorReference { get; set; }

        [Display(Name = "Reference model")]
        public string? CreditorReferenceModel { get; set; }
    }
}
