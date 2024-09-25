using Microsoft.AspNetCore.Mvc;
using WEB_253505_Bekarev.Domain.Entities;

namespace WEB_253505_Bekarev.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Cart _cart;
        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(View(_cart));
        }
    }
}
