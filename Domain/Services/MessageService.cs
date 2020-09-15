using Domain.DTOs;
using Domain.Persistance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class MessageService
    {
        /// <summary>
        /// Sends message and returns bool indicating success/failure
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> SendMessage(Message message)
        {
            using (var context = new Context())
            {
                await context.Messages.AddAsync(message);
                return await context.SaveChangesAsync() == 1;
            }
        }

        public async Task<List<ConversationDTO>> GetMessageList(Guid userId, Guid contactId)
        {
            var results = new List<ConversationDTO>();
            try
            {
                using (var context = new Context())
                {
                    var messagesTo = from messages in context.Messages
                                     where messages.From == userId || messages.To == userId
                                     join users in context.Users on messages.To equals users.Id
                                     group new { messages, users } by new
                                     {
                                         messages.TimeSent
                                     } into grouped
                                     select new ConversationDTO
                                     {
                                         Message = grouped.FirstOrDefault().messages.Content,
                                         FromId = grouped.FirstOrDefault().messages.From,
                                         ToId = grouped.FirstOrDefault().messages.To,
                                         ProfilePicture = grouped.FirstOrDefault().users.Icon,
                                         TimeSent = grouped.FirstOrDefault().messages.TimeSent.ToString()
                                     };

                    var messagesFrom = from messages in context.Messages
                                     where messages.From == userId || messages.To == userId
                                     join users in context.Users on messages.From equals users.Id
                                     group new { messages, users } by new
                                     {
                                         messages.TimeSent
                                     } into grouped
                                     select new ConversationDTO
                                     {
                                         Message = grouped.FirstOrDefault().messages.Content,
                                         FromId = grouped.FirstOrDefault().messages.From,
                                         ToId = grouped.FirstOrDefault().messages.To,
                                         ProfilePicture = grouped.FirstOrDefault().users.Icon,
                                         TimeSent = grouped.FirstOrDefault().messages.TimeSent.ToString()
                                     };

                    results.AddRange(messagesTo);
                    results.AddRange(messagesFrom);
                    results = results.OrderByDescending(x => x.TimeSent).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return results;
        }
    }
}
