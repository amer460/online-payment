using System.Drawing;
using XCoreAssignment.Helpers;
using XCoreAssignment.ViewModels.Utility;
using ZXing;
using Application;
using Application.QRCode;
using Domain.Entities;

namespace XCoreAssignment.Services;

public interface IQRService
{
    ViewResultDTO IndexGet();
    ViewResultDTO IndexPost(IFormFile FormFile);
}

public class QRService : IQRService
{
    public ViewResultDTO IndexGet()
    {
        return new ViewResultDTO("Index");
    }

    public ViewResultDTO IndexPost(IFormFile FormFile)
    {
        string resultText;
        try
        {

            var barcodeReader = new BarcodeReader();
            using var stream = FormFile.OpenReadStream();
            var bitmap = (Bitmap)Image.FromStream(stream);
            var result = barcodeReader.Decode(bitmap);
            resultText = result.Text;
        }
        catch (Exception)
        {
            return new ViewResultDTO("Exception", "Error reading QR code.");
        }

        if (string.IsNullOrWhiteSpace(resultText))
            return new ViewResultDTO("Exception", "Content of QR code is empty.");


        PaymentInfo paymentInfo;
        try
        {
            paymentInfo = QRPayment.Read(resultText);
        }
        catch (Exception ex)
        {
            return new ViewResultDTO("Exception", ex.Message);
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

        return new ViewResultDTO("~/Views/Utility/Template.cshtml", vm);

    }




}
