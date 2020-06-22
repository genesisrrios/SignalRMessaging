using Domain.Persistance;
using System.Threading.Tasks;

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
    }
}
