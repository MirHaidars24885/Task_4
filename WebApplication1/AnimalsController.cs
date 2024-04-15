using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class AnimalsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllAnimals()
    {
        return Ok(DataStore.Animals);
    }

    [HttpGet("{id}")]
    public IActionResult GetAnimalById(int id)
    {
        var animal = DataStore.Animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
        {
            return NotFound();
        }
        return Ok(animal);
    }

    [HttpPost]
    public IActionResult AddAnimal([FromBody] Animal newAnimal)
    {
        var newId = DataStore.Animals.Any() ? DataStore.Animals.Max(a => a.Id) + 1 : 1;
        newAnimal.Id = newId;
        DataStore.Animals.Add(newAnimal);
        return CreatedAtAction(nameof(GetAnimalById), new { id = newAnimal.Id }, newAnimal);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAnimal(int id, [FromBody] Animal updatedAnimal)
    {
        var animal = DataStore.Animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
        {
            return NotFound();
        }

        animal.Name = updatedAnimal.Name;
        animal.Category = updatedAnimal.Category;
        animal.Weight = updatedAnimal.Weight;
        animal.FurColor = updatedAnimal.FurColor;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        var animal = DataStore.Animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
        {
            return NotFound();
        }

        DataStore.Animals.Remove(animal);
        return NoContent();
    }

    // GET: /Animals/{animalId}/Visits
    [HttpGet("{animalId}/Visits")]
    public IActionResult GetVisitsForAnimal(int animalId)
    {
        if (!DataStore.Animals.Any(a => a.Id == animalId))
        {
            return NotFound("Animal not found");
        }

        var visits = DataStore.Visits.Where(v => v.AnimalId == animalId).ToList();
        return Ok(visits);
    }

    // POST: /Animals/{animalId}/Visits
    [HttpPost("{animalId}/Visits")]
    public IActionResult AddVisitForAnimal(int animalId, [FromBody] Visit newVisit)
    {
        if (!DataStore.Animals.Any(a => a.Id == animalId))
        {
            return NotFound("Animal not found");
        }

        newVisit.Id = DataStore.Visits.Any() ? DataStore.Visits.Max(v => v.Id) + 1 : 1;
        newVisit.AnimalId = animalId; // Ensure the new visit is associated with the correct animal
        DataStore.Visits.Add(newVisit);
        return CreatedAtAction(nameof(GetVisitsForAnimal), new { animalId = animalId }, newVisit);
    }
}
