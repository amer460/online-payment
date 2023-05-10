using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IQRService
{
    string ReadImage(IFormFile? formFile);
}
