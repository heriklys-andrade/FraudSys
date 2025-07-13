using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FraudSys.Api.Middlewares;
using FraudSys.Domain.Environments;
using FraudSys.Domain.Interfaces.Repositories;
using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services;
using FraudSys.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));

var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();

//DI Services
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
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
