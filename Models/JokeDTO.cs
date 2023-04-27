﻿namespace XCoreAssignment.Models;

public class JokeDTO
{
    public bool Error { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public string Setup { get; set; }
    public string Delivery { get; set; }
    public Flags Flags { get; set; }
    public bool Safe { get; set; }
    public int Id { get; set; }
    public string Lang { get; set; }
}

public class Flags
{
    public bool Nsfw { get; set; }
    public bool Religious { get; set; }
    public bool Political { get; set; }
    public bool Racist { get; set; }
    public bool Sexist { get; set; }
    public bool Explicit { get; set; }
}
