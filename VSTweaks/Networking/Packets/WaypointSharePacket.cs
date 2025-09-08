using ProtoBuf;

using Vintagestory.API.MathTools;

namespace VSTweaks.Networking.Packets {
	[ProtoContract]
	internal class WaypointSharePacket {
		[ProtoMember(1)]
		public BlockPos Pos;

		[ProtoMember(2)]
		public string Title;
	}
}
