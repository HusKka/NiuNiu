using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class G2R_PlayerOnlineHandler : AMRpcHandler<G2R_PlayerOnline, R2G_PlayerOnline>
    {
        protected override async void Run(Session session, G2R_PlayerOnline message,Action<R2G_PlayerOnline> reply)
        {
            R2G_PlayerOnline response = new R2G_PlayerOnline();
            try
            {
                OnlineComponent onlineComponent = Game.Scene.GetComponent<OnlineComponent>();

                //将已在线玩家踢下线
                await RealmHelper.KickOutPlayer(message.UserID);

                //玩家上线
                onlineComponent.Add(message.UserID, message.GateAppID);
                Log.Info($"玩家{message.UserID}上线");

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
