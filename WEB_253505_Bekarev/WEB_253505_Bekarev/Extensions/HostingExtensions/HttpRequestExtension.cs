namespace WEB_253505_Bekarev.Extensions.HostingExtensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest"; //IsAjaxRequst()? missing
        }
    }
}
