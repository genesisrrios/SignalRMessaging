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

                    var messagesTo = await (from messages in context.Messages
                                            where messages.From == userId && messages.To == contactId
                                            join users in context.Users on messages.From equals users.Id
                                            select new ConversationDTO
                                            {
                                                Message = messages.Content,
                                                MessageId = messages.Id,
                                                FromId = messages.From,
                                                ToId = messages.To,
                                                SentByMe = true,
                                                ProfilePicture = users.Icon,
                                                TimeSent = messages.TimeSent.TimeOfDay.ToString()
                                            }).ToListAsync();

                    var messagesFrom = await (from messages in context.Messages
                                            where messages.From == contactId && messages.To == userId
                                            join users in context.Users on messages.From equals users.Id
                                            select new ConversationDTO
                                            {
                                                Message = messages.Content,
                                                MessageId = messages.Id,
                                                FromId = messages.From,
                                                ToId = messages.To,
                                                SentByMe = false,
                                                ProfilePicture = users.Icon,
                                                TimeSent = messages.TimeSent.TimeOfDay.ToString()
                                            }).ToListAsync();

                    results.AddRange(messagesTo);
                    results.AddRange(messagesFrom);
                    results = results.OrderBy(x => x.TimeSent).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return results;
        }
    }
}
