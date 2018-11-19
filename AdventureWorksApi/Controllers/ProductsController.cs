using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Serilog;

namespace AdventureWorksApi.Controllers
{
    public class ProductsController : ApiController
    {
        private AdventureWorksContext db;

        public ProductsController()
        {
            db = new AdventureWorksContext()
            {
                Configuration = { ProxyCreationEnabled = false }
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("log.txt")
                .CreateLogger();
        }

        // GET: api/Products
        public IHttpActionResult GetProducts()
        {
            Log.Information("Getting all the products...");

            var products = db.Products.ToList();

            return Ok(products);
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Log.Information("Getting product by id={ID}", id);

            Product product = db.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            Log.Information("Updating product by id={ID}", id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    Log.Error("Updating failed: product with id={Id} doesn't exist", id);
                    return NotFound();
                }
                else
                {
                    Log.Information("Updating failed for product by id={ID}", id);
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            Log.Information("Posting product with name={Name}", product.Name);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Log.Information("Deleting product by id={ID}", id);

            Product product = db.Products.Find(id);

            if (product == null)
            {
                Log.Error("Updating failed: product with id={Id} doesn't exist", id);
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}