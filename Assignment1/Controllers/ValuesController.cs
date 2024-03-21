
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
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ValuesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KeyPairDtos>>> GetData()
        {
            var keyPairs = await _dataContext.KeyPairs.ToListAsync();
            var keyPairDtos = keyPairs.Select(kp => new KeyPairDtos { key = kp.key, value = kp.value }).ToList();

            return Ok(keyPairDtos);
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<KeyPairDtos>> GetByIdData(string key)
        {
            var keyPair = await _dataContext.KeyPairs.FirstOrDefaultAsync(elem => elem.key == key);
            if (keyPair == null)
            {
                return NotFound();
            }
            var newKeyPair = new KeyPairDtos { key = keyPair.key, value = keyPair.value };
            return Ok(newKeyPair);
        }

        [HttpPost]
        public async Task<ActionResult<KeyPairDtos>> PostData(KeyPairDtos data)
        {
            var existingKeyPair = await _dataContext.KeyPairs.FirstOrDefaultAsync(elem => elem.key == data.key);
            if (existingKeyPair != null)
            {
                return Conflict($"A record with key '{data.key}' already exists.");
            }
            var newData=new KeyPair {id= Guid.NewGuid(), key = data.key,value=data.value };
            _dataContext.KeyPairs.Add(newData);
            await _dataContext.SaveChangesAsync();
            return Ok("Data is created sucessfully");
        }

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
