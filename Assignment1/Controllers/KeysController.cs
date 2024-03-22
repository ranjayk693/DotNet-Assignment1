
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Models;
using Assignment1.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeysController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public KeysController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("key-value")]
        public async Task<ActionResult<IEnumerable<KeyPair>>> GetData()
        {
            var keyPairs = await _dataContext.KeyPairs.ToListAsync();
            return Ok(keyPairs);
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<KeyPair>> GetByIdData(string key)
        {
            var keyPair = await _dataContext.KeyPairs.FirstOrDefaultAsync(elem => elem.key == key);
            if (keyPair == null)
            {
                return NotFound();
            }
            return Ok(keyPair);
        }

        /*Post Api by key-value*/
        [HttpPost]
        public async Task<ActionResult<KeyPair>> PostData(KeyPair data)
        {
            var existingKeyPair = await _dataContext.KeyPairs.FirstOrDefaultAsync(elem => elem.key == data.key);
            if (existingKeyPair != null)
            {
                return Conflict($"A record with key '{data.key}' already exists.");
            }
            _dataContext.KeyPairs.Add(data);
            await _dataContext.SaveChangesAsync();
            return Ok("Data is created sucessfully");
        }

        /*Update value*/
        [HttpPut("{key}")]
        public async Task<IActionResult> PutData(string key, UpdateDtos keyvalue)
        {
            var data = await _dataContext.KeyPairs.FirstOrDefaultAsync(elem => elem.key == key);
            if (data == null)
            {
                return NotFound();
            }
            data.value = keyvalue.value;

            await _dataContext.SaveChangesAsync();
            return Ok("Data is updated sucessfully");
        }

        /*Delete API by key*/
        [HttpDelete("{key}")]
        public async Task<ActionResult> DeleteData(string key)
        {
            var data = await _dataContext.KeyPairs.FirstOrDefaultAsync(elem => elem.key == key);
            if (data == null)
            {
                return NotFound();
            }
            _dataContext.KeyPairs.Remove(data);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
