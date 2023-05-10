using Application.Common.Interfaces;
using Application.HelperModels;
using Microsoft.AspNetCore.Http;
using System;
using ZXing;
using System.Drawing;

namespace Infrastructure.Persistance;

public class QRService : IQRService
{
    public string ReadImage(IFormFile? qrCodeImage)
    {

        if (qrCodeImage == null)
            throw new Exception("Image not specified.");

        string resultText;
        try
        {

            var barcodeReader = new BarcodeReader();
            using var stream = qrCodeImage.OpenReadStream();
            var bitmap = (Bitmap)Image.FromStream(stream);
            var result = barcodeReader.Decode(bitmap);
            resultText = result.Text;
        }
        catch (Exception)
        {
            throw new Exception("Error reading QR code.");
        }

        if (string.IsNullOrWhiteSpace(resultText))
            throw new Exception("Content of QR code is empty.");

        return resultText;

    }
}
