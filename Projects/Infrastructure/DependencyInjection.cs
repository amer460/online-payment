using Application.Common.Interfaces;
using Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IQRService), typeof(QRService));
        services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
        return services;
    }
}
