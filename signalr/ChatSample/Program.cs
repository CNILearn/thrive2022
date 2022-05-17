global using Microsoft.AspNetCore.SignalR;
global using ChatSample;
global using ChatSample.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddSingleton<GroupNamesService>();
var app = builder.Build();
app.UseStaticFiles();

app.MapHub<ChatHub>("/chat");
app.MapHub<GroupChatHub>("/groupchat");

app.MapGet("/api/groups", (GroupNamesService groupsService) =>
{
    var groups = groupsService.GetGroups();
    return Results.Ok(groups);
});

app.MapGet("/", () => "Use a SignalR client");

app.Run();
