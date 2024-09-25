using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System.Reflection.Emit;
using System.Reflection;
using System;
using WEB_253505_Bekarev.Domain.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WEB_253505_Bekarev.TagHelpers
{
    [HtmlTargetElement("Pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly HttpContext _httpContext;
        private readonly LinkGenerator _linkGenerator;

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Category { get; set; }
        public bool Admin { get; set; } = false;

        public PagerTagHelper(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _linkGenerator = linkGenerator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.Attributes.SetAttribute("class", "Page navigation example");

            var ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination");

            var isPrevAvaibale = CurrentPage > 1;
            var isNextAvaibale = CurrentPage < TotalPages;

            var prevLiTag = new TagBuilder("li");
            prevLiTag.AddCssClass(isPrevAvaibale ? "page-item" : "page-item disabled");

            var aPrevTag = new TagBuilder("a");
            aPrevTag.AddCssClass("page-link");
            aPrevTag.Attributes["aria-label"] = "Previous";

            if (isPrevAvaibale)
            {
                var prevUrl = GetPageUrl(CurrentPage - 1);
                aPrevTag.Attributes["href"] = prevUrl;
                aPrevTag.Attributes["data-ajax-url"] = prevUrl;
                aPrevTag.Attributes["data-ajax-method"] = "GET";
            }
            aPrevTag.InnerHtml.AppendHtml("&laquo;");
            prevLiTag.InnerHtml.AppendHtml(aPrevTag);
            ulTag.InnerHtml.AppendHtml(prevLiTag);

            for (int i = 1; i <= TotalPages; i++)
            {
                var liTag = new TagBuilder("li");
                liTag.AddCssClass("page-item");

                if (CurrentPage == i)
                {
                    liTag.AddCssClass("active");
                }

                var pageUrl = GetPageUrl(i);

                var aTag = new TagBuilder("a");
                aTag.AddCssClass("page-link");
                aTag.Attributes["href"] = pageUrl;
                aTag.Attributes["data-ajax-url"] = pageUrl;
                aTag.Attributes["data-ajax-method"] = "GET";
                aTag.InnerHtml.AppendHtml(i.ToString());

                liTag.InnerHtml.AppendHtml(aTag);
                ulTag.InnerHtml.AppendHtml(liTag);
            }

            var nextLiTag = new TagBuilder("li");
            nextLiTag.AddCssClass(isNextAvaibale ? "page-item" : "page-item disabled");

            var aNextTag = new TagBuilder("a");
            aNextTag.AddCssClass("page-link");
            aNextTag.Attributes["aria-label"] = "Next";

            if (isNextAvaibale)
            {
                var nextUrl = GetPageUrl(CurrentPage + 1);
                aNextTag.Attributes["href"] = nextUrl;
                aNextTag.Attributes["data-ajax-url"] = nextUrl;
                aNextTag.Attributes["data-ajax-method"] = "GET";
            }

            aNextTag.InnerHtml.AppendHtml("&raquo;");
            nextLiTag.InnerHtml.AppendHtml(aNextTag);
            ulTag.InnerHtml.AppendHtml(nextLiTag);

            output.Content.AppendHtml(ulTag);
        }

        private string GetPageUrl(int pageNumber)
        {
            if (Admin)
            {
                return _linkGenerator.GetPathByPage(
                    _httpContext,
                    page: "/Index",
                    values: new { area = "Admin", pageNo = pageNumber }
                );
            }
            else
            {
                return _linkGenerator.GetPathByAction(
                    _httpContext,
                    action: "Index",
                    controller: "Product",
                    values: new { pageNo = pageNumber, category = Category }
                );
            }
        }
    }
}

/*<nav aria-label="Page navigation example">
    <ul class="pagination">
        <li class="page-item">
            <a class="page-link" aria-label="Previous" asp-action="index" asp-controller="product" asp-route-pageNo="@(Model.CurrentPage==1?Model.TotalPages:Model.CurrentPage-1)" asp-route-category=@current_category>
                <span aria-hidden="true">&laquo;</span>
            </a>
        </li>
        @for(int i = 1; i <= Model.TotalPages; i++)
        {
            <li class="page-item @(i==Model.CurrentPage?"active":"")">
                <a class="page-link" asp-controller="product" asp-action="index" asp-route-pageNo="@i" asp-route-category=@current_category>
                    @i
                </a>
            </li>
        }
        <li class="page-item">
            <a class="page-link" aria-label="Next" asp-route-pageNo="@(Model.CurrentPage==Model.TotalPages?1:Model.CurrentPage+1)" asp-route-category=@current_category>
                <span aria-hidden="true">&raquo;</span>
            </a>
        </li>
    </ul>
</nav>*/
