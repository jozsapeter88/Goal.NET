using Microsoft.AspNetCore.Mvc;

namespace Backend.Services;

public interface IUserService
{
    public bool Login(string username, string password);

    public bool Register(string username, string password);

}