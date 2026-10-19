using FoodOpsERP.Data;
using FoodOpsERP.Middlewares;
using FoodOpsERP.Services.Inventory;
using FoodOpsERP.Services.Manufacturing.Costing;
using FoodOpsERP.Services.Manufacturing.Production;
using FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;
using FoodOpsERP.Services.Manufacturing.Quality;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ═══════════════════════════════════════════════════════════
// 1. DATABASE
// ═══════════════════════════════════════════════════════════
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
            sqlOptions.CommandTimeout(60);
        }));

// ═══════════════════════════════════════════════════════════
// 2. MVC + ANTI-FORGERY + RAZOR VIEW LOCATION
// ═══════════════════════════════════════════════════════════
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
})
.AddRazorOptions(options =>
{

    // YEH NAYI LINE ADD HUI HAI:
    options.ViewLocationExpanders.Add(new FoodOpsERP.Infrastructure.ModuleViewLocationExpander());

    options.ViewLocationFormats.Add("/Views/SupplyChain/Inventory/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/SupplyChain/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Manufacturing/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Workforce/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/AI/{1}/{0}.cshtml");
    // /Views/Shared/{0}.cshtml pehle se default hai, dobara add karne ki zarurat nahi
});

// AJAX (fetch) POST/DELETE token header se bhejte hain
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
    options.Cookie.Name = ".FoodOpsERP.Antiforgery";
});

// ═══════════════════════════════════════════════════════════
// 3. SESSION + TEMPDATA
// ═══════════════════════════════════════════════════════════
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".FoodOpsERP.Session";
});

builder.Services.Configure<CookieTempDataProviderOptions>(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".FoodOpsERP.TempData";
});

// ═══════════════════════════════════════════════════════════
// 4. INVENTORY MODULE — Services (DI)
// ═══════════════════════════════════════════════════════════
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IGoodsReceivingService, GoodsReceivingService>();
builder.Services.AddScoped<IIncomingQCService, IncomingQCService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IInventoryLookupService, InventoryLookupService>();
// Module 3.1 - Products & Recipes
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IPackagingConfigService, PackagingConfigService>();
builder.Services.AddScoped<IRecipeChangeRequestService, RecipeChangeRequestService>();
builder.Services.AddScoped<IProductService, ProductService>();
// Module 3.2 - Production & Manufacturing (YEH NAYI LINES ADD KAREIN)
builder.Services.AddScoped<IDemandService, DemandService>();
builder.Services.AddScoped<IProductionPlanService, ProductionPlanService>();
builder.Services.AddScoped<IProductionOrderService, ProductionOrderService>();
// Module 3.3 - Quality Control
builder.Services.AddScoped<IQCParameterService, QCParameterService>();
builder.Services.AddScoped<IQualityDecisionService, QualityDecisionService>();
// Module 3.4 - Costing & Variance
builder.Services.AddScoped<ICostSetupService, CostSetupService>();
builder.Services.AddScoped<IOverheadService, OverheadService>();
builder.Services.AddScoped<IBatchCostingService, BatchCostingService>();


// ═══════════════════════════════════════════════════════════
// 5. HTTP CONTEXT ACCESSOR
// ═══════════════════════════════════════════════════════════
builder.Services.AddHttpContextAccessor();

// ═══════════════════════════════════════════════════════════
// 6. LOGGING
// ═══════════════════════════════════════════════════════════
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// ═══════════════════════════════════════════════════════════
// BUILD APP
// ═══════════════════════════════════════════════════════════
var app = builder.Build();

// ═══════════════════════════════════════════════════════════
// 7. MIDDLEWARE PIPELINE
// ═══════════════════════════════════════════════════════════

// 7.1 Global Exception Handler — SABSE PEHLA
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ?v=hash wali files (asp-append-version) 1 saal cache
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.Context.Request.Query.ContainsKey("v"))
            ctx.Context.Response.Headers.CacheControl = "public,max-age=31536000,immutable";
    }
});

// 7.2 Security headers + Vary (sirf dynamic pages)
app.Use(async (context, next) =>
{
    var h = context.Response.Headers;
    h["X-Content-Type-Options"] = "nosniff";
    h["X-Frame-Options"] = "DENY";
    h["Referrer-Policy"] = "strict-origin-when-cross-origin";
    // Same URL: kabhi poora page, kabhi sirf content. Back button pe galat copy cache na ho.
    h.Append("Vary", "X-Requested-With");
    await next();
});

app.UseRouting();

// 7.3 Session
app.UseSession();

// 7.4 Authentication (future — Identity)
// app.UseAuthentication();

app.UseAuthorization();

// ═══════════════════════════════════════════════════════════
// 8. ROUTES
// ═══════════════════════════════════════════════════════════
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();