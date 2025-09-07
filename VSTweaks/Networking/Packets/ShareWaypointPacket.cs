using ProtoBuf;

using Vintagestory.API.MathTools;

namespace VSTweaks.Networking.Packets {
	[ProtoContract]
	internal class ShareWaypointPacket {
		[ProtoMember(1)]
		public BlockPos Pos;

		[ProtoMember(2)]
		public string Title;
	}
}
