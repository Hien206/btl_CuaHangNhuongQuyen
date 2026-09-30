using DAL;
using Microsoft.AspNetCore.Mvc;
using Model;
using Asp.Versioning;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemNewController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemNewController(IItemRepository itemRepository) =>
            _itemRepository = itemRepository;

        [HttpGet("get-by-id/{id}")]
        public ActionResult<ItemModel> GetDatabyID(string id)
        {
            var item = _itemRepository.GetDatabyID(id);
            return item == null ? NotFound() : Ok(item);
        }
    }
}
