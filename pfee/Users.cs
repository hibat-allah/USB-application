using System;


namespace WhiteList
{

public class Users
{
    // Attributs
    public string Name { get; set; }
    public string Password { get; set; }

    // Constructeur
    public Users(string name, string password)
    {
        Name = name;
        Password = password;
    }
}
}