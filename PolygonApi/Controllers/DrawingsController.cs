using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PolygonApi.Model;
using System.Text.Json;
using System.IO;

namespace PolygonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrawingsController : ControllerBase
    {
        private string _filePath; //dosya yolunu tutacagim degisken

        public DrawingsController()
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            _filePath = Path.Combine(directory, "drawings.json"); //root altına dosyanın yolu
        }

        //get isteklerini karsilar..
        [HttpGet]
        public IActionResult GetDrawings()
        {
            try
            {
                if (System.IO.File.Exists(_filePath))
                {
                    var json = System.IO.File.ReadAllText(_filePath);
                    if (!string.IsNullOrEmpty(json))
                    {
                        var drawings = JsonSerializer.Deserialize<List<Drawing>>(json);
                        return Ok(drawings);
                    }
                }
                return Ok(new List<Drawing>());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the file: {ex.Message}");
            }
        }

        //post isteklerini karsilar..
        [HttpPost]
        public IActionResult AddDrawing([FromBody] Drawing drawing)
        {
            if (drawing == null)
            {
                return BadRequest("Drawing cannot be null");
            }

            try
            {
                List<Drawing> drawings = new List<Drawing>();
                if (System.IO.File.Exists(_filePath))
                {
                    var json = System.IO.File.ReadAllText(_filePath);
                    if (!string.IsNullOrEmpty(json))
                    {
                        drawings = JsonSerializer.Deserialize<List<Drawing>>(json);
                    }
                }
                drawings.Add(drawing);
                var updatedJson = JsonSerializer.Serialize(drawings);
                System.IO.File.WriteAllText(_filePath, updatedJson);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data to the file: {ex.Message}");
            }
        }
    }
}
