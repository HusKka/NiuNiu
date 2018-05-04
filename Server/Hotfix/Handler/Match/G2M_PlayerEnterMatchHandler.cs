using System;
using ETModel;
using System.Threading.Tasks;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    public class G2M_PlayerEnterMatchHandler : AMRpcHandler<G2M_PlayerEnterMatch, M2G_PlayerEnterMatch>
    {
        protected override async void Run(Session session, G2M_PlayerEnterMatch message, Action<M2G_PlayerEnterMatch> reply)
        {
            M2G_PlayerEnterMatch response = new M2G_PlayerEnterMatch();
            try
            {
                MatchComponent matchComponent = Game.Scene.GetComponent<MatchComponent>();
                ActorProxyComponent actorProxyComponent = Game.Scene.GetComponent<ActorProxyComponent>();

                if (matchComponent.Playing.ContainsKey(message.UserID))
                {
                    //todo 重连逻辑
                    MatchRoomComponent matchRoomComponent = Game.Scene.GetComponent<MatchRoomComponent>();
                    long roomId = matchComponent.Playing[message.UserID];
                    Room room = matchRoomComponent.Get(roomId);
                    Gamer gamer = room.Get(message.UserID);

                    //重置GateActorID
                    gamer.PlayerID = message.PlayerID;

                    //重连房间
                    ActorProxy actorProxy = actorProxyComponent.Get(roomId);
                    await actorProxy.Call(new MH2MP_PlayerEnterRoom()
                    {
                        PlayerID = message.PlayerID,
                        UserID = message.UserID,
                        SessionID = message.SessionID
                    });

                    //向玩家发送匹配成功消息
                    ActorProxy gamerActorProxy = actorProxyComponent.Get(gamer.PlayerID);
                    gamerActorProxy.Send(new M2G_MatchSucess() { GamerID = gamer.Id });
                }
                else
                {
                    //创建匹配玩家
                    Matcher matcher = MatcherFactory.Create(message.PlayerID, message.UserID, message.SessionID);
                }

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
