using Blog.Data;
using Blog.Extensions;
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
        try
        {
            var categories = await context.Categories.ToListAsync();
            if (categories.Count == 0)
                return NotFound(new ResultViewModel<Category>("Nenhuma categoria encontrada!"));

            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new ResultViewModel<List<Category>>($"05X04 - Falha interna no servidor - | {ex.Message} |"));
        }
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(
                    new ResultViewModel<Category>($"Nenhuma categoria encontrada! | Categoria pesquisada - '{id}' |"));

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new ResultViewModel<Category>($"05X04 - Falha interna no servidor - | {ex.Message} |"));
        }
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

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

            return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500,
                new ResultViewModel<Category>($"05XE09 - Não foi possível incluir a categoria - | {ex.Message} |"));
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new ResultViewModel<Category>($"05XE10 - Falha interna no servidor - | {ex.Message} |"));
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
                return NotFound(
                    new ResultViewModel<Category>($"Nenhuma categoria encontrada! | Categoria pesquisada - '{id}' |"));

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500,
                new ResultViewModel<Category>($"05XE08 - Falha na atualização da categoria - | {ex.Message} |"));
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new ResultViewModel<Category>($"05XE10 - Falha interna no servidor - | {ex.Message} |"));
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
                return NotFound(
                    new ResultViewModel<Category>($"Nenhuma categoria encontrada! | Categoria pesquisada - '{id}' |"));

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<Category>(
                $"Categoria deletada com sucesso - Categoria Deletada: {category.Id}"));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500,
                new ResultViewModel<Category>($"05XE07 - Falha ao deletar a categoria - | {ex.Message} |"));
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new ResultViewModel<Category>($"05XE10 - Falha interna no servidor - | {ex.Message} |"));
        }
    }
}