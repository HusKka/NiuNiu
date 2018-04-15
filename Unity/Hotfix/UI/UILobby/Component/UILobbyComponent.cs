using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiLobbyComponentSystem : AwakeSystem<UILobbyComponent>
	{
		public override void Awake(UILobbyComponent self)
		{
			self.Awake();
		}
	}
	
	public class UILobbyComponent : Component
	{
		private Text nameText;
        private Text goldText;
        private Button addGoldBtn;
        private Button shopBtn;
        private Button taskBtn;
        private Button activityBtn;
        private Button settingBtn;
        private GameObject noticeRoot;
        private Text noticeText;
        private Button goldPlaceBtn;
        private Button hundredPlaceBtn;
        private Button personalPlaceBtn;

        public void Awake()
		{
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			nameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            goldText = rc.Get<GameObject>("GoldNumText").GetComponent<Text>();
            addGoldBtn = rc.Get<GameObject>("AddGoldBtn").GetComponent<Button>();
            shopBtn = rc.Get<GameObject>("ShopBtn").GetComponent<Button>();
            taskBtn = rc.Get<GameObject>("TaskBtn").GetComponent<Button>();
            activityBtn = rc.Get<GameObject>("ActivityBtn").GetComponent<Button>();
            settingBtn = rc.Get<GameObject>("SettingBtn").GetComponent<Button>();
            noticeRoot = rc.Get<GameObject>("NoticeRoot");
            noticeText = rc.Get<GameObject>("NoticeText").GetComponent<Text>();
            goldPlaceBtn = rc.Get<GameObject>("GoldPlaceBtn").GetComponent<Button>();
            hundredPlaceBtn = rc.Get<GameObject>("HundredPlaceBtn").GetComponent<Button>();
            personalPlaceBtn = rc.Get<GameObject>("PersonalPlaceBtn").GetComponent<Button>();

            addGoldBtn.onClick.Add(() => OnEmptyBtnClick(addGoldBtn.gameObject));

            shopBtn.onClick.Add(() => OnEmptyBtnClick(shopBtn.gameObject));
            taskBtn.onClick.Add(() => OnEmptyBtnClick(taskBtn.gameObject));
            activityBtn.onClick.Add(() => OnEmptyBtnClick(activityBtn.gameObject));
            settingBtn.onClick.Add(() => OnEmptyBtnClick(settingBtn.gameObject));

            goldPlaceBtn.onClick.Add(OnGoldPlaceBtnClick);
            hundredPlaceBtn.onClick.Add(() => OnEmptyBtnClick(hundredPlaceBtn.gameObject));
            personalPlaceBtn.onClick.Add(() => OnEmptyBtnClick(personalPlaceBtn.gameObject));

            ApplyUserInfo();
        }

        public async void ApplyUserInfo()
        {
            //获取玩家数据
            long userId = ClientComponent.Instance.LocalPlayer.UserID;
            C2G_GetUserInfo c2G_GetUserInfo_Req = new C2G_GetUserInfo() { UserID = userId };
            G2C_GetUserInfo g2C_GetUserInfo_Ack = await SessionWrapComponent.Instance.Session.Call(c2G_GetUserInfo_Req) as G2C_GetUserInfo;

            //显示用户信息
            rc.Get<GameObject>("NickName").GetComponent<Text>().text = g2C_GetUserInfo_Ack.NickName;
            rc.Get<GameObject>("Money").GetComponent<Text>().text = g2C_GetUserInfo_Ack.Money.ToString();
        }

        public void OnEmptyBtnClick(GameObject go)
        {
            Log.Info($"OnEmptyBtnClick:{go.name}");
        }

        public void OnGoldPlaceBtnClick()
        {
        }


    }
}
