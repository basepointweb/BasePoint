using BasePoint.Core.Application.Services;
using BasePoint.Core.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BasePoint.Core.Configurations
{
    public static class ServiceCollectionExtentions
    {
        public static void MapDefaultApplicationServices(this IServiceCollection service)
        {
            service.AddScoped<ITokenAuthentication, TokenAuthentication>();
        }
    }
}