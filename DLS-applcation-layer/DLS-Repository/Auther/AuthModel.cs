using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein_Layer.DTO.AutherDTO
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
     
        public string UserID { get; set; }
        public List<string> Roles { get; internal set; }
        public string Id { get; internal set; }
    }
}
