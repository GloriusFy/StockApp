namespace Stock.WebAPI;

public interface IStockStartup
{
    void Configure(IApplicationBuilder application);
    void ConfigureServices(IServiceCollection services);
}