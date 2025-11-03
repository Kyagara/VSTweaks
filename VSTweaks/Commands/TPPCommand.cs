using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

using VSTweaks.Networking.Handlers;

namespace VSTweaks.Commands;

static class TPPCommand {
	public static void Register(ICoreServerAPI api) {
		api.ChatCommands.Create("tpp")
		   .WithDescription("Teleport to a player.")
		   .WithArgs(new OnlinePlayerArgParser("destination", api, true))
		   .RequiresPlayer()
		   .RequiresPrivilege(Config.TPPCommandPerm)
		   .HandleWith(OnTeleportCommand);
	}

	private static TextCommandResult OnTeleportCommand(TextCommandCallingArgs args) {
		if (args.Parsers == null || args.Parsers[0] == null) return TextCommandResult.Error("Parser missing.");
		if (args.Parsers[0].GetValue() == null) return TextCommandResult.Error("Player value missing.");
		if (args.Parsers[0].GetValue() is not IPlayer toPlayer) return TextCommandResult.Error("Error casting Parser as Player.");
		if (args.Caller.Player is not IServerPlayer fromPlayer) return TextCommandResult.Error("Error casting Player as ServerPlayer.");

		string uid = fromPlayer.PlayerUID;
		EntityPos currentPos = fromPlayer.Entity.Pos;
		TeleportHandler.UpdatePlayerPreviousPos(uid, currentPos.Copy());

		fromPlayer.Entity.TeleportTo(toPlayer.Entity.Pos);

		if (!Config.EnableFeedback) return TextCommandResult.Success();
		return TextCommandResult.Success($"Teleported to {toPlayer.PlayerName}.");
	}
}
