using Domain.Helpers;
using Domain.Models;
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
    public User GetUser(string username)
    {
        var user = new User();
        try
        {
            using (var context = new Context())
            {
                user = context.Users.Where(x => x.UserName == username).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
        }
        return user;
    }
    public async Task<User> GetUser(Guid userId)
    {
        var user = new User();
        try
        {
            using (var context = new Context())
            {
                user = await context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync();
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

        public (string,string) GenerateUserColor()
        {
            var random = new Random();
            var colorList = new ColorList();
            var colorListCount = colorList.Colors.Count;
            var colorListIndex = random.Next(0, colorListCount);
            var colorName = colorList.Colors.ElementAt(colorListIndex).Key;
            var colorHex = colorList.Colors.ElementAt(colorListIndex).Value;
            return (colorName,colorHex);
        }
        public string GenerateUserAnimal()
        {
            var random = new Random();
            var animalList = new AnimalList();
            var animalListCount = animalList.Animals.Count;
            var animalListIndex = random.Next(0, animalListCount);
            var animal = animalList.Animals[animalListIndex];
            return animal;
        }

        public string GetUserImage()
        {
            var random = new Random();
            var images = new Images();
            var imagesListCount = images.ImageList.Count;
            var imageListIndex = random.Next(0, imagesListCount);
            var image = images.ImageList[imageListIndex];
            return image;
        }

        public async Task<List<User>> GetUserByName(string contactName, Guid userSearching)
        {
            List<User> results = new List<User>();
            try
            {
                using (var context = new Context())
                {
                    results = await context.Users.Where(x => x.UserName.Contains(contactName) && x.Id != userSearching).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                
            }
            return results;
        }
        public async Task<User> GetUserById(Guid id)
        {
            var results = new User();
            try
            {
                using (var context = new Context())
                {
                    results = await context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {

            }
            return results;
        }
    }
}