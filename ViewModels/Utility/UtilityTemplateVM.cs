using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using XCoreAssignment.Helpers;
using XCoreAssignment.Models;
using XCoreAssignment.Validations;

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
        [Required]
        public int? DebtorAccountId { get; set; }

        public List<AccountDTO> DebtorAccounts  = DummyData.Accounts.DummyAccounts;


        [Display(Name = "Name")]
        [Required]
        public string DebtorName { get; } = "John Do";

        [Display(Name = "Address line")]
        [Required]
        public string DebtorAddressLine { get; } = "Joke Street 69";

        [Display(Name = "City")]
        [Required]
        public string DebtorCity { get; } = "10009 Joke City";



        [Display(Name = "Amount")]
        [Required]
        [RegularExpression(@"^\d{1,3}(,\d{3})*(\.\d+)?$", ErrorMessage = "Please enter a valid number in the format of 1,000.00")]
        public string? Amount { get; set; }



        [Display(Name = "Name")]
        [Required]
        public string? CreditorName { get; set; }

        [Display(Name = "Address line")]
        [Required]
        public string? CreditorAddressLine { get; set; }

        [Display(Name = "City")]
        [Required]
        public string? CreditorCity { get; set; }

        [Display(Name = "Payment description")]
        [Required]
        public string? PaymentDescription { get; set; }

        [Display(Name = "Purpose code")]
        [Required]
        public string? PurposeCode { get; set; }

        [Display(Name = "Currency code")]
        [Required]
        [CustomValidation(typeof(CurrencyCodeValidation), "ValidateCurrencyCode")]
        public string? CurrencyCode { get; set; }

        [Display(Name = "Account")]
        [Required]
        public string? CreditorAccount { get; set; }

        [Display(Name = "Reference")]
        [Required]
        public string? CreditorReference { get; set; }

        [Display(Name = "Reference model")]
        [Required]
        public string? CreditorReferenceModel { get; set; }
    }
}
