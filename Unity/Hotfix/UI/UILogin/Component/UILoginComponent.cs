using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiLoginComponentSystem : AwakeSystem<UILoginComponent>
	{
		public override void Awake(UILoginComponent self)
		{
			self.Awake();
		}
	}
	
	public class UILoginComponent: Component
	{
		private GameObject weixinBtn;
		private GameObject visitorBtn;

        private bool isLogining;

        public void Awake()
		{
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            weixinBtn = rc.Get<GameObject>("WeiXinBtn");
            weixinBtn.GetComponent<Button>().onClick.Add(OnWeiXinLogin);
            visitorBtn = rc.Get<GameObject>("VisitorBtn");
            visitorBtn.GetComponent<Button>().onClick.Add(OnVisitorLogin);
        }

        public void OnWeiXinLogin()
        {
            Log.Info("OnWeiXinLogin!");
        }

		public async void OnVisitorLogin()
		{
            if (isLogining || this.IsDisposed)
            {
                return;
            }
            NetOuterComponent netOuterComponent = ETModel.Game.Scene.GetComponent<NetOuterComponent>();

            //设置登录中状态
            isLogining = true;
            SessionWrap sessionWrap = null;
            try
			{
                string macAddress = SystemInfo.deviceUniqueIdentifier;
                string password = "VisitorPassword";

				IPEndPoint connetEndPoint = NetworkHelper.ToIPEndPoint(GlobalConfigComponent.Instance.GlobalProto.Address);
                Session session = netOuterComponent.Create(connetEndPoint);
                sessionWrap = new SessionWrap(session);
                sessionWrap.session.GetComponent<SessionCallbackComponent>().DisposeCallback += s =>
                {
                    if (Game.Scene.GetComponent<UIComponent>()?.Get(UIType.UILogin) != null)
                    {
                        isLogining = false;
                    }
                };

                R2C_Login r2CLogin = (R2C_Login)await sessionWrap.Call(new C2R_Login() { Account = macAddress, Password = password });
				sessionWrap.Dispose();

                if (this.IsDisposed)
                {
                    return;
                }

                if (r2CLogin.Error == ErrorCode.ERR_AccountOrPasswordError)
                {
                    return;
                }

                connetEndPoint = NetworkHelper.ToIPEndPoint(r2CLogin.Address);
				Session gateSession = netOuterComponent.Create(connetEndPoint);
                Game.Scene.AddComponent<SessionWrapComponent>().Session = new SessionWrap(gateSession);
                //SessionWeap添加连接断开组件，用于处理客户端连接断开
                Game.Scene.GetComponent<SessionWrapComponent>().Session.AddComponent<SessionOfflineComponent>();
                ETModel.Game.Scene.AddComponent<SessionComponent>().Session = gateSession;
				G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await SessionWrapComponent.Instance.Session.Call(new C2G_LoginGate() { Key = r2CLogin.Key });
                if (g2CLoginGate.Error == ErrorCode.ERR_ConnectGateKeyError)
                {
                    Game.Scene.GetComponent<SessionWrapComponent>().Session.Dispose();
                    return;
                }

                Log.Info("登陆gate成功!");

                // 创建Player
                Player player = ETModel.ComponentFactory.CreateWithId<Player, long>(g2CLoginGate.PlayerId, g2CLoginGate.UserId);
                PlayerComponent playerComponent = ETModel.Game.Scene.GetComponent<PlayerComponent>();
                playerComponent.MyPlayer = player;

                Game.Scene.GetComponent<UIComponent>().Create(UIType.UILobby);
				Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILogin);
			}
			catch (Exception e)
			{
				sessionWrap?.Dispose();
				Log.Error(e);
			}
            finally
            {
                //断开验证服务器的连接
                netOuterComponent.Remove(sessionWrap.session.Id);
                //设置登录处理完成状态
                isLogining = false;
            }
        }
	}
}
