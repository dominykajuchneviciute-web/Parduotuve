using Microsoft.AspNetCore.Http;
using EShop.Api.Models;
using EShop.Api.Models.Items;
using EShop.Api.Services;
using Microsoft.AspNetCore.Mvc;
using EShop.Api.DTOs;


namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;
        private readonly ItemFileLoader _itemFileLoader;

        public ItemsController(ItemService itemService, ItemFileLoader itemFileLoader)
        {
            _itemService = itemService;
            _itemFileLoader = itemFileLoader;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetItems()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _itemService.GetByIdAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(CreateItemRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Prekės pavadinimas yra privalomas.");
            }

            Item item;

            switch (request.Category.Trim().ToLowerInvariant())
            {
                case "clothing":
                    item = new Clothing
                    {
                        Name = request.Name,
                        Condition = request.Condition,
                        Description = request.Description,
                        Size = request.Size,
                        Color = request.Color,
                        Manufacturer = request.Manufacturer
                    };
                    break;

                case "electronics":
                    item = new Electronics
                    {
                        Name = request.Name,
                        Condition = request.Condition,
                        Description = request.Description,
                        Model = request.Model,
                        Color = request.Color,
                        Manufacturer = request.Manufacturer
                    };
                    break;

                case "furniture":
                    item = new Furniture
                    {
                        Name = request.Name,
                        Condition = request.Condition,
                        Description = request.Description,
                        Material = request.Material,
                        Color = request.Color,
                        Dimensions = request.Dimensions
                    };
                    break;

                case "transport":
                    item = new Transport
                    {
                        Name = request.Name,
                        Condition = request.Condition,
                        Description = request.Description,
                        Manufacturer = request.Manufacturer,
                        Model = request.Model,
                        Year = request.Year,
                    };
                    break;

                case "footwear":
                    item = new Footwear
                    {
                        Name = request.Name,
                        Condition = request.Condition,
                        Description = request.Description,
                        ShoeSize = request.ShoeSize,
                        Color = request.Color,
                        Manufacturer = request.Manufacturer
                    };
                    break;

                case "instrument":
                    item = new Instrument
                    {
                        Name = request.Name,
                        Condition = request.Condition,
                        Description = request.Description,
                        InstrumentType = request.InstrumentType,
                        Manufacturer = request.Manufacturer,
                        Year = request.Year
                    };
                    break;

                default:
                    return BadRequest(
                        "Nežinoma prekės kategorija. " +
                        "Galimos kategorijos: Clothing, Electronics, Furniture, Transport, Footwear, Instrument.");
            }

            var createdItem = await _itemService.AddAsync(item);

            return CreatedAtAction(
                nameof(GetItem),
                new { id = createdItem.Id },
                createdItem);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadItems(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            await using var stream = file.OpenReadStream();

            var items = await _itemFileLoader.LoadAsync(stream);

            return Ok(items);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var deleted = await _itemService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
