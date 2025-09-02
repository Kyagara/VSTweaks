using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VSTweaks.Commands
{
    internal static class SetupCommands
    {
        public static void RegisterServerCommands(ICoreServerAPI api)
        {
            api.ChatCommands.Create("tpp")
               .WithDescription("Teleport yourself to a player.")
               .WithArgs(new OnlinePlayerArgParser("destination", api, true))
               .RequiresPlayer()
               .RequiresPrivilege(Privilege.tp)
               .HandleWith(OnTeleportCommand);
        }

        private static TextCommandResult OnTeleportCommand(TextCommandCallingArgs args)
        {
            var destination = args.Parsers[0].GetValue() as IPlayer;

            args.Caller.Player.Entity.TeleportTo(destination.Entity.Pos);

            return TextCommandResult.Success("Teleported to " + destination.PlayerName);
        }
    }
}