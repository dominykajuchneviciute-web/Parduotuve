using Microsoft.AspNetCore.Http;
using EShop.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly List<Item> items = new()
        {
            new Item(
                "Miesto dviratis",
                ItemCondition.Used,
                1,
                "Tvarkingas dviratis, paruoštas sezonui.",
                26,
                "Kross",
                "Juoda"
            ),

            new Item(
                "Dell 24\" monitorius",
                ItemCondition.Used,
                2,
                "Full HD monitorius be defektų.",
                24,
                "Dell",
                "Juoda"
            ),

            new Item(
                "C# Programavimo vadovėlis",
                ItemCondition.New,
                3,
                "Naudinga knyga .NET programuotojams.",
                null,
                "Alma littera",
                "Mėlyna"
            )
        };

        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetItem(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);

            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateItem(Item item)
        {
            items.Add(item);

            return Created($"/api/items/{item.Id}", item);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteItem(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);

            if (item is null)
            {
                return NotFound();
            }

            items.Remove(item);

            return NoContent();
        }

    }
}
