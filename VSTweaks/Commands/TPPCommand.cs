using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VSTweaks.Commands {
	internal static class TPPCommand {
		public static void Register(ICoreServerAPI api) {
			api.ChatCommands.Create("tpp")
			   .WithDescription("Teleport to a player.")
			   .WithArgs(new OnlinePlayerArgParser("destination", api, true))
			   .RequiresPlayer()
			   .RequiresPrivilege(Config.Instance.TPPCommandPerm)
			   .HandleWith(OnTeleportCommand);
		}

		private static TextCommandResult OnTeleportCommand(TextCommandCallingArgs args) {
			if (args.Parsers == null || args.Parsers[0] == null) return TextCommandResult.Error("Parser missing.");
			if (args.Parsers[0].GetValue() == null) return TextCommandResult.Error("Player value missing.");
			if (args.Parsers[0].GetValue() is not IPlayer destination) return TextCommandResult.Error("Error casting Parser as Player.");

			args.Caller.Player.Entity.TeleportTo(destination.Entity.Pos);

			return TextCommandResult.Success($"Teleported to {destination.PlayerName}.");
		}
	}
}
