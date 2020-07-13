using Domain.Helpers;
using Domain.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public string generateUserName()
        {
            var random = new Random();
            var animalList = new AnimalsList();
            var colorList = new ColorsList();
            var animalListCount = animalList.Animals.Count;
            var colorListCount = colorList.Colors.Count;
            var animalListIndex = random.Next(0, animalListCount);
            var colorListIndex = random.Next(0, colorListCount);
            var color = colorList.Colors[colorListIndex];
            var animal = animalList.Animals[animalListIndex];
            return $"{color}  {animal}";
        }

        public async Task<List<User>> GetUserByName(string contactName)
        {
            List<User> results = new List<User>();
            try
            {
                using (var context = new Context())
                {
                    results = await context.Users.Where(x=>x.UserName.Contains(contactName)).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                
            }
            return results;
        }
        public async Task<List<User>> GetUserById(Guid id)
        {
            List<User> results = new List<User>();
            try
            {
                using (var context = new Context())
                {
                    results = await context.Users.Where(x => x.Id == id).ToListAsync();
                }
            }
            catch (Exception ex)
            {

            }
            return results;
        }
    }
}