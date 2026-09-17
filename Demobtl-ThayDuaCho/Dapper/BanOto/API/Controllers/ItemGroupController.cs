using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model;

namespace API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemGroupController : ControllerBase
    {
        private IItemGroupBusiness _itemGroupBusiness;
        public ItemGroupController(IItemGroupBusiness itemGroupBusiness)
        {
            _itemGroupBusiness = itemGroupBusiness;
        }

        [Route("get-menu")]
        [HttpGet]
        public IEnumerable<ItemGroupModel> GetAllMenu()
        {
            return _itemGroupBusiness.GetData();
        }
    }
}
