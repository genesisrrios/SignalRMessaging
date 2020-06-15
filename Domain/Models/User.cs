using System;

public class User
{
	public User()
	{
	}
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public DateTimeOffset LastTimeLogged { get; set; }
}
