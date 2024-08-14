using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebAPIAutores.Entities;
using WebAPIAutores.Migrations;
using Microsoft.EntityFrameworkCore;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("/api/books")]
    public class BooksController : ControllerBase
    {
        public readonly ApplicationDbContext context;
        public BooksController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get()
        {
            return await context.Books.Include(x => x.Author).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await context.Books.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Book book)
        {
            var author = context.Authors.AnyAsync(x => x.Id == book.AuthorId);

            if (author == null)
            {
                return NotFound("Author not found");
            }

            context.Add(book);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
