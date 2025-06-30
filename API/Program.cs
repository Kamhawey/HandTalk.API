using API.Extensions;
using Carter;
using HandTalkModel.Module.Extensions;
using Module.Dictionary.Extensions;
using Module.Identity.Extensions;
using Shared.Core.Extensions;
using Shared.Extensions;
using Shared.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

var identityModuleAssembly = typeof(IdentityModuleExtensions).Assembly;
var handTalkModuleAssembly = typeof(HandTalkModuleExtensions).Assembly;
var dictionaryModuleAssembly = typeof(DictionaryModuleExtensions).Assembly;
var sharedModuleAssembly = typeof(SharedModuleExtensions).Assembly;

builder.Services
    .AddMediatRWithAssemblies(identityModuleAssembly)
    .AddMediatRWithAssemblies(handTalkModuleAssembly)
    .AddMediatRWithAssemblies(sharedModuleAssembly)
    .AddMediatRWithAssemblies(dictionaryModuleAssembly);

builder.Services
    .AddCarterModules(identityModuleAssembly, handTalkModuleAssembly, dictionaryModuleAssembly, sharedModuleAssembly);

builder.Services
    .AddSharedModuleServices()
    .AddIdentityModule(builder.Configuration)
    .AddHandTalkModule(builder.Configuration)
    .AddDictionaryModule(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAny");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
//await app.SeedDictionaryEntriesAsync();
app.Run();
