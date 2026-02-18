using System;


namespace DataLayer.Entities;

public class Exercise : BaseModel
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
