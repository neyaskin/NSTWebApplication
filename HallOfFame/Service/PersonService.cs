﻿using System.Net.Sockets;
using Azure.Core;
using HallOfFame.Exceptions;
using HallOfFame.Interfaces;
using HallOfFame.Models;
using HallOfFame.Models.Response;
using HallOfFame.Models.Requests;
using HallOfFame.Service;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Service;

public class PersonService : IPersonService
{

    private HellOfFameContext _context; 

    public PersonService(HellOfFameContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод для получения данных о всех Person.
    /// </summary>
    /// <returns>Массив объектов типа PersonResponse.</returns>
    public List<PersonResponse> GetAll()
    {
        var persons = _context.Persons.Include(q=>q.Skills).ToList();
        List<PersonResponse> personResponses = persons.Select(q => new PersonResponse()
        {
            Id = q.Id,
            Name = q.Name, DisplayName = q.DisplayName,
            Skills = q.Skills.Select(w => new SkillsResponse() { Name = w.Name, Level = w.Level }).ToList()
        }).ToList();

        return personResponses;
    }
    
    /// <summary>
    /// Метод для получения данных конкретного Person.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Объект типа PersonResponse.</returns>
    /// <exception cref="NotFoundException"></exception>
    public PersonResponse Get(int id)
    {
        var person = _context.Persons.Include(w=>w.Skills).FirstOrDefault(p => p.Id == id);
        if (person is null)
        {
            throw new NotFoundException();
        }
        PersonResponse personResponse = new PersonResponse()
        {
            Id = person.Id,
            Name = person.Name,
            DisplayName = person.DisplayName,
            Skills = person.Skills.Select(q => new SkillsResponse()
            {
                Name = q.Name,
                Level = q.Level
            }).ToList()
        };
        return personResponse;
    } 

    /// <summary>
    /// Метод для добавления нового Person в БД
    /// </summary>
    /// <param name="personRequest"></param>
    /// <returns>Объект типа PersonResponse</returns>
    public PersonResponse Add(PersonRequest personRequest)
    {
        Person person = new Person()
        {
            Name = personRequest.Name,
            DisplayName = personRequest.DisplayName,
            Skills = personRequest.Skills.Select(q => new Skills() { Name = q.Name, Level = q.Level }).ToList()
        };

        _context.Persons.Add(person);
        _context.SaveChanges();

        PersonResponse personResponse = new PersonResponse()
        {
            Id = person.Id,
            Name = person.Name,
            DisplayName = person.DisplayName,
            Skills = person.Skills.Select(q => new SkillsResponse() { Name = q.Name, Level = q.Level }).ToList()
        };

        return personResponse;
    }
    
    /// <summary>
    /// Метод для обновления данных Person
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="personRequest">Данные для обновления.</param>
    /// <exception cref="NotFoundException"></exception>
    public void Update(int id, PersonRequest personRequest)
    {
        Person person = _context.Persons.Include(q=>q.Skills).FirstOrDefault(q => q.Id == id);
        if (person is null)
            throw new NotFoundException();
        person.Name = personRequest.Name;
        person.DisplayName = personRequest.DisplayName;
        List<Skills> newSkills = personRequest.Skills.Select(q => new Skills() { Name = q.Name, Level = q.Level }).ToList();
        person.Skills = newSkills;
        _context.SaveChanges();
    } 
    
    /// <summary>
    /// Метод для удаления Person
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <exception cref="NotFoundException"></exception>
    public void Delete(int id)
    {
        PersonResponse personResponse = Get(id);
        if (personResponse is null)
            throw new NotFoundException();
        _context.Persons.Remove(_context.Persons.First(q=>q.Id == id));
        _context.SaveChanges();
    }
}