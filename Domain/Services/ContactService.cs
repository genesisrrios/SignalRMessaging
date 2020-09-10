using Domain.DTOs;
using Domain.Models;
using Domain.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ContactService
    {
        public async Task<(bool, int)> AddContact(Contact contact)
        {
            try
            {
                //TODO FIX Messages for errors
                using (var context = new Context())
                {
                    var alreadyContacts = await context.Contacts.Where(x => x.ContactId == contact.ContactId && x.UserId == contact.UserId).AnyAsync();

                    if (alreadyContacts)
                        return (false, 1);
                    
                    context.Contacts.Add(new Contact
                    {
                        ContactId = contact.ContactId,
                        UserId = contact.UserId,
                        IsBlocked = false
                    });

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
            return (true, 0);
        }

        public async Task<List<ContactInformationDTO>> GetUserContacts(Guid user_id)
        {
            var contactList = new List<ContactInformationDTO>();

            try
            {
                using (var context = new Context())
                {
                    var contacts = context.Contacts.Where(x => x.UserId == user_id);

                    await contacts.ForEachAsync(async x =>
                     {
                         var lastMessage = await context.Messages
                             .Where(w => w.From == user_id && w.To == x.ContactId)
                             .Select(m=>m.Content).LastOrDefaultAsync();

                         lastMessage = lastMessage?.Substring(0, Math.Min(lastMessage.Length, 100));

                         var name = await context.Users.Where(w => w.Id == x.ContactId).Select(s => s.UserName).FirstOrDefaultAsync();

                         contactList.Add(new ContactInformationDTO
                         {
                             LastMessage = lastMessage,
                             UserId = x.ContactId,
                             UserName = name
                         });

                     });

                };
            }
            catch (Exception ex)
            {

            }
            return contactList;
        }
    }
}
