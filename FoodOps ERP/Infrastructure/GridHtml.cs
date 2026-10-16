using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOpsERP.Infrastructure;

public static class GridHtml
{
    /// <param name="filters">"All|Active|Inactive": pehla naam hamesha "sab dikhao" hota hai</param>
    /// <param name="actions">toolbar ke right ke buttons (Razor inline template)</param>
    public static IHtmlContent GridToolbar(this IHtmlHelper html, string placeholder, string noun, int count,
        string? filters = null, Func<object?, IHtmlContent>? actions = null)
    {
        var b = new HtmlContentBuilder();
        b.AppendHtml("<div class=\"grid-toolbar\"><label class=\"grid-search\"><i class=\"bi bi-search\"></i><input type=\"search\" data-table-filter placeholder=\"");
        b.Append(placeholder);
        b.AppendHtml("\" aria-label=\"");
        b.Append(placeholder);
        b.AppendHtml("\" /></label>");

        if (!string.IsNullOrWhiteSpace(filters))
        {
            b.AppendHtml("<div class=\"seg\" role=\"group\" aria-label=\"Filter\">");
            var first = true;
            foreach (var f in filters.Split('|', StringSplitOptions.RemoveEmptyEntries))
            {
                b.AppendHtml(first ? "<button type=\"button\" class=\"is-on\" aria-pressed=\"true\" data-value=\"\">"
                                   : "<button type=\"button\" aria-pressed=\"false\" data-value=\"");
                if (!first) { b.Append(f); b.AppendHtml("\">"); }
                b.Append(f);
                b.AppendHtml("</button>");
                first = false;
            }
            b.AppendHtml("</div>");
        }

        b.AppendHtml("<span class=\"grid-count\" data-grid-count data-noun=\"");
        b.Append(noun);
        b.AppendHtml("\">");
        b.Append($"{count} {noun}");
        b.AppendHtml("</span>");

        if (actions is not null)
        {
            b.AppendHtml("<div class=\"grid-actions\">");
            b.AppendHtml(actions(null));
            b.AppendHtml("</div>");
        }
        b.AppendHtml("</div>");
        return b;
    }
}