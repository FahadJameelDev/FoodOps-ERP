using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace FoodOpsERP.Infrastructure;

public sealed class ModuleViewLocationExpander : IViewLocationExpander
{
    private const string Prefix = "FoodOpsERP.Controllers.";

    private static string? ModulePath(ViewLocationExpanderContext c)
    {
        var ns = (c.ActionContext.ActionDescriptor as ControllerActionDescriptor)?.ControllerTypeInfo.Namespace;
        return ns is not null && ns.StartsWith(Prefix) ? ns[Prefix.Length..].Replace('.', '/') : null;
    }

    public void PopulateValues(ViewLocationExpanderContext c) => c.Values["module"] = ModulePath(c);

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext c, IEnumerable<string> viewLocations)
    {
        var module = ModulePath(c);
        if (module is not null) yield return $"/Views/{module}/{{1}}/{{0}}.cshtml";
        foreach (var l in viewLocations) yield return l;
    }
}