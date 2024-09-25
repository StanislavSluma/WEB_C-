using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Extensions.HostingExtensions;

namespace WEB_253505_Bekarev.Services.CartService
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession? Session { get; set; }
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
            SessionCart cart = session.Get<SessionCart>("cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        public override void AddToCart(Anime anime)
        {
            base.AddToCart(anime);
            Session?.Set("cart", this);
        }
        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set("cart", this);
        }
        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("cart");
        }
    }
}
