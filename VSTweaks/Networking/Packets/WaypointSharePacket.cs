using ProtoBuf;

using Vintagestory.API.MathTools;

namespace VSTweaks.Networking.Packets;

[ProtoContract]
class WaypointSharePacket {
	[ProtoMember(1)]
	public BlockPos Pos;

	[ProtoMember(2)]
	public string Title;

	[ProtoMember(3)]
	public string Icon;

	[ProtoMember(4)]
	public int Color;
}
