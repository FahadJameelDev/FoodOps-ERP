namespace FoodOpsERP.Infrastructure;

public static class HttpRequestExtensions
{
    public static bool IsAjax(this HttpRequest request) =>
        request.Headers["X-Requested-With"] == "XMLHttpRequest";
}