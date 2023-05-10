namespace XCoreAssignment.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        Infrastructure.DependencyInjection.AddInfrastructure(services);

        services.AddScoped<IQRControllerService, QRControllerService>();
        services.AddScoped<IUtilityControllerService, UtilityControllerService>();

        return services;
    }
}
