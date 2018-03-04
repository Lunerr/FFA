using Discord;
using FFA.Database.Models;
using FFA.Entities.Service;
using FFA.Extensions.Database;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FFA.Services
{
    public sealed class LeaderboardService : Service
    {
        private readonly IMongoCollection<User> _dbUsers;

        public LeaderboardService(IMongoCollection<User> dbUsers)
        {
            _dbUsers = dbUsers;
        }
        
        public async Task<string> GetUserLbAsync<TKey>(IGuild guild, Func<User, TKey> keySelector, int quantity, bool ascending = false)
        {
            var dbGuildUsers = await _dbUsers.WhereAsync(x => x.GuildId == guild.Id);
            var ordered = ascending ? dbGuildUsers.OrderBy(keySelector) : dbGuildUsers.OrderByDescending(keySelector);
            var orderedArr = ordered.ToArray();
            var desc = string.Empty;
            var pos = 0;

            for (int i = 0; i < orderedArr.Length; i++)
            {
                var user = await guild.GetUserAsync(orderedArr[i].UserId);

                if (user != null)
                    desc += $"{(++pos)}. **{user}:** {orderedArr[i].Reputation.ToString("F2")}\n";

                if (pos == quantity)
                    break;
            }

            return desc;
        }
    }
}
