using BasePoint.Core.Presentation.AspNetCoreApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

var app = DefaultApiConfiguration.Configure(builder);

app.Run();
