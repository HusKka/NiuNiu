﻿using ETModel;
using System.Net;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_ReturnLobbyHandler : AMHandler<C2G_ReturnLobby>
    {
        protected override async void Run(Session session, C2G_ReturnLobby message)
        {
            //验证Session
            if (!GateHelper.SignSession(session))
            {
                return;
            }

            User user = session.GetComponent<SessionUserComponent>().User;
            StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
            ActorProxyComponent actorProxyComponent = Game.Scene.GetComponent<ActorProxyComponent>();

            //正在匹配中发送玩家退出匹配请求
            if (user.IsMatching)
            {
                IPEndPoint matchIPEndPoint = config.MatchConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session matchSession = Game.Scene.GetComponent<NetInnerComponent>().Get(matchIPEndPoint);
                await matchSession.Call(new G2M_PlayerExitMatch() { UserID = user.UserID });

                user.IsMatching = false;
            }

            //正在游戏中发送玩家退出房间请求
            if (user.ActorID != 0)
            {
                ActorProxy actorProxy = actorProxyComponent.Get(user.ActorID);
                await actorProxy.Call(new G2M_PlayerExitRoom() { UserID = user.UserID });

                user.ActorID = 0;
            }
        }
    }
}
