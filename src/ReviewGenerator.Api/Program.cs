using ReviewGenerator.Core.Managers;
using ReviewGenerator.Core.Models;
using ReviewGenerator.Core.Repositories;
using ReviewGenerator.Core.Services;
using ReviewGenerator.Data;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<FilesystemReviewRepositoryOptions>(
    builder.Configuration.GetSection(nameof(FilesystemReviewRepositoryOptions)));
builder.Services.Configure<StarsGeneratorOptions>(
    builder.Configuration.GetSection(nameof(StarsGeneratorOptions)));

//builder.Services.AddSingleton<IReviewTextGenerator, MarkovChainReviewTextGenerator>();
builder.Services.AddSingleton<IReviewTextGenerator, MarkovChainNuGetReviewTextGenerator>();
builder.Services.AddSingleton<IStarsGenerator, BasicSentimentStarsGenerator>();
builder.Services.AddSingleton<IReviewDataInfoProvider, ReviewDataInfoProvider>();
builder.Services.AddSingleton<IWeightedChoiceProvider, WeightedChoiceProvider>();
builder.Services.AddSingleton<IReviewRepository, FileSystemReviewRepository>();
builder.Services.AddSingleton<IReviewGenerator, StarsAndTextReviewGenerator>();
builder.Services.AddSingleton<IReviewCreationManager, ReviewCreationManager>();

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            builder =>
            {
                builder.AllowAnyOrigin()
                        .AllowAnyHeader();
            });
    }
);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Services.GetRequiredService<IReviewTextGenerator>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
