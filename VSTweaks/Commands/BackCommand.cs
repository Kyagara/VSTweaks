using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

using VSTweaks.Networking.Handlers;

namespace VSTweaks.Commands;

static class BackCommand {
	public static void Register(ICoreServerAPI api) {
		api.ChatCommands.Create("back")
		   .WithDescription("Teleport to your previous location before a teleport or death.")
		   .RequiresPlayer()
		   .RequiresPrivilege(Config.BackCommandPerm)
		   .HandleWith(OnBackCommand);
	}

	private static TextCommandResult OnBackCommand(TextCommandCallingArgs args) {
		if (args.Caller.Player is not IServerPlayer serverPlayer) return TextCommandResult.Error("Error casting Player as ServerPlayer.");

		string uid = serverPlayer.PlayerUID;
		EntityPos previousPos = TeleportHandler.GetPlayerPreviousPos(uid);
		if (previousPos == null) return TextCommandResult.Error("No previous position set.");

		serverPlayer.Entity.TeleportTo(previousPos);

		if (!Config.EnableFeedback) return TextCommandResult.Success();
		return TextCommandResult.Success("Teleported to your previous location.");
	}
}
