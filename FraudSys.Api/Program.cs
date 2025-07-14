using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using FraudSys.Api.Middlewares;
using FraudSys.Domain.Environments;
using FraudSys.Domain.Interfaces.Repositories;
using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services;
using FraudSys.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));

        var awsOptions = builder.Configuration.GetAWSOptions();
        builder.Services.AddDefaultAWSOptions(awsOptions);

        //DynamoDB Settings
        builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<AwsSettings>>().Value;
            var credentials = new BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(settings.Region)
            };

            return new AmazonDynamoDBClient(credentials, config);
        });

        builder.Services.AddSingleton<IDynamoDBContext>(sp =>
        {
            var client = sp.GetRequiredService<IAmazonDynamoDB>();

            return new DynamoDBContextBuilder()
                .WithDynamoDBClient(() => client)
                .ConfigureContext(cfg => cfg.DisableFetchingTableMetadata = true)
                .Build();
        });

        //DI Services
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>(app.Environment.IsDevelopment());

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}