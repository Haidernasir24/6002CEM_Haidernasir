namespace ChatApp.Services;
using ChatApp.Models;
public interface IDatabaseService
{
    Task InitializeDatabaseAsync();
    Task<List<User>> GetUsersAsync();
    Task<string> GetLastMessageForUserAsync(string userId);
    Task InsertUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task InsertChatMessageAsync(ChatMessagesModel message);
    Task UpdateChatMessageAsync(ChatMessagesModel message);
}

