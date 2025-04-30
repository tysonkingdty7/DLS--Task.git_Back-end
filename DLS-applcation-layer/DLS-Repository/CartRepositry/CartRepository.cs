using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLS_applcation_layer.DLS_Repository.CartRepositry;
using DLS_applcation_layer.DTO;
using DLS_Domin_layer.Modules;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using Microsoft.EntityFrameworkCore;

namespace DLS_applcation_layer.DLS_Repository.CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly APPDbcontext _context;

        public CartRepository(APPDbcontext context)
        {
            _context = context;
        }

        public async Task<CartItem> Add(string ProducID, string userID)
        {
            var user = _context.Users.FirstOrDefault(i => i.Id == userID);
            if (user != null)
            {
                if (user.Cart.Items.Where(c => c.ProductID == new Guid(ProducID)).Count() != 0)
                    return null;
                var item = new CartItem
                {
                    CartID = user.Cart.Id,
                    ProductID = new Guid(ProducID),
                    Qauntety = 1
                };
                await _context.CartItems.AddAsync(item);
                return item;
            }
            return null;
        }

        public async Task<CartItem> Delete(int id)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(item => item.Id == id);
            if (item != null)
                _context.CartItems.Remove(item);
            return item;
        }

        public async void Empty(string id)
        {
            var user = _context.Users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                foreach (var item in user.Cart.Items)
                {
                    _context.CartItems.Remove(item);
                }
            }
        }
        public async Task<IEnumerable<CartItem>> GetAllItems(string UserId)
        {
            return await _context.CartItems.Where(c => c.Cart.UserId == UserId).ToListAsync();
        }

        public async Task<CartItem> Update(CartItemUpdateDTO cartItemUpdateDTO)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(item => item.Id == cartItemUpdateDTO.CartItemID);
            if (item == null)
                return item;
            item.Qauntety = cartItemUpdateDTO.Quantity;
            _context.CartItems.Update(item);
            return item;
        }
    }
}

   

