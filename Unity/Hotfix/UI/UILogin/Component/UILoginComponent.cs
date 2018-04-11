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
			SessionWrap sessionWrap = null;
			try
			{
                string macAddress = SystemInfo.deviceUniqueIdentifier;
                string password = "VisitorPassword";

				IPEndPoint connetEndPoint = NetworkHelper.ToIPEndPoint(GlobalConfigComponent.Instance.GlobalProto.Address);
				Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
				sessionWrap = new SessionWrap(session);
                R2C_Login r2CLogin = (R2C_Login)await sessionWrap.Call(new C2R_Login() { Account = macAddress, Password = password });
				sessionWrap.Dispose();

				connetEndPoint = NetworkHelper.ToIPEndPoint(r2CLogin.Address);
				Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
				Game.Scene.AddComponent<SessionWrapComponent>().Session = new SessionWrap(gateSession);
				ETModel.Game.Scene.AddComponent<SessionComponent>().Session = gateSession;
				G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await SessionWrapComponent.Instance.Session.Call(new C2G_LoginGate() { Key = r2CLogin.Key });

				Log.Info("登陆gate成功!");

				// 创建Player
				Player player = ETModel.ComponentFactory.CreateWithId<Player>(g2CLoginGate.PlayerId);
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
		}
	}
}
