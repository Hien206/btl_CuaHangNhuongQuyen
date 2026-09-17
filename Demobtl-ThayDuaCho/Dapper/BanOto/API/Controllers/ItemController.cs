using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Model;

namespace API.Controllers
{
    //[Authorize]
    //[ApiKey]
    //http://localhost:52872/api/Item/get-by-id/1
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private IItemBusiness _itemBusiness;
        private IMemoryCache _memoryCache;
        public ItemController(IItemBusiness itemBusiness, IMemoryCache memoryCache)
        {
            _itemBusiness = itemBusiness;
            _memoryCache = memoryCache;
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public ItemModel GetDatabyID(string id)
        {
            return _itemBusiness.GetDatabyID(id);
        }

        [Route("create-item")]
        [HttpPost]
        public ItemModel CreateItem([FromBody] ItemModel model)
        {
            //_memoryCache.Remove("all-item");
            model.item_id = Guid.NewGuid().ToString();
            _itemBusiness.Create(model);
            return model;
        }
        [Route("update-item")]
        [HttpPost]
        public ItemModel UpdateItem([FromBody] ItemModel model)
        {
            //_memoryCache.Remove("all-item");
            _itemBusiness.Update(model);
            return model;
        }

      

        [Route("get-all")]
        [HttpGet]
        public IEnumerable<ItemModel> GetDatabAll()
        {
            var list = _memoryCache.Get<List<ItemModel>>("all-item");
            if (list == null)
            {
                var result = _itemBusiness.GetDataAll();
                _memoryCache.Set("all-item", result, TimeSpan.FromMinutes(60));
                return result;
            }
            else
            {
                return list;
            }
        }

        [Route("search")]
        [HttpPost]
        public ResponseModel Search([FromBody] Dictionary<string, object> formData)
        {
            var response = new ResponseModel();
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                string item_group_id = "";
                if (formData.Keys.Contains("item_group_id") && !string.IsNullOrEmpty(Convert.ToString(formData["item_group_id"]))) { item_group_id = Convert.ToString(formData["item_group_id"]); }
                string item_name = "";
                if (formData.Keys.Contains("item_name") && !string.IsNullOrEmpty(Convert.ToString(formData["item_name"]))) { item_name = Convert.ToString(formData["item_name"]); }
                long total = 0;
                var data = _itemBusiness.Search(page, pageSize, out total, item_group_id, item_name);
                response.TotalItems = total;
                response.Data = data;
                response.Page = page;
                response.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

    }
}
