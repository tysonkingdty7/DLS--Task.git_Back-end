using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_applaction_Layer.DTO;
using DLS_Domin_layer.DLS_Repository;
using DLS_Domin_layer.DLS_Repository.Auther;
using DLS_Domin_layer.DLS_Servises.User_Servise;
using DLS_Domin_layer.Modules;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using Domein_Layer.DTO.AutherDTO;

namespace DLS_applcation_layer.DLS_Servises.User_Servise
{
    public class UserServies : BaseRepository<User>, IUserServies
    {
         private readonly IAutherReposirty _regiserReposirty;
        public UserServies(APPDbcontext appContext , IAutherReposirty regiserReposirty ) : base(appContext)
        {
            _regiserReposirty = regiserReposirty;
            

        }

        public async Task<AuthModel> LoginModel(LoginDTO loginModel)
        {
            return await _regiserReposirty.LoginModel(loginModel);


        }

        public Task<AuthModel> RegisterModelAsync(RegisterDTO regsterModel)
        {
            return _regiserReposirty.RegisterModelAsync(regsterModel);
        }
    }
}
