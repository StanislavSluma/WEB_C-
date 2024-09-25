using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253505_Bekarev.Domain.Entities
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="anime">Добавляемый объект</param>
        public virtual void AddToCart(Anime anime)
        {
            if (CartItems.ContainsKey(anime.Id))
            {
                CartItems[anime.Id].Count += 1;
            } 
            else
            {
                CartItems[anime.Id] = new CartItem { Anime = anime, Count = 1};
            }
        }
        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }
        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count 
        { 
            get => CartItems.Sum(item => item.Value.Count);
        }
        /// <summary>
        /// Общее количество минут
        /// </summary>
        public int TotalTime
        {
            get => CartItems.Sum(item => item.Value.Anime.TotalTime * item.Value.Count);
        }
    }
}
