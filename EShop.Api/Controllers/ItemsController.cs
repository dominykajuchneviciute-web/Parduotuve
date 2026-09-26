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

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult<List<Item>>> GetItems()
        {
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

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
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
