using System.Text.Json;
using Asp.Versioning;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Model;

namespace API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class ItemController : ControllerBase
{
    private const string AllItemsCacheKey = "all-item";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IItemBusiness _itemBusiness;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<ItemController> _logger;

    public ItemController(
        IItemBusiness itemBusiness,
        IMemoryCache memoryCache,
        IDistributedCache distributedCache,
        ILogger<ItemController> logger)
    {
        _itemBusiness = itemBusiness;
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<ActionResult<ItemModel>> GetDatabyID(
        string id,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Mã xe không được để trống.", nameof(id));
        }

        var cacheKey = $"item_{id}";
        var cachedData = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrWhiteSpace(cachedData))
        {
            _logger.LogInformation("Distributed cache hit cho xe {ItemId}", id);
            return Ok(JsonSerializer.Deserialize<ItemModel>(cachedData, JsonOptions));
        }

        _logger.LogInformation("Distributed cache miss cho xe {ItemId}", id);
        var item = _itemBusiness.GetDatabyID(id)
            ?? throw new KeyNotFoundException($"Không tìm thấy xe có mã '{id}'.");

        await _distributedCache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(item, JsonOptions),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            },
            cancellationToken);

        return Ok(item);
    }

    [HttpPost("create-item")]
    public async Task<ActionResult<ItemModel>> CreateItem(
        [FromBody] ItemModel model,
        CancellationToken cancellationToken)
    {
        model.item_id = Guid.NewGuid().ToString();
        _itemBusiness.Create(model);
        _memoryCache.Remove(AllItemsCacheKey);
        await _distributedCache.RemoveAsync($"item_{model.item_id}", cancellationToken);

        _logger.LogInformation(
            "Đã tạo xe {ItemId} - {ItemName}, giá {ItemPrice}",
            model.item_id,
            model.item_name,
            model.item_price);
        return Ok(model);
    }

    [HttpPost("update-item")]
    public async Task<ActionResult<ItemModel>> UpdateItem(
        [FromBody] ItemModel model,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(model.item_id))
        {
            throw new ArgumentException("Mã xe không được để trống.", nameof(model.item_id));
        }

        _itemBusiness.Update(model);
        _memoryCache.Remove(AllItemsCacheKey);
        await _distributedCache.RemoveAsync($"item_{model.item_id}", cancellationToken);

        _logger.LogInformation(
            "Đã cập nhật xe {ItemId} - {ItemName}, giá {ItemPrice}",
            model.item_id,
            model.item_name,
            model.item_price);
        return Ok(model);
    }

    [HttpGet("get-all")]
    [ApiKey]
    public ActionResult<IEnumerable<ItemModel>> GetDatabAll()
    {
        if (_memoryCache.TryGetValue(AllItemsCacheKey, out List<ItemModel>? items) && items is not null)
        {
            _logger.LogInformation("Memory cache hit cho danh sách xe");
            return Ok(items);
        }

        _logger.LogInformation("Memory cache miss cho danh sách xe");
        items = _itemBusiness.GetDataAll();
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        _memoryCache.Set(AllItemsCacheKey, items, cacheOptions);
        return Ok(items);
    }

    [HttpPost("search")]
    public ActionResult<ResponseModel> Search([FromBody] Dictionary<string, object> formData)
    {
        var page = GetRequiredInt(formData, "page");
        var pageSize = GetRequiredInt(formData, "pageSize");
        if (page < 1 || pageSize < 1)
        {
            throw new ArgumentException("page và pageSize phải lớn hơn 0.");
        }

        var itemGroupId = GetOptionalString(formData, "item_group_id");
        var itemName = GetOptionalString(formData, "item_name");
        var data = _itemBusiness.Search(
            page,
            pageSize,
            out var total,
            itemGroupId,
            itemName);

        return Ok(new ResponseModel
        {
            TotalItems = total,
            Data = data,
            Page = page,
            PageSize = pageSize
        });
    }

    private static int GetRequiredInt(IReadOnlyDictionary<string, object> data, string key)
    {
        if (!data.TryGetValue(key, out var value) ||
            !int.TryParse(Convert.ToString(value), out var result))
        {
            throw new ArgumentException($"Trường '{key}' không hợp lệ.");
        }

        return result;
    }

    private static string GetOptionalString(IReadOnlyDictionary<string, object> data, string key) =>
        data.TryGetValue(key, out var value) ? Convert.ToString(value) ?? string.Empty : string.Empty;
}
