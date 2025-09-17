using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

using VSTweaks.Networking.Handlers;

namespace VSTweaks.Commands;

internal static class BackCommand {
	public static void Register(ICoreServerAPI api) {
		api.ChatCommands.Create("back")
		   .WithDescription("Teleport to your previous location before a teleport or death.")
		   .RequiresPlayer()
		   .RequiresPrivilege(Config.Instance.BackCommandPerm)
		   .HandleWith(OnBackCommand);
	}

	private static TextCommandResult OnBackCommand(TextCommandCallingArgs args) {
		if (args.Caller.Player is not IServerPlayer serverPlayer) return TextCommandResult.Error("Error casting Player as ServerPlayer.");

		string uid = serverPlayer.PlayerUID;
		BlockPos previousPos = TeleportHandler.GetPlayerPreviousPos(uid);
		serverPlayer.Entity.TeleportTo(previousPos);

		return TextCommandResult.Success("Teleported to your previous location.");
	}
}
