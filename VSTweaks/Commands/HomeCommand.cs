using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VSTweaks.Commands {
	internal static class HomeCommand {
		public static void Register(ICoreServerAPI api) {
			api.ChatCommands.Create("home")
			   .WithDescription("Teleport back to your spawn point.")
			   .RequiresPlayer()
			   .RequiresPrivilege(Config.Instance.HomeCommandPerm)
			   .HandleWith(OnHomeCommand);
		}

		private static TextCommandResult OnHomeCommand(TextCommandCallingArgs args) {
			if (args.Caller.Player is not IServerPlayer serverPlayer) return TextCommandResult.Error("Error casting Player as ServerPlayer.");

			var pos = serverPlayer.GetSpawnPosition(false);

			serverPlayer.Entity.TeleportTo(pos);

			return TextCommandResult.Success("Teleported to your spawn point.");
		}
	}
}
