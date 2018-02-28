using Discord;
using Discord.Commands;

namespace FFA.Extensions.Discord
{
    internal static class CommandInfoExtensions
    {
        internal static string GetUsage(this CommandInfo command)
        {
            var usage = command.Name;

            foreach (var param in command.Parameters)
                usage += $" {(param.IsOptional ? "[" : "<")}{param.Name}{(param.IsOptional ? "]" : ">")}";

            return usage;
        }

        internal static string GetExample(this CommandInfo command)
        {
            var example = command.Name;

            foreach (var param in command.Parameters)
            {
                var before = string.Empty;

                if (typeof(IUser).IsAssignableFrom(param.Type) || typeof(IRole).IsAssignableFrom(param.Type))
                    before += "@";
                else if (typeof(ITextChannel).IsAssignableFrom(param.Type))
                    before += "#";

                example += $" {before}{param.Summary ?? param.Name}";
            }

            return example;
        }
    }
}