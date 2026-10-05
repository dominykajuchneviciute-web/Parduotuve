using Microsoft.AspNetCore.Http;
using EShop.Api.Models;
using EShop.Api.Services;
using Microsoft.AspNetCore.Mvc;


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
        public async Task<ActionResult<Item>> CreateItem(Item item)
        {
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
