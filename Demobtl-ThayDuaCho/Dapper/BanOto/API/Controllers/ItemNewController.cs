using DAL;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
