namespace XCoreAssignment.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUtilityControllerService), typeof(UtilityControllerService));
            services.AddScoped(typeof(IQRControllerService), typeof(QRControllerService));

            Infrastructure.DependencyInjection.AddInfrastructure(services);

            return services;
        }
    }

}
