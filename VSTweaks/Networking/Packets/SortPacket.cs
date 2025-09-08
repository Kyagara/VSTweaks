using ProtoBuf;

namespace VSTweaks.Networking.Packets {
	[ProtoContract]
	internal class SortPacket {
		[ProtoMember(1)]
		public string InventoryID;
	}
}
