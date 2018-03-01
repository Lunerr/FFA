using Discord;
using Discord.WebSocket;
using FFA.Common;
using FFA.Services;
using FFA.Utility;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FFA.Events
{
    public sealed class Ready
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly LoggingService _logger;

        public Ready(IServiceProvider provider)
        {
            _provider = provider;
            _client = provider.GetRequiredService<DiscordSocketClient>();
            _logger = provider.GetRequiredService<LoggingService>();

            _client.Ready += OnReadyAsync;
        }

        private Task OnReadyAsync()
        {
            Task.Run(async () =>
            {
                try
                {
                    Loader.LoadTimers(_provider);

                    await _client.SetGameAsync(Config.GAME);
                }
                catch (Exception ex)
                {
                    await _logger.LogAsync(LogSeverity.Error, ex.ToString());
                }
            });

            return Task.CompletedTask;
        }
    }
}
