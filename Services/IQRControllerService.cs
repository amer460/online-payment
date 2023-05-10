using Application.Common.Interfaces;
using Application.HelperModels;
using Domain.Entities;
using System.Drawing;
using XCoreAssignment.ViewModels.Utility;
using ZXing;

namespace XCoreAssignment.Services;

public interface IQRControllerService
{
    ViewResultHelper IndexGet();
    ViewResultHelper IndexPost(IFormFile qrCodeImage);
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

    public ViewResultHelper IndexGet()
    {
        return new ViewResultHelper("Index");
    }

    public ViewResultHelper IndexPost(IFormFile? qrCodeImage)
    {
        string? qrCodeText = null;
        try
        {
            qrCodeText = _qrService.ReadImage(qrCodeImage);
        }catch (Exception ex)
        {
            return new ViewResultHelper("Exception", ex.Message);
        }

        PaymentInfo paymentInfo;
        try
        {
            paymentInfo = _paymentService.Parse(qrCodeText);
        }
        catch (Exception ex)
        {
            return new ViewResultHelper("Exception", ex.Message);
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

        return new ViewResultHelper("~/Views/Utility/Template.cshtml", vm);
    }
}
