using FFA.Database.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FFA.Extensions.Database
{
    public static class GuildCollectionExtensions
    {
        private static UpdateDefinition<Guild> GetFactory(ulong guildId)
            => new UpdateDefinitionBuilder<Guild>()
            .SetOnInsert(x => x.GuildId, guildId)
            .SetOnInsert(x => x.CaseCount, 1u)
            .SetOnInsert(x => x.AutoMute, true)
            .SetOnInsert(x => x.MaxActions, 10u)
            .SetOnInsert(x => x.IgnoredChannelIds, new List<ulong>());

        public static Task<Guild> GetGuildAsync(this IMongoCollection<Guild> collection, ulong guildId)
            => collection.GetAsync(x => x.GuildId == guildId, GetFactory(guildId));

        public static Task UpsertGuildAsync(this IMongoCollection<Guild> collection, ulong guildId, Action<Guild> update)
            => collection.UpsertAsync(x => x.GuildId == guildId, update, GetFactory(guildId));
    }
}
