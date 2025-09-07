using ProtoBuf;

namespace VSTweaks.Networking.Packets {
	[ProtoContract]
	internal class SortRequestPacket {
		[ProtoMember(1)]
		public string inventoryID;
	}
}
