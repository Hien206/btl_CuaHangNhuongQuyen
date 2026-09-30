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
using Asp.Versioning;

namespace API.Controllers
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
