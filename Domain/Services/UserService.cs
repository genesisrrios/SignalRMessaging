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
            var colorList = new ColorsList();
            var colorListCount = colorList.Colors.Count;
            var colorListIndex = random.Next(0, colorListCount);
            var colorName = colorList.Colors.ElementAt(colorListIndex).Key;
            var colorHex = colorList.Colors.ElementAt(colorListIndex).Value;
            return (colorName,colorHex);
        }
        public string GenerateUserAnimal()
        {
            var random = new Random();
            var animalList = new AnimalsList();
            var animalListCount = animalList.Animals.Count;
            var animalListIndex = random.Next(0, animalListCount);
            var animal = animalList.Animals[animalListIndex];
            return animal;
        }

        public async Task<List<User>> GetUserByName(string contactName)
        {
            List<User> results = new List<User>();
            try
            {
                using (var context = new Context())
                {
                    results = await context.Users.Where(x => x.UserName.Contains(contactName)).ToListAsync();
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
        public async Task<(bool,int)> AddContact(Contact contact)
        {
            try
            {
                //TODO FIX Messages for errors
                using (var context = new Context())
                {
                    var alreadyContacts = await context.Contacts.Where(x => x.ContactId == contact.ContactId && x.UserId == contact.UserId).AnyAsync();

                    if (alreadyContacts)
                        return (false, 1);
                    // One means users are already contacts

                    var userRequestingToAddContact = await GetUserById(contact.UserId);
                    if(userRequestingToAddContact == default)
                        return (false, 2);
                    //USER DOESNT EXIST

                    var contactToAdd = await GetUserById(contact.ContactId);
                    if(contactToAdd == default)
                        return (false, 2);

                    context.Contacts.Add(new Contact
                    {
                        ContactId = contactToAdd.Id,
                        UserId = contact.UserId,
                        IsBlocked = false
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return (true,0);
        }
    }
}