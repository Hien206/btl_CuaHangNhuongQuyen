using DAL;
using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using Helper;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BLL
{
    public partial class NewsBusiness : INewsBusiness
    {
        private INewsRepository _res;
        private string Secret;
        public NewsBusiness(INewsRepository res, IConfiguration configuration)
        {
            Secret = configuration["AppSettings:Secret"];
            _res = res;
        }
        
        public bool Create(NewsModel model)
        {
            return _res.Create(model);
        }
       
    }

}
