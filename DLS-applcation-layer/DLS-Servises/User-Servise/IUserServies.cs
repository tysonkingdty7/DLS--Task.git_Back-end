using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_applaction_Layer.DTO;
using Domein_Layer.DTO.AutherDTO;


namespace DLS_Domin_layer.DLS_Servises.User_Servise
{
    public interface IUserServies
    {
         Task<AuthModel> RegisterModelAsync(RegisterDTO regsterModel);
        Task<AuthModel> LoginModel(LoginDTO loginModel);
    }
}
