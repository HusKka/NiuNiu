using System;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class G2M_PlayerExitRoomHandler : AMActorRpcHandler<Gamer, G2M_PlayerExitRoom, M2G_PlayerExitRoom>
    {
        protected override async Task Run(Gamer gamer, G2M_PlayerExitRoom message, Action<M2G_PlayerExitRoom> reply)
        {
            M2G_PlayerExitRoom response = new M2G_PlayerExitRoom();
            try
            {
                Room room = Game.Scene.GetComponent<RoomComponent>().Get(gamer.RoomID);
                if (room.State == RoomState.Game)
                {
                    gamer.isOffline = true;
                    //玩家断开添加自动出牌组件
                    if (gamer.GetComponent<TrusteeshipComponent>() == null)
                        gamer.AddComponent<TrusteeshipComponent>();

                    Log.Info($"玩家{message.UserID}断开，切换为自动模式");
                }
                else
                {
                    //房间移除玩家
                    room.Remove(gamer.UserID);

                    //同步匹配服务器移除玩家
                    await MapHelper.GetMapSession().Call(new MP2MH_PlayerExitRoom() { RoomID = room.Id, UserID = gamer.UserID });

                    //消息广播给其他人
                    room.Broadcast(new M2C_GamerExitRoom() { UserID = gamer.UserID });

                    Log.Info($"Map：玩家{gamer.UserID}退出房间");
                    gamer.Dispose();
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
