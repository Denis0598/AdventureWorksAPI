using Microsoft.AspNetCore.Mvc;
using AdventureWorksNS.Data;
using AdventureWorksAPI.Repositories;

namespace AdventureWorksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustumerRepository repo;

        public CustomerController(ICustumerRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]// esto para decir que es
        [ProducesResponseType (200, Type = typeof(IEnumerable<Customer>))]  // se usa el codigo 200 si esta bien
        // para que sepa que es el api
        public async Task<IEnumerable<Customer>> GetCustomers(string?  companyName)
        {
            if(string.IsNullOrEmpty(companyName))
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync())
                        .Where(Customer => Customer.CompanyName == companyName);
            }
        }

        [HttpGet("id", Name = nameof(GetCustomer))] //Ruta
        [ProducesResponseType(200, Type =typeof(Customer))]  //esto si sale todo bien
        [ProducesResponseType(404)] // esto si sale algo mal

        public async Task<IActionResult> GetCustomer(int id)
        {
            Customer? c= await repo.RetrieveAsync(id);
            if(c == null)
            {
                return NotFound(); // Error 404

            }
            return Ok(c);
        }

        [HttpPost]  // para crear
        [ProducesResponseType(20, Type = typeof(Customer))] //codigo 202 porque funciona
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Customer c)
        {
            if(c == null)
            {
                return BadRequest();
            }

            Customer? addCustomer = await repo.CreateAsync(c);
            if(addCustomer == null)
            {
                return BadRequest("Fallo el repositorio");
            }
            else
            {
                return CreatedAtRoute(
                    routeName: nameof(GetCustomer),
                    routeValues: new {id = addCustomer.CustomerId},
                    value: addCustomer);
                     
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] Customer c)
        {
            if(c == null || c.CustomerId != id)
            {
                return BadRequest(); //400

            }
            Customer? existe = await repo.RetrieveAsync(id);
            if(existe == null)
            {
                return NotFound(); //404
            }
            await repo.UpdateAsync(id, c);
            return new NoContentResult(); //204
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<IActionResult> Delete(int id)
        {
            Customer? existe = await repo.RetrieveAsync(id);
            if (existe == null)
            {
                return NotFound(); //404
            }
            bool? deleted = await repo.DeleteAsync(id);
            if(deleted.HasValue  &&  deleted.Value)
            {
                return new NoContentResult(); //201
            }
            return BadRequest($"El cliente con el {id} no se puede borrar");
        }
    }
}
