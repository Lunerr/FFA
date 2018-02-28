using Discord;
using FFA.Common;
using FFA.Database.Models;
using FFA.Extensions.Database;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FFA.Services
{
    public sealed class LeaderboardService
    {
        private readonly IMongoCollection<User> _userCollection;

        public LeaderboardService(IMongoDatabase db)
        {
            _userCollection = db.GetCollection<User>("users");
        }

        // TODO: variable amount of users in the leaderboards provided in command!
        public async Task<string> GetUserLbAsync<TKey>(IGuild guild, Func<User, TKey> keySelector, bool ascending = false)
        {
            var dbUsers = await _userCollection.WhereAsync(x => x.GuildId == guild.Id);
            var ordered = ascending ? dbUsers.OrderBy(keySelector) : dbUsers.OrderByDescending(keySelector);
            var orderedArr = ordered.ToArray();
            var desc = string.Empty;
            var pos = 1;

            for (int i = 0; i < orderedArr.Length; i++)
            {
                var user = await guild.GetUserAsync(orderedArr[i].UserId);

                if (user != null)
                    desc += $"{(pos++)}. **{user}:** {orderedArr[i].Reputation}\n";

                if (pos == Config.LB_COUNT)
                    break;
            }

            return desc;
        }
    }
}