using XCoreAssignment.Helpers;
using XCoreAssignment.ViewModels.Utility;
using Domain.Entities;
using Application.Common.Interfaces;

namespace XCoreAssignment.Services;

public interface IQRControllerService
{
    ViewResultDTO IndexGet();
    ViewResultDTO IndexPost(IFormFile FormFile);
}

public class QRControllerService : IQRControllerService
{
    private readonly IQRService _qrService;
    private readonly IPaymentService _paymentService;

    public QRControllerService(IQRService qrService, IPaymentService paymentService)
    {
        _qrService = qrService;
        _paymentService = paymentService;
    }

    public ViewResultDTO IndexGet()
    {
        return new ViewResultDTO("Index");
    }

    public ViewResultDTO IndexPost(IFormFile FormFile)
    {
        if(FormFile == null)
            return new ViewResultDTO("Exception", "Image not specified.");

        string qrCodeText;
        try
        {
            qrCodeText = _qrService.ReadImage(FormFile);
        }
        catch (Exception)
        {
            return new ViewResultDTO("Exception", "Error reading QR code.");
        }

        if (string.IsNullOrWhiteSpace(qrCodeText))
            return new ViewResultDTO("Exception", "Content of QR code is empty.");

        PaymentInfo paymentInfo;
        try
        {
            paymentInfo = _paymentService.GetPaymentInfo(qrCodeText);
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

        I made it so that sender name, address line and city are always the same.
        They are being proccessed on QR code load but I am not changing or validating user's personal information.

         */

        UtilityTemplateVM vm = new(paymentInfo);

        return new ViewResultDTO("~/Views/Utility/Template.cshtml", vm);
    }

}
