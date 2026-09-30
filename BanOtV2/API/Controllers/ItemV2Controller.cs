using Asp.Versioning;
using BLL;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/Item")]
public sealed class ItemV2Controller : ControllerBase
{
    private readonly IItemBusiness _itemBusiness;

    public ItemV2Controller(IItemBusiness itemBusiness)
    {
        _itemBusiness = itemBusiness;
    }

    /// <summary>
    /// Phiên bản 2 trả dữ liệu kèm metadata thay vì trả trực tiếp một mảng như phiên bản 1.
    /// </summary>
    [HttpGet("get-all")]
    public IActionResult GetAll()
    {
        var items = _itemBusiness.GetDataAll();
        return Ok(new
        {
            version = "2.0",
            totalItems = items.Count,
            data = items
        });
    }
}
