using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Globalization;
using XCoreAssignment.Helpers;
using XCoreAssignment.Models;
using XCoreAssignment.ViewModels.Utility;
using ZXing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XCoreAssignment.Controllers
{
    public class QRController : Controller
    {

        public static PaymentInfo ParsePaymentInfo(string input)
        {
            var splittedByLine = input.Split("\r\n");

            if (splittedByLine.Length != 5)
                throw new Exception("Invalid number of lines");

            PaymentInfo paymentInfo = new();

            string line1 = splittedByLine[0];
            string line2 = splittedByLine[1];
            string line3 = splittedByLine[2];
            string line4 = splittedByLine[3];
            string line5 = splittedByLine[4];

            AssignValuesFromLine1(paymentInfo, line1);
            AssignValuesFromLine2(paymentInfo, line2);
            AssignValuesFromLine3(paymentInfo, line3);
            AssignValuesFromLine4(paymentInfo, line4);
            AssignValuesFromLine5(paymentInfo, line5);

            return paymentInfo;
        }

        private static void AssignValuesFromLine5(PaymentInfo paymentInfo, string line5)
        {
            var line5Splitted = line5.Split('|');
            if (line5Splitted.Length != 4)
                throw new Exception("Invalid format of fifth line");

            var debtorCity = GetDebtorCity(line5Splitted);
            var purposeCode = GetPurposeCode(line5Splitted);
            var paymentDescription = GetPaymentDescription(line5Splitted);
            var creditorReferenceAndModel = GetCreditorReferenceAndModel(line5Splitted);
            //
            paymentInfo.DebtorCity = debtorCity;
            paymentInfo.PurposeCode = purposeCode;
            paymentInfo.PaymentDescription = paymentDescription;
            paymentInfo.CreditorReference = creditorReferenceAndModel.CreditorReference;
            paymentInfo.CreditorReferenceModel = creditorReferenceAndModel.CreditorReferenceModel;
        }

        private static CreditorRefenceAndModelDTO GetCreditorReferenceAndModel(string[] line5Splitted)
        {
            var creditorReferenceAndModelStr = line5Splitted[3];
            var splitted = creditorReferenceAndModelStr.Split("RO:");
            if (splitted.Length != 2)
                throw new Exception("Invalid format for creditor refence and model");

            //assuming that the first 2 digits of "RO" are credit refence model and 
            //assmming that credit reference can have number that does not have to have 15 digits.
            //meaning that credit reference can have one digit (not very likely but it can easily be changed)
            if (splitted[1].Length < 3)
                throw new Exception("Invalid format for creditor refence and model. Length is less than 3");

            var creditorReferenceModelInText = splitted[1].Substring(0, 2);
            var isAllDigitsInCreditorRefenceModel = creditorReferenceModelInText.Length == creditorReferenceModelInText.Where(x => char.IsDigit(x)).Count();
            if (!isAllDigitsInCreditorRefenceModel)
                throw new Exception("Invalid value for creditor refence model. All chars have to be digits");

            var creditorReferenceInText = splitted[1].Substring(2);
            var isAllDigitsInCreditorReference = creditorReferenceInText.Length == creditorReferenceInText.Where(x => char.IsDigit(x)).Count();
            if (!isAllDigitsInCreditorReference)
                throw new Exception("Invalid value for creditor refence. All chars have to be digits");

            return new CreditorRefenceAndModelDTO(creditorReferenceInText,creditorReferenceModelInText);
        }

        private static string GetPaymentDescription(string[] line5Splitted)
        {
            var paymentDescriptionWithMark = line5Splitted[2];
            var splitted = paymentDescriptionWithMark.Split("S:");

            if (splitted.Length != 2)
                throw new Exception("Invalid format for payment description");

            var paymentDescription = splitted[1];

            if (string.IsNullOrWhiteSpace(paymentDescription))
                throw new Exception("Invalid value for payment description. Value cannot be empty.");

            return paymentDescription;
        }

        private static string GetPurposeCode(string[] line5Splitted)
        {
            var purposeCodeWithMark = line5Splitted[1];
            var splitted = purposeCodeWithMark.Split("SF:");

            if (splitted.Length != 2) throw new Exception("Invalid format for purpose code");

            var purpseCodeText = splitted[1];

            var isAllCharDigit = purpseCodeText.Length == purpseCodeText.Where(x => char.IsDigit(x)).Count();
            if (!isAllCharDigit)
                throw new Exception("Invalid value for purpose code. Not all chars are digits.");

            return purpseCodeText;
        }

        private static string GetDebtorCity(string[] line5Splitted)
        {
            var debtorCity = line5Splitted[0];

            if (string.IsNullOrWhiteSpace(debtorCity))
                throw new Exception("Invalid value for debtor city. Debtor city cannot be null");
            return debtorCity;
        }

        private static void AssignValuesFromLine4(PaymentInfo paymentInfo, string line4)
        {
            var debtorAddressLine = line4;

            if (string.IsNullOrWhiteSpace(debtorAddressLine))
                throw new Exception("Invalid value for debtor address line. Address line for debtor cannot be null");

            paymentInfo.DebtorAddressLine = debtorAddressLine;
        }

        private static void AssignValuesFromLine3(PaymentInfo paymentInfo, string line3)
        {
            var line3Splitted = line3.Split('|');
            if (line3Splitted.Length != 3)
                throw new Exception("Invalid format of third line");

            var creditorCity = GetCreditorCity(line3Splitted);
            var currentyCodeAndAmount = GetCurrenyCodeAndAmount(line3Splitted);
            var debtorName = GetDebtorName(line3Splitted);

            paymentInfo.CreditorCity = creditorCity;
            paymentInfo.CurrencyCode = currentyCodeAndAmount.CurrencyCode;
            paymentInfo.Amount = currentyCodeAndAmount.Amount;
            paymentInfo.DebtorName = debtorName;
        }

        private static string GetDebtorName(string[] line3Splitted)
        {
            var debtorNameWithMark = line3Splitted[2];

            var debtorNameSplitted = debtorNameWithMark.Split("P:");
            if (debtorNameSplitted.Length != 2)
                throw new Exception("Invalid debtor name format");

            var debtorName = debtorNameSplitted[1];
            if (string.IsNullOrWhiteSpace(debtorName))
                throw new Exception("Invalid value for debtor name. Value cannot be empty");

            return debtorName;
        }

        private static CurrencyAmountDTO GetCurrenyCodeAndAmount(string[] line3Splitted)
        {
            var currenyAndAmountStr = line3Splitted[1];
            var splitted = currenyAndAmountStr.Split("I:");
            if (splitted.Length != 2)
                throw new Exception("Invalid format for curreny and amount");

            //less than 4 because curreny code is 3 chars and amount should be at least 1 char
            //we can say that amount should be at least 3 chars as well. For example: 1.00
            //but let's asume that can expect value 1 instead of 1.00
            if (splitted[1].Length < 4)
                throw new Exception("Invalid format for curreny and amount. Curreny code or amount is missing");

            var currenyCodeInText = splitted[1].Substring(0, 3);
            var currencyCode = CurrencyHelper.LookupByCode(currenyCodeInText);
            if (currencyCode == null)
                throw new Exception("Invalid currency code.");

            var amountInText = splitted[1].Substring(3);
            if (!double.TryParse(amountInText, NumberStyles.Currency, CultureInfo.InvariantCulture, out var amount))
                throw new ArgumentException("Invalid currency amount");

            string amountFormated = string.Format("{0:n2}", amount);

            return new CurrencyAmountDTO(currencyCode.Number.ToString(), amountFormated);

        }



        private static string GetCreditorCity(string[] line3Splitted)
        {
            var creditorCity = line3Splitted[0];

            if (string.IsNullOrWhiteSpace(creditorCity))
                throw new Exception("Invalid value for creditor city. Value cannot be empty.");

            return creditorCity;
        }

        private static void AssignValuesFromLine2(PaymentInfo paymentInfo, string line2)
        {
            var creditorAddressLine = line2;

            if (string.IsNullOrWhiteSpace(creditorAddressLine))
                throw new Exception("Invalid value in line 2. Creditor address line cannot be empty");

            paymentInfo.CreditorAddressLine = creditorAddressLine;
        }

        private static void AssignValuesFromLine1(PaymentInfo paymentInfo, string line1)
        {
            var line1Splitted = line1.Split('|');
            if (line1Splitted.Length != 5)
                throw new Exception("Invalid format of first line");

            var creditorAccount = GetCreditorAcc(line1Splitted);
            var creditorName = GetCreditorName(line1Splitted);

            paymentInfo.CreditorAccount = creditorAccount;
            paymentInfo.CreditorName = creditorName;
        }

        private static string GetCreditorAddressLine(string[] line1Splitted)
        {
            var creditorAddressLineWithSymbol = line1Splitted[4];
            var creditorAddressLineSplited = creditorAddressLineWithSymbol.Split("N:");

            if (creditorAddressLineSplited.Length != 2)
                throw new Exception("Invalid format of creditor address line");

            var creditorAddressLine = creditorAddressLineSplited[1];
            return creditorAddressLine;
        }

        private static string GetCreditorName(string[] line1Splitted)
        {
            var creditorNameWithSymbol = line1Splitted[4];
            var creditorNameSplited = creditorNameWithSymbol.Split("N:");

            if (creditorNameSplited.Length != 2)
                throw new Exception("Invalid format of creditor name");

            var creditorName = creditorNameSplited[1];
            return creditorName;
        }

        private static string GetCreditorAcc(string[] line1Splitted)
        {
            var creditorAccWithSymbol = line1Splitted[3];
            var creditorAccSplited = creditorAccWithSymbol.Split("R:");

            if (creditorAccSplited.Length != 2)
                throw new Exception("Invalid format of creditor account");

            var creditorAcc = creditorAccSplited[1];
            var isAllDigits = creditorAcc.Length == creditorAcc.Where(x => char.IsDigit(x)).Count();
            if (!isAllDigits)
                throw new Exception("Invalid format of creditor account number. Not all characters are digits.");

            //assuming that all account numbers have 16 and only 16 digits
            if (creditorAcc.Length != 16)
                throw new Exception("Invalid creditor account number. Number of digits has to be 16");

            return creditorAcc;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile FormFile)
        {
            string resultText;
            try
            {

                var barcodeReader = new BarcodeReader();
                using var stream = FormFile.OpenReadStream();
                var bitmap = (Bitmap)Image.FromStream(stream);
                var result = barcodeReader.Decode(bitmap);
                resultText = result.Text;
            }catch(Exception ex)
            {
                return View("Exception", "Error reading QR code.");
            }

            if (string.IsNullOrWhiteSpace(resultText))
                return View("Exception", "Content of QR code is empty.");

            PaymentInfo paymentInfo;
            try
            {
                paymentInfo = ParsePaymentInfo(resultText);
            }catch (Exception ex) {
                return View("Exception", ex.Message);
            }

            /*
             NOTE:
            QR Code in Assignment PDF file returns this for Creditor account:
            R:1703001370600358

            but
            in section 2.2. "Transaction Data" Creditor account has value:
             "CreditorAccount": "170003001370600358"

            there is difference:
            1. 1703......
            2. 170003....

            I assume it was a typo.
            If it wasn't, I should have manipulated the value to get the desired result.

            another difference is "John Do" and "JOHN DO"
            Maybe I should have made all letters lowercase except the first one of each word.
             */

            UtilityTemplateVM vm = new(paymentInfo);

            return View("~/Views/Utility/Template.cshtml", vm);
        }

    }
}

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
