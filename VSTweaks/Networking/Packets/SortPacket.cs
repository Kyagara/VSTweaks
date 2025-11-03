using ProtoBuf;

namespace VSTweaks.Networking.Packets;

[ProtoContract]
class SortPacket {
	[ProtoMember(1)]
	public string InventoryID;
}
