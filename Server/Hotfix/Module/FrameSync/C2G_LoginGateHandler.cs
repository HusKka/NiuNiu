using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override async void Run(Session session, C2G_LoginGate message, Action<G2C_LoginGate> reply)
		{
			G2C_LoginGate response = new G2C_LoginGate();
			try
			{
                GateSessionKeyComponent gateSessionKeyComponent = Game.Scene.GetComponent<GateSessionKeyComponent>();
                long userId = gateSessionKeyComponent.Get(message.Key);
				if (userId == 0)
				{
					response.Error = ErrorCode.ERR_ConnectGateKeyError;
					response.Message = "Gate key验证失败!";
					reply(response);
					return;
				}

                //Key过期
                gateSessionKeyComponent.Remove(message.Key);

                //创建User对象
                User user = UserFactory.Create(userId, session.Id);
                await user.AddComponent<ActorComponent>().AddLocation();

                //添加User对象关联到Session上
                session.AddComponent<SessionUserComponent>().User = user;
                //添加消息转发组件
                await session.AddComponent<ActorComponent, string>(ActorType.GateSession).AddLocation();

                //向登录服务器发送玩家上线消息
                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);
                await realmSession.Call(new G2R_PlayerOnline() { UserId = userId, GateAppId = config.StartConfig.AppId });

                response.PlayerId = user.Id;
                response.UserId = user.UserID;
                reply(response);

				session.Send(new G2C_TestHotfixMessage() { Info = "recv hotfix message success" });
			}
			catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}