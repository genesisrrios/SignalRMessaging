using Domain.Persistance;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Domain.Service { 
public class UserService
{
    public UserService()
    {
    }
    public User GetUser(string username,string password)
    {
        var user = new User();
        try
        {
            using (var context = new Context())
            {
                user = context.Users.Where(x => x.UserName == username && x.Password == password).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
        }
        return user;
    }
    public User GetUser(Guid userId)
    {
        var user = new User();
        try
        {
            using (var context = new Context())
            {
                user = context.Users.Where(x => x.Id == userId).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
        }
        return user;
    }
    public async Task<bool> CreateUser(User user)
    {
        bool userWasCreated = false;
        try
        {
            using (var context = new Context())
            {
                await context.Users.AddAsync(user);
                userWasCreated = await context.SaveChangesAsync() == 1;
            }
        }
        catch (Exception ex)
        {
            userWasCreated = false;
        }
        return userWasCreated;
    }
    public async Task<bool> UpdateUser(User user)
    {
        bool userWasUpdated = false;
        try
        {
            using (var context = new Context())
            {
                context.Users.Update(user);
                userWasUpdated = await context.SaveChangesAsync() == 1;
            }
        }
        catch (Exception ex)
        {
            userWasUpdated = false;
        }
        return userWasUpdated;
    }
    }
}