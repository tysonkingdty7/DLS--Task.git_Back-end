using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_applcation_layer.DTO;
using DLS_Domin_layer.Modules;

namespace DLS_applcation_layer.DLS_Repository.CartRepositry
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetAllItems(string UserID);
        Task<CartItem> Add(string ProducID, string userID);
        Task<CartItem> Update(CartItemUpdateDTO cartItemUpdateDTO);
        Task<CartItem> Delete(int id);
        void Empty(string id);
    }
}
