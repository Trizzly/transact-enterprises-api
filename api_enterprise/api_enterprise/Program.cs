using api_enterprise.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment environment = builder.Environment;

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();


builder.Services.AddDbContext<transactContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CONNEXION_BD")
    ));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options => { options.ReportApiVersions = true; });
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Enterprises",
        Version = "v1"
    });
});
var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(options => options.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        if (httpReq.Host.Host == "http://localhost:36944/")
        {
            swagger.Servers = new List<OpenApiServer>
                        { new OpenApiServer { Url = "/enterprises"}};
        }
    }));
    app.UseSwaggerUI(
        options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                   $"swagger/{description.GroupName}/swagger.json",
                   description.GroupName);

                options.RoutePrefix = string.Empty;
            }
        });

}

app.UseRouting();
app.UsePathBase("/{controller}");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
