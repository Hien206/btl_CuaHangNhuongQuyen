using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private INewsBusiness _newsBusiness;
        private string _path;
        public NewsController(INewsBusiness newsBusiness, IConfiguration configuration)
        {
            _newsBusiness = newsBusiness;
            _path = configuration["AppSettings:PATH"];
        }

        [Route("create-news")]
        [HttpPost]
        public NewsModel CreateUser([FromBody] NewsModel model)
        {
            model.news_id = Guid.NewGuid().ToString();
            _newsBusiness.Create(model);
            return model;
        } 
    }
}
