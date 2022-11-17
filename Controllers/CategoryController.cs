using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices] BlogDataContext context)
    {
        var categories = await context.Categories.ToListAsync();

        if (categories.Count == 0)
            return NotFound("Nenhuma categoria encontrada!");

        return Ok(categories);
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] BlogDataContext context)
    {
        var category = await context
            .Categories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return NotFound("Nenhuma categoria encontrada!");

        return Ok(category);
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = new Category
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Slug.ToLower()
            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", category);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"05XE09 - Não foi possível incluir a categoria - {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"05XE10 - Falha interna no servidor - {ex.Message}");
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound("Nenhuma categoria encontrada!");

            category.Name = model.Name;
            category.Slug = model.Slug;
            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(category);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"05XE09 - Falha na atualização da categoria - {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"05XE10 - Falha interna no servidor - {ex.Message}");
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound("Nenhuma categoria encontrada!");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(category);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"05XE09 - Falha ao deletar a categoria - {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"05XE10 - Falha interna no servidor - {ex.Message}");
        }
    }
}