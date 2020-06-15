using System;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Guid From { get; set; }
    public Guid To { get; set; }
    public bool Read { get; set; }
}
