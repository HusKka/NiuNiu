using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    public class G2M_PlayerExitMatchHandler : AMRpcHandler<G2M_PlayerExitMatch, M2G_PlayerExitMatch>
    {
        protected override void Run(Session session, G2M_PlayerExitMatch message, Action<M2G_PlayerExitMatch> reply)
        {
            M2G_PlayerExitMatch response = new M2G_PlayerExitMatch();
            try
            {
                Matcher matcher = Game.Scene.GetComponent<MatcherComponent>().Remove(message.UserID);
                matcher?.Dispose();
                Log.Info($"玩家{message.UserID}退出匹配队列");

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
