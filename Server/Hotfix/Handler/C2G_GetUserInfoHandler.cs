using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_GetUserInfoHandler : AMRpcHandler<C2G_GetUserInfo, G2C_GetUserInfo>
    {
        protected override async void Run(Session session, C2G_GetUserInfo message, Action<G2C_GetUserInfo> reply)
        {
            G2C_GetUserInfo response = new G2C_GetUserInfo();
            try
            {
                //验证Session
                if (!GateHelper.SignSession(session))
                {
                    response.Error = ErrorCode.ERR_SignError;
                    reply(response);
                    return;
                }

                //查询用户信息
                DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
                UserInfo userInfo = await dbProxyComponent.Query<UserInfo>(message.UserID, false);

                response.NickName = userInfo.NickName;
                response.Wins = userInfo.Wins;
                response.Loses = userInfo.Loses;
                response.Gold = userInfo.Gold;

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
