using aspnetserver.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:3000", "https://appname.azurestaticapps.net");
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions => 
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Asp.Net React Tutorial", Version = "v1" });
});

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<PostRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(swaggerUiOptions => 
{
    swaggerUiOptions.DocumentTitle = "Asp.Net React Tutorial";
    swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API serving a very simple Post model.");
    swaggerUiOptions.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

app.MapGet("/get-all-posts", async (PostRepository repository) => await repository.GetPosts())
    .WithTags("Posts Endpoints");

app.MapGet("/get-post-by-id/{postId}", async (int postId, PostRepository repository) =>
{
    Post? post = await repository.GetPostById(postId);

    if (post == null) return Results.BadRequest();

    return Results.Ok(post);
}).WithTags("Posts Endpoints");

app.MapPost("/create-post", async (Post post, PostRepository repository) =>
{
    bool isOk = await repository.CreatePost(post);

    if (isOk == false) return Results.BadRequest();

    return Results.Ok(isOk);
}).WithTags("Posts Endpoints");

app.MapPut("/update-post", async (Post post, PostRepository repository) =>
{
    bool isOk = await repository.UpdatePost(post);

    if (isOk == false) return Results.BadRequest();

    return Results.Ok(isOk);
}).WithTags("Posts Endpoints");

app.MapDelete("/delete-post-by-id/{postId}", async (int postId, PostRepository repository) =>
{
    bool isOk = await repository.DeletePost(postId);

    if (isOk == false) return Results.BadRequest();

    return Results.Ok(isOk);
}).WithTags("Posts Endpoints");


app.Run();