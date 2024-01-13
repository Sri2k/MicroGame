using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Services.Dtos;

namespace Play.Catalog.Services.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new(){
            new ItemDto(Guid.NewGuid(),"Potion","Restores a small amount of HP",5,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"AntiDote","Cures poision",7,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Bromze Sword","Deals a small amount of damage",20,DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{Id}")]
        public ActionResult<ItemDto> GetById(Guid Id)
        {
            var item = items.Where(x => x.Id == Id).SingleOrDefault();
            if (item == null)
                return NotFound();
            return item;
        }
        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            ItemDto itemDto = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(itemDto);
            return CreatedAtAction(nameof(GetById), new { Id = itemDto.Id }, itemDto);
        }
        [HttpPut("{Id}")]
        public IActionResult Put(Guid Id, UpdateItemDto updateItemDto)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            ItemDto existingItem = items.Where(x => x.Id == Id).SingleOrDefault();
            if(existingItem == null)
                return NotFound();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            ItemDto updateItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price,
            };
            var index = items.FindIndex(x => x.Id == Id);
            items[index] = updateItem;
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            var index = items.FindIndex(x => x.Id == Id);
            if(index<0)
                return NotFound();
            items.RemoveAt(index);
            return NoContent();
        }

    }
}
