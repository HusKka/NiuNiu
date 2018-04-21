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
            C2G_GetUserInfo c2G_GetUserInfo = new C2G_GetUserInfo() { UserID = userId };
            G2C_GetUserInfo g2C_GetUserInfo = await SessionWrapComponent.Instance.Session.Call(c2G_GetUserInfo) as G2C_GetUserInfo;

            //显示用户信息
            nameText.text = g2C_GetUserInfo.NickName;
            goldText.text = g2C_GetUserInfo.Gold.ToString();
        }

        public void OnEmptyBtnClick(GameObject go)
        {
            Log.Info($"OnEmptyBtnClick:{go.name}");
        }

        public void OnGoldPlaceBtnClick()
        {
            OnStartMatch();
        }

        /// <summary>
        /// 开始匹配按钮事件
        /// </summary>
        public async void OnStartMatch()
        {
            try
            {
                //发送开始匹配消息
                C2G_StartMatch c2G_StartMatch = new C2G_StartMatch();
                G2C_StartMatch g2C_StartMatch = await SessionWrapComponent.Instance.Session.Call(c2G_StartMatch) as G2C_StartMatch;

                if (g2C_StartMatch.Error == ErrorCode.ERR_UserGoldLessError)
                {
                    Log.Error("余额不足");
                    return;
                }

                //切换到房间界面
                UI room = Game.Scene.GetComponent<UIComponent>().Create(UIType.UIRoom);
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILobby);

                //将房间设为匹配状态
                room.GetComponent<UIRoomComponent>().Matching = true;
            }
            catch (Exception e)
            {
                Log.Error(e.ToStr());
            }
        }

    }
}
