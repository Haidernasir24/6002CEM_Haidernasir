namespace ChatApp.Services;
using ChatApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DatabaseService : IDatabaseService
{
    private readonly SQLiteAsyncConnection _db;
    static string databaseName = "MyAppDatabase.db";

    // Construct the full path to the database file
    string dbPath = Path.Combine(FileSystem.AppDataDirectory, databaseName);
    public DatabaseService()
    {
        _db = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitializeDatabaseAsync()
    {
        await _db.CreateTableAsync<User>();
        await _db.CreateTableAsync<ChatMessagesModel>();
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _db.Table<User>().ToListAsync();
    }

    public async Task<string> GetLastMessageForUserAsync(string userId)
    {
        var lastMessage = await _db.Table<ChatMessagesModel>()
                                   .OrderByDescending(m => m.timeStamp)
                                   .FirstOrDefaultAsync();

        return lastMessage?.text ?? "No messages";
    }

    public async Task<List<ChatMessagesModel>> GetMessagesAsync()
    {
        return await _db.Table<ChatMessagesModel>().ToListAsync();
    }

    public async Task InsertUserAsync(User user)
    {
        await _db.InsertAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _db.UpdateAsync(user);
    }

    public async Task InsertChatMessageAsync(ChatMessagesModel message)
    {
        await _db.InsertAsync(message);
    }

    public async Task UpdateChatMessageAsync(ChatMessagesModel message)
    {
        await _db.UpdateAsync(message);
    }
}

