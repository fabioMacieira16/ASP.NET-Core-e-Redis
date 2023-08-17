using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TodoRedis.Core.Entities;
using TodoRedis.Infrastructure.Caching;
using TodoRedis.Infrastructure.Persistence;
using TodoRedis.Models;

namespace TodoRedis.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ToDoListDbContext _context;
    private readonly ICachingService _cache;

    public TodoController(ToDoListDbContext context, ICachingService cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var todoCache = await _cache.GetAsync(id.ToString());
        ToDo? todo;

        if (!string.IsNullOrWhiteSpace(todoCache))
        {
            todo = JsonConvert.DeserializeObject<ToDo>(todoCache);

            Console.WriteLine("Loadded from cache.");

            return Ok(todo);
        }

        todo = await _context.ToDos.SingleOrDefaultAsync(t => t.Id == id);

        if (todo == null)
            return NotFound();

        await _cache.SetAsync(id.ToString(), JsonConvert.SerializeObject(todo));

        return Ok(todo);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ToDoInputModel model)
    {
        var todo = new ToDo(0, model.Title, model.Description);

        await _context.ToDos.AddAsync(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, model);
    }
}
