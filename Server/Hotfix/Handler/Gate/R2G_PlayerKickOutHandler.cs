using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class R2G_PlayerKickOutHandler : AMRpcHandler<R2G_PlayerKickOut, G2R_PlayerKickOut>
    {
        protected override async void Run(Session session, R2G_PlayerKickOut message, Action<G2R_PlayerKickOut> reply)
        {
            G2R_PlayerKickOut response = new G2R_PlayerKickOut();
            try
            {
                User user = Game.Scene.GetComponent<UserComponent>().Get(message.UserId);

                //服务端主动断开客户端连接
                long userSessionId = user.GetComponent<UnitGateComponent>().GateSessionId;
                Game.Scene.GetComponent<NetOuterComponent>().Remove(userSessionId);
                Log.Info($"将玩家{message.UserId}连接断开");

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
