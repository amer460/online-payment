using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Globalization;
using ZXing;

namespace XCoreAssignment.Controllers
{
    public class TestController : Controller
    {
        public static PaymentInfo ParsePaymentInfo(string input)
        {

            var result = new PaymentInfo();
            var sections = input.Split('|').Select(x => x.Trim()).ToArray();
            foreach (var section in sections)
            {
                if (section.StartsWith("N:"))
                {
                    result.CreditorName = section.Substring(2);
                }
                else if (section.StartsWith("C:"))
                {
                    var address = section.Substring(2).Split(new[] { "\r\n" }, StringSplitOptions.None);
                    result.CreditorAddressLine = address[0];
                    result.CreditorCity = address[1];
                }
                else if (section.StartsWith("P:"))
                {
                    var name = section.Substring(2).Split(new[] { "\r\n" }, StringSplitOptions.None);
                    result.DebtorName = name[0];
                    result.DebtorAddressLine = name[1];
                    result.DebtorCity = name[2];
                }
                else if (section.StartsWith("S:"))
                {
                    result.PaymentDescription = section.Substring(2);
                }
                else if (section.StartsWith("SF:"))
                {
                    result.PurposeCode = section.Substring(3);
                }
                else if (section.StartsWith("I:"))
                {
                    var amount = section.Substring(2);
                    result.Amount = double.Parse(amount,CultureInfo.InvariantCulture);
                    result.CurrencyCode = "978"; // EUR currency code
                }
                else if (section.StartsWith("R:"))
                {
                    result.CreditorAccount = section.Substring(2);
                }
                else if (section.StartsWith("RO:"))
                {
                    result.CreditorReference = section.Substring(3);
                    result.CreditorReferenceModel = section.Substring(section.Length - 2);
                }
            }
            result.DebtorAccount = "0011225333665"; // hardcoded for example purposes
            return result;
        }
    



        [HttpGet]
        public IActionResult MainScreen()
        {
            return View();
        }

        [HttpGet]
        public IActionResult QRCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult QRCode(IFormFile FormFile)
        {
            var barcodeReader = new BarcodeReader();
            using (var stream = FormFile.OpenReadStream())
            {
                var bitmap = (Bitmap)Image.FromStream(stream);
                var result = barcodeReader.Decode(bitmap);
                var resultText = result.Text;

                var paymentInfo = ParsePaymentInfo(resultText);

                if (result != null)
                {
                    return Ok(result.Text);
                }
                else
                {
                    return BadRequest("QR code not found");
                }
            }

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
    public double Amount { get; set; }
    public string CurrencyCode { get; set; }
    public string DebtorAccount { get; set; }
    public string CreditorAccount { get; set; }
    public string CreditorReference { get; set; }
    public string CreditorReferenceModel { get; set; }
}
