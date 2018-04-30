using ProtoBuf;
using ETModel;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
namespace ETModel
{
	[Message(HotfixOpcode.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: IRequest
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public string Account;

		[ProtoMember(2, IsRequired = true)]
		public string Password;

	}

	[Message(HotfixOpcode.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: IResponse
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(91, IsRequired = true)]
		public int Error { get; set; }

		[ProtoMember(92, IsRequired = true)]
		public string Message { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public string Address;

		[ProtoMember(2, IsRequired = true)]
		public long Key;

	}

	[Message(HotfixOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: IRequest
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long Key;

	}

	[Message(HotfixOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: IResponse
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(91, IsRequired = true)]
		public int Error { get; set; }

		[ProtoMember(92, IsRequired = true)]
		public string Message { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long PlayerId;

		[ProtoMember(2, IsRequired = true)]
		public long UserId;

	}

	[Message(HotfixOpcode.G2C_TestHotfixMessage)]
	[ProtoContract]
	public partial class G2C_TestHotfixMessage: IMessage
	{
		[ProtoMember(1, IsRequired = true)]
		public string Info;

	}

	[Message(HotfixOpcode.C2M_TestActorRequest)]
	[ProtoContract]
	public partial class C2M_TestActorRequest: IActorRequest
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public string Info;

	}

	[Message(HotfixOpcode.M2C_TestActorResponse)]
	[ProtoContract]
	public partial class M2C_TestActorResponse: IActorResponse
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(91, IsRequired = true)]
		public int Error { get; set; }

		[ProtoMember(92, IsRequired = true)]
		public string Message { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public string Info;

	}

	[Message(HotfixOpcode.PlayerInfo)]
	[ProtoContract]
	public partial class PlayerInfo: IMessage
	{
	}

	[Message(HotfixOpcode.C2G_PlayerInfo)]
	[ProtoContract]
	public partial class C2G_PlayerInfo: IRequest
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

	}

	[Message(HotfixOpcode.G2C_PlayerInfo)]
	[ProtoContract]
	public partial class G2C_PlayerInfo: IResponse
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(91, IsRequired = true)]
		public int Error { get; set; }

		[ProtoMember(92, IsRequired = true)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<PlayerInfo> PlayerInfos = new List<PlayerInfo>();

	}

	[Message(HotfixOpcode.C2G_GetUserInfo)]
	[ProtoContract]
	public partial class C2G_GetUserInfo: IRequest
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

	}

	[Message(HotfixOpcode.G2C_GetUserInfo)]
	[ProtoContract]
	public partial class G2C_GetUserInfo: IResponse
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(91, IsRequired = true)]
		public int Error { get; set; }

		[ProtoMember(92, IsRequired = true)]
		public string Message { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public string NickName;

		[ProtoMember(2, IsRequired = true)]
		public int Wins;

		[ProtoMember(3, IsRequired = true)]
		public int Loses;

		[ProtoMember(4, IsRequired = true)]
		public long Gold;

	}

	[Message(HotfixOpcode.C2G_StartMatch)]
	[ProtoContract]
	public partial class C2G_StartMatch: IRequest
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

	}

	[Message(HotfixOpcode.G2C_StartMatch)]
	[ProtoContract]
	public partial class G2C_StartMatch: IResponse
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(91, IsRequired = true)]
		public int Error { get; set; }

		[ProtoMember(92, IsRequired = true)]
		public string Message { get; set; }

	}

	[Message(HotfixOpcode.C2G_ReturnLobby)]
	[ProtoContract]
	public partial class C2G_ReturnLobby: IMessage
	{
	}

	#region Map-Client

	[Message(HotfixOpcode.GamerInfo)]
	[ProtoContract]
	public partial class GamerInfo
	{
		[ProtoMember(1, IsRequired = true)]
		public long UserID;

		[ProtoMember(2, IsRequired = true)]
		public bool IsReady;

	}

	[Message(HotfixOpcode.M2C_GamerEnterRoom)]
	[ProtoContract]
	public partial class M2C_GamerEnterRoom: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<GamerInfo> Gamers = new List<GamerInfo>();

	}

	[Message(HotfixOpcode.M2C_GamerExitRoom)]
	[ProtoContract]
	public partial class M2C_GamerExitRoom: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

	}

	[Message(HotfixOpcode.M2C_GameStart)]
	[ProtoContract]
	public partial class M2C_GameStart: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public Card[] GamerCards;

		[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
		[ProtoMember(2)]
		public Dictionary<long,int> GamerCardsNum = new Dictionary<long,int>();

	}

	[Message(HotfixOpcode.M2C_AuthorityGrabLandlord)]
	[ProtoContract]
	public partial class M2C_AuthorityGrabLandlord: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

	}

	[Message(HotfixOpcode.M2C_SetMultiples)]
	[ProtoContract]
	public partial class M2C_SetMultiples: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public int Multiples;

	}

	[Message(HotfixOpcode.M2C_SetLandlord)]
	[ProtoContract]
	public partial class M2C_SetLandlord: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

		[ProtoMember(2, IsRequired = true)]
		public Card[] LordCards;

	}

	[Message(HotfixOpcode.M2C_AuthorityPlayCard)]
	[ProtoContract]
	public partial class M2C_AuthorityPlayCard: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

		[ProtoMember(2, IsRequired = true)]
		public bool IsFirst;

	}

	[Message(HotfixOpcode.M2C_GamerPlayCard_Ntt)]
	[ProtoContract]
	public partial class M2C_GamerPlayCard_Ntt: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

		[ProtoMember(2, IsRequired = true)]
		public Card[] Cards;

	}

	[Message(HotfixOpcode.M2C_Gameover)]
	[ProtoContract]
	public partial class M2C_Gameover: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public Identity Winner;

		[ProtoMember(2, IsRequired = true)]
		public long BasePointPerMatch;

		[ProtoMember(3, IsRequired = true)]
		public int Multiples;

		[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
		[ProtoMember(4)]
		public Dictionary<long,long> GamersScore = new Dictionary<long,long>();

	}

	[Message(HotfixOpcode.M2C_GamerMoneyLess)]
	[ProtoContract]
	public partial class M2C_GamerMoneyLess: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

	}

	#endregion

	#region Client-Map

	[Message(HotfixOpcode.C2M_GamerReady)]
	[ProtoContract]
	public partial class C2M_GamerReady: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

	}

	[Message(HotfixOpcode.C2M_GamerGrabLandlordSelect)]
	[ProtoContract]
	public partial class C2M_GamerGrabLandlordSelect: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

		[ProtoMember(2, IsRequired = true)]
		public bool IsGrab;

	}

	[Message(HotfixOpcode.C2M_Trusteeship)]
	[ProtoContract]
	public partial class C2M_Trusteeship: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

		[ProtoMember(2, IsRequired = true)]
		public bool isTrusteeship;

	}

	[Message(HotfixOpcode.C2M_GamerPrompt)]
	[ProtoContract]
	public partial class C2M_GamerPrompt: IActorRequest
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

	}

	[Message(HotfixOpcode.M2C_GamerPrompt)]
	[ProtoContract]
	public partial class M2C_GamerPrompt: IActorResponse
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(91, IsRequired = true)]
		public int Error { get; set; }

		[ProtoMember(92, IsRequired = true)]
		public string Message { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public Card[] Cards;

	}

	[Message(HotfixOpcode.C2M_GamerDontPlay)]
	[ProtoContract]
	public partial class C2M_GamerDontPlay: IActorMessage
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public long UserID;

	}

	[Message(HotfixOpcode.C2M_GamerPlayCard)]
	[ProtoContract]
	public partial class C2M_GamerPlayCard: IActorRequest
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(93, IsRequired = true)]
		public long ActorId { get; set; }

		[ProtoMember(1, IsRequired = true)]
		public Card[] Cards;

	}

	[Message(HotfixOpcode.M2C_GamerPlayCard)]
	[ProtoContract]
	public partial class M2C_GamerPlayCard: IActorResponse
	{
		[ProtoMember(90, IsRequired = true)]
		public int RpcId { get; set; }

		[ProtoMember(91, IsRequired = true)]
		public int Error { get; set; }

		[ProtoMember(92, IsRequired = true)]
		public string Message { get; set; }

	}

	#endregion

}
