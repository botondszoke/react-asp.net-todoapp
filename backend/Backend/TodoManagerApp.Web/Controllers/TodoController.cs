using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoManagerApp.BL;
using ModelTodo = TodoManagerApp.DAL.Models.Todo;

namespace TodoManagerApi.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoManager tm;

        public TodoController(TodoManager tm) => this.tm = tm;

        [HttpGet]
        public async Task<IEnumerable<ModelTodo>> Get() => await tm.TodoList();

        [HttpGet("{todoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ModelTodo>> Get(int todoId)
        {
            var todo = await tm.GetTodoOrNull(todoId);
            if (todo == null)
                return NotFound();
            else
                return Ok(todo);
        }

        [HttpDelete("{todoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Delete(int todoId)
        {
            var result = await tm.DeleteTodo(todoId);
            if (result == "Not found")
                return NotFound();
            else if (result == "Success")
                return NoContent();
            else
                return Conflict();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Create([FromBody] ModelTodo newTodo)
        {
            var newId = await tm.InsertTodo(newTodo);
            if (newId >= 0)
                return CreatedAtAction(nameof(Get), new { id = newId }, new ModelTodo(newId, newTodo.Title, newTodo.Description, newTodo.Deadline, newTodo.Priority, newTodo.ColumnID));
            else if (newId == -2)
                return BadRequest();
            else
                return Conflict();
        }

        [HttpPut("{todoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<ModelTodo>> Update(int todoId, [FromBody] ModelTodo updatedTodo)
        {
            if (todoId != updatedTodo.ID)
                return BadRequest();

            var result = await tm.UpdateTodo(updatedTodo);
            if (result == "Not found")
                return NotFound();
            else if (result == "Success")
                return Ok(updatedTodo);
            else if (result == "Bad request")
                return BadRequest();
            else
                return Conflict();
        }

        [HttpPut("{todoId}/priority")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<ModelTodo>> ModifiedPriority(int todoId, [FromBody] ModelTodo updatedTodo)
        {
            if (todoId != updatedTodo.ID)
                return BadRequest();

            var result = await tm.ModifiedTodoPriority(updatedTodo);
            if (result == "Not found")
                return NotFound();
            else if (result == "Bad request")
                return BadRequest();
            else if (result == "Success")
                return Ok(updatedTodo);
            else
                return Conflict();
        }
    }
}
