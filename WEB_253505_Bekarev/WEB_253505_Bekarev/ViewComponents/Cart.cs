using Microsoft.AspNetCore.Mvc;

namespace WEB_253505_Bekarev.ViewComponents
{
    public class Cart : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
