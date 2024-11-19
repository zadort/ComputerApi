using ComputerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsController : ControllerBase
    {
        private readonly ComputerContext computerContext;

        public OsController(ComputerContext computerContext)
        {
            this.computerContext = computerContext;
        }

        [HttpPost]
        public async Task<ActionResult<OSystem>> Post(CreateOsDto createOsDto)
        {
            var os = new OSystem
            {
                Id = Guid.NewGuid(),
                Name = createOsDto.Name,
                CreatedTime = DateTime.Now,
            };
            if (os != null)
            {
                await computerContext.Os.AddAsync(os);
                await computerContext.SaveChangesAsync();
                return StatusCode(201, os);
            }
            return BadRequest();    
        }

        [HttpGet]
        public async Task<ActionResult<OSystem>> Get()
        {
            return Ok(await computerContext.Os.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<OSystem>> Put(Guid Id, CreateOsDto createOsDto)
        {
            var existingOs = await computerContext.Os.FindAsync(Id);
            if (existingOs == null)
            {
                return NotFound();
            }

            existingOs.Name = createOsDto.Name;
            existingOs.CreatedTime = DateTime.Now;
            computerContext.Os.Update(existingOs);
            await computerContext.SaveChangesAsync();
            return Ok(existingOs);
        }
    }
}
