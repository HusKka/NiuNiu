using ETModel;
using System.Collections.Generic;
using System.Net;

namespace ETHotfix
{
    [ObjectSystem]
    public class MatchComponentUpdateSystem : UpdateSystem<MatchComponent>
    {
        public override void Update(MatchComponent self)
        {
            self.Update();
        }
    }

    public static class MatchComponentSystem
    {
        public static void Update(this MatchComponent self)
        {
            while (true)
            {
                MatcherComponent matcherComponent = Game.Scene.GetComponent<MatcherComponent>();
                Queue<Matcher> matchers = new Queue<Matcher>(matcherComponent.GetAll());
                MatchRoomComponent roomManager = Game.Scene.GetComponent<MatchRoomComponent>();
                Room room = roomManager.GetReadyRoom();

                if (matchers.Count == 0)
                {
                    //当没有匹配玩家时直接结束
                    break;
                }

                if (room == null && matchers.Count >= 3)
                {
                    //当还有一桌匹配玩家且没有可加入房间时使用空房间
                    room = roomManager.GetIdleRoom();
                }

                if (room != null)
                {
                    //当有准备状态房间且房间还有空位时匹配玩家直接加入填补空位
                    while (matchers.Count > 0 && room.Count < 3)
                    {
                        self.JoinRoom(room, matcherComponent.Remove(matchers.Dequeue().UserID));
                    }
                }
                else if (matchers.Count >= 3)
                {
                    //当还有一桌匹配玩家且没有空房间时创建新房间
                    self.CreateRoom();
                    break;
                }
                else
                {
                    break;
                }

                //移除匹配成功玩家
                while (self.MatchSuccessQueue.Count > 0)
                {
                    matcherComponent.Remove(self.MatchSuccessQueue.Dequeue().UserID);
                }
            }
        }

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="self"></param>
        public static async void CreateRoom(this MatchComponent self)
        {
            if (self.CreateRoomLock)
            {
                return;
            }

            //消息加锁，避免因为延迟重复发多次创建消息
            self.CreateRoomLock = true;

            //发送创建房间消息
            IPEndPoint mapIPEndPoint = Game.Scene.GetComponent<AllotMapComponent>().GetAddress().GetComponent<InnerConfig>().IPEndPoint;
            Session mapSession = Game.Scene.GetComponent<NetInnerComponent>().Get(mapIPEndPoint);
            MP2MH_CreateRoom createRoomRE = await mapSession.Call(new MH2MP_CreateRoom()) as MP2MH_CreateRoom;

            Room room = ComponentFactory.CreateWithId<Room>(createRoomRE.RoomID);
            Game.Scene.GetComponent<MatchRoomComponent>().Add(room);

            //解锁
            self.CreateRoomLock = false;
        }

        /// <summary>
        /// 加入房间
        /// </summary>
        /// <param name="self"></param>
        /// <param name="room"></param>
        /// <param name="matcher"></param>
        public static async void JoinRoom(this MatchComponent self, Room room, Matcher matcher)
        {
            //玩家加入房间，移除匹配队列
            self.Playing[matcher.UserID] = room.Id;
            self.MatchSuccessQueue.Enqueue(matcher);

            //向房间服务器发送玩家进入请求
            ActorProxy actorProxy = Game.Scene.GetComponent<ActorProxyComponent>().Get(room.Id);
            MP2MH_PlayerEnterRoom actor_PlayerEnterRoom = await actorProxy.Call(new MH2MP_PlayerEnterRoom()
            {
                PlayerID = matcher.PlayerID,
                UserID = matcher.UserID,
                SessionID = matcher.GateSessionID
            }) as MP2MH_PlayerEnterRoom;

            Gamer gamer = GamerFactory.Create(matcher.PlayerID, matcher.UserID, actor_PlayerEnterRoom.GamerID);
            room.Add(gamer);

            //向玩家发送匹配成功消息
            ActorProxyComponent actorProxyComponent = Game.Scene.GetComponent<ActorProxyComponent>();
            ActorProxy gamerActorProxy = actorProxyComponent.Get(gamer.PlayerID);
            gamerActorProxy.Send(new M2G_MatchSucess() { GamerID = gamer.Id });
        }
    }
}
