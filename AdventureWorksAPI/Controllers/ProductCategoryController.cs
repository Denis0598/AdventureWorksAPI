using Microsoft.AspNetCore.Mvc;
using AdventureWorksNS.Data;
using AdventureWorksAPI.Repositories;

namespace AdventureWorksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategory repo;

        public ProductCategoryController(IProductCategory repo)
        {
            this.repo = repo;
        }

        [HttpGet]// esto para decir que es
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductCategory>))]  // se usa el codigo 200 si esta bien
        // para que sepa que es el api
        public async Task<IEnumerable<ProductCategory>> GetProductCategories(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync())
                        .Where(ProductCategory => ProductCategory.Name == name);
            }
        }

        [HttpGet("id", Name = nameof(GetProductCategories))] //Ruta
        [ProducesResponseType(200, Type = typeof(ProductCategory))]  //esto si sale todo bien
        [ProducesResponseType(404)] // esto si sale algo mal

        public async Task<IActionResult> GetProductCategories(int id)
        {
            ProductCategory? cp = await repo.RetrieveAsync(id);
            if (cp == null)
            {
                return NotFound(); // Error 404

            }
            return Ok(cp);
        }

        [HttpPost]  // para crear
        [ProducesResponseType(20, Type = typeof(ProductCategory))] //codigo 202 porque funciona
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ProductCategory cp)
        {
            if (cp == null)
            {
                return BadRequest();
            }

            ProductCategory? addproductCategory = await repo.CreateAsync(cp);
            if (addproductCategory == null)
            {
                return BadRequest("Fallo el repositorio");
            }
            else
            {
                return CreatedAtRoute(
                    routeName: nameof(GetProductCategories),
                    routeValues: new { id = addproductCategory.ProductCategoryId },
                    value: addproductCategory);

            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategory cp)
        {
            if (cp == null || cp.ProductCategoryId != id)
            {
                return BadRequest(); //400

            }
            ProductCategory? existe = await repo.RetrieveAsync(id);
            if (existe == null)
            {
                return NotFound(); //404
            }
            await repo.UpdateAsync(id, cp);
            return new NoContentResult(); //204
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<IActionResult> Delete(int id)
        {
            ProductCategory? existe = await repo.RetrieveAsync(id);
            if (existe == null)
            {
                return NotFound(); //404
            }
            bool? deleted = await repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value)
            {
                return new NoContentResult(); //201
            }
            return BadRequest($"El cliente con el {id} no se puede borrar");
        }
    }
}

