﻿namespace HallOfFame.Models;

public class Person
{
    public long? Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public List<Skills> Skills { get; set; }
}