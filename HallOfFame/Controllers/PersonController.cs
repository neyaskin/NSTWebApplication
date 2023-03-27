using HallOfFame.Exceptions;
using HallOfFame.Interfaces;
using HallOfFame.Models;
using HallOfFame.Models.Requests;
using HallOfFame.Models.Response;
using HallOfFame.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFame.Controllers;

[ApiController]
[Route("api/v1")]
public class PersonController : ControllerBase
{
    private IPersonService _personService;
    private readonly ILogger<IPersonService> _logger;

    public PersonController(IPersonService personService, ILogger<IPersonService> logger)
    {
        _personService = personService;
        _logger = logger;
    }

    /// <summary>
    /// Метод получения данных о всех Person.
    /// </summary>
    /// <returns>Возвращает данные о всех Person.</returns>
    [HttpGet]
    [Route("persons")]
    public ActionResult<List<PersonResponse>> GetAll() => _personService.GetAll();

    /// <summary>
    /// Метод получения данных конретного Person.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Возвращает данные конретного Person.</returns>
    [HttpGet]
    [Route("person/{id}")]
    public ActionResult<PersonResponse> Get(int id)
    {
        try
        {
            PersonResponse person = _personService.Get(id);
            return person;
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound();
        }
    }

    /// <summary>
    /// Метод добавления нового Person.
    /// </summary>
    /// <param name="personRequest">Данные Person для добавления.</param>
    /// <returns>Статус выполнения запроса.</returns>
    [HttpPost]
    [Route("person")]
    public IActionResult Create(PersonRequest personRequest)
    {
        try
        {
            PersonResponse personResponse = _personService.Add(personRequest);
            return CreatedAtAction(nameof(Get), new { id = personResponse.Id }, personResponse);
        }
        catch (ModelException ex)
        {
            var messages = ex.Errors.Select(x => x.ErrorMessage).ToList();
            messages.ForEach(x => _logger.LogError(x));
            return new BadRequestObjectResult(messages);
        }
        catch (DuplicateException ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }

    /// <summary>
    /// Метод обновления данных Person.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="personRequest">Данные Person для обновления.</param>
    /// <returns>Статус выполнения запроса.</returns>
    [HttpPut]
    [Route("person/{id}")]
    public IActionResult Update(int id, PersonRequest personRequest)
    {
        try
        {
            _personService.Update(id, personRequest);
            return NoContent();
        }
        catch (ModelException ex)
        {
            var messages = ex.Errors.Select(x => x.ErrorMessage).ToList();
            messages.ForEach(x => _logger.LogError(x));
            return new BadRequestObjectResult(messages);
        }
        catch (DuplicateException ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex.Message);
            return new NotFoundResult();
        }
    }

    /// <summary>
    /// Метод удаления Person.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Статус выполнения запроса.</returns>
    [HttpDelete]
    [Route("person/{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _personService.Delete(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound();
        }
    }
}