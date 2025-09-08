using ProtoBuf;

using Vintagestory.API.MathTools;

namespace VSTweaks.Networking.Packets {
	[ProtoContract]
	internal class WaypointTeleportPacket {
		[ProtoMember(1)]
		public BlockPos Pos;
	}
}
