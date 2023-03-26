﻿using HallOfFame.Exceptions;
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

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    [Route("persons")]
    public ActionResult<List<PersonResponse>> GetAll() => _personService.GetAll();

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