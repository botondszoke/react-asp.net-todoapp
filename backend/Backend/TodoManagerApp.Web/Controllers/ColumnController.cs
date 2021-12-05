using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoManagerApp.BL;
using ModelColumn = TodoManagerApp.DAL.Models.Column;

namespace TodoManagerApi.Controllers
{
    [Route("api/column")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly ColumnManager cm;

        public ColumnController(ColumnManager cm) => this.cm = cm;

        [HttpGet]
        public async Task<IEnumerable<ModelColumn>> Get() => await cm.ColumnList();

        [HttpGet("{columnId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ModelColumn>> Get(int columnId)
        {
            var column = await cm.GetColumnOrNull(columnId);
            if (column == null)
                return NotFound();
            else
                return Ok(column);
        }

        [HttpDelete("{columnId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Delete(int columnId)
        {
            var result = await cm.DeleteColumn(columnId);
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
        public async Task<IActionResult> Create([FromBody] ModelColumn newColumn)
        {
            var newId = await cm.InsertColumn(newColumn);
            if (newId >= 0)
                return CreatedAtAction(nameof(Get), new { id = newId }, new ModelColumn(newId, newColumn.Name, newColumn.Priority));
            else if (newId == -2)
                return BadRequest();
            else
                return Conflict();
        }

        [HttpPut("{columnId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<ModelColumn>> Update(int columnId, [FromBody] ModelColumn updatedColumn)
        {
            if (columnId != updatedColumn.ID)
                return BadRequest();

            var result = await cm.UpdateColumn(updatedColumn);
            if (result == "Not found")
                return NotFound();
            else if (result == "Success")
                return Ok(updatedColumn);
            else if (result == "Bad request")
                return BadRequest();
            else
                return Conflict();
        }

        [HttpPut("{columnId}/priority")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<ModelColumn>> ModifiedPriority(int columnId, [FromBody] ModelColumn updatedColumn)
        {
            if (columnId != updatedColumn.ID)
                return BadRequest();

            var result = await cm.ModifiedColumnPriority(updatedColumn);
            if (result == "Not found")
                return NotFound();
            else if (result == "Bad request")
                return BadRequest();
            else if (result == "Success")
                return Ok(updatedColumn);
            else
                return Conflict();
        }
    }
}
