using SlowBurn.Components;
using SlowBurn.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IContactService, ContactService>();

var url = builder.Configuration["Supabase:Url"]!;
var key = builder.Configuration["Supabase:Key"]!;
var supabaseOptions = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = false
};
var supabase = new Supabase.Client(url, key, supabaseOptions);
await supabase.InitializeAsync();
builder.Services.AddSingleton(supabase);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


