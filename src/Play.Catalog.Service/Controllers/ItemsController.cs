using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Services.Dtos;

namespace Play.Catalog.Services.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ItemsController : ControllerBase
    {
        // private static readonly List<ItemDto> items = new(){
        //     new ItemDto(Guid.NewGuid(),"Potion","Restores a small amount of HP",5,DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(),"AntiDote","Cures poision",7,DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(),"Bromze Sword","Deals a small amount of damage",20,DateTimeOffset.UtcNow)
        // };
        private readonly ItemRepository itemRepository = new();
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemRepository.GetAllSync()).Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ItemDto>> GetById(Guid Id)
        {
            var item = await itemRepository.GetAsync(Id);
            if (item == null)
                return NotFound();
            return item.AsDto();
        }
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(Guid Id, UpdateItemDto updateItemDto)
        {
            var exisitingItem = await itemRepository.GetAsync(Id);
            if (exisitingItem == null)
            {
                return NotFound();
            }
            exisitingItem.Name = updateItemDto.Name;
            exisitingItem.Description = updateItemDto.Description;
            exisitingItem.Price = updateItemDto.Price;
            await itemRepository.UpdateAsync(exisitingItem);
            return NoContent();

        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var exisitingItem = await itemRepository.GetAsync(Id);
            if (exisitingItem == null)
            {
                return NotFound();
            }
            await itemRepository.RemoveAsync(exisitingItem.Id)
            return NoContent();
        }

    }
}
