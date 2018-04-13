using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class G2R_PlayerOfflineHandler : AMRpcHandler<G2R_PlayerOffline, R2G_PlayerOffline>
    {
        protected override void Run(Session session, G2R_PlayerOffline message,Action<R2G_PlayerOffline> reply)
        {
            R2G_PlayerOffline response = new R2G_PlayerOffline();
            try
            {
                //玩家下线
                Game.Scene.GetComponent<OnlineComponent>().Remove(message.UserID);
                Log.Info($"玩家{message.UserID}下线");

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
