using System;
using ETModel;
using System.Net;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_StartMatchHandler : AMRpcHandler<C2G_StartMatch, G2C_StartMatch>
    {
        protected override async void Run(Session session, C2G_StartMatch message, Action<G2C_StartMatch> reply)
        {
            G2C_StartMatch response = new G2C_StartMatch();
            try
            {
                //验证Session
                if (!GateHelper.SignSession(session))
                {
                    response.Error = ErrorCode.ERR_SignError;
                    reply(response);
                    return;
                }

                User user = session.GetComponent<SessionUserComponent>().User;

                //验证玩家是否符合进入房间要求,默认为100底分局
                RoomConfig roomConfig = Game.Scene.GetComponent<ConfigComponent>().Get<RoomConfig>(1);
                UserInfo userInfo = await Game.Scene.GetComponent<DBProxyComponent>().Query<UserInfo>(user.UserID, false);
                if (userInfo.Gold < roomConfig.MinThreshold)
                {
                    response.Error = ErrorCode.ERR_UserGoldLessError;
                    reply(response);
                    return;
                }

                //这里先发送响应，让客户端收到后切换房间界面，否则可能会出现重连消息在切换到房间界面之前发送导致重连异常
                reply(response);

                //向匹配服务器发送匹配请求
                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                IPEndPoint matchIPEndPoint = config.MatchConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session matchSession = Game.Scene.GetComponent<NetInnerComponent>().Get(matchIPEndPoint);
                M2G_PlayerEnterMatch m2G_PlayerEnterMatch = await matchSession.Call(new G2M_PlayerEnterMatch()
                {
                    PlayerID = user.Id,
                    UserID = user.UserID,
                    SessionID = session.Id,
                }) as M2G_PlayerEnterMatch;

                user.IsMatching = true;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
