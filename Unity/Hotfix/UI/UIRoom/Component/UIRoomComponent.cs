﻿using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIRoomComponentAwakeSystem : AwakeSystem<UIRoomComponent>
    {
        public override void Awake(UIRoomComponent self)
        {
            self.Awake();
        }
    }

    public class UIRoomComponent : Component
    {
        public GameObject quitButton;
        public GameObject readyButton;

        private UIInteractionComponent interaction;

        //private Text multiples;

        public readonly GameObject[] GamersPanel = new GameObject[5];

        public bool Matching { get; set; }

        public UIInteractionComponent Interaction
        {
            get
            {
                if (interaction == null)
                {
                    UI uiRoom = this.GetParent<UI>();
                    UI uiInteraction = UIInteractionFactory.Create(UIType.UIInteraction, uiRoom);
                    interaction = uiInteraction.GetComponent<UIInteractionComponent>();
                }
                return interaction;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.Matching = false;
            this.interaction = null;
        }

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            quitButton = rc.Get<GameObject>("QuitButton");
            readyButton = rc.Get<GameObject>("ReadyButton");
            //GameObject multiplesObj = rc.Get<GameObject>("Multiples");
            //multiples = multiplesObj.GetComponent<Text>();

            //绑定事件
            quitButton.GetComponent<Button>().onClick.Add(OnQuit);
            readyButton.GetComponent<Button>().onClick.Add(OnReady);

            //默认隐藏UI
            //multiplesObj.SetActive(false);
            readyButton.SetActive(false);
            //rc.Get<GameObject>("Desk").SetActive(false);

            //添加玩家面板
            GameObject gamersPanel = rc.Get<GameObject>("Gamers");
            this.GamersPanel[0] = gamersPanel.Get<GameObject>("Left2");
            this.GamersPanel[1] = gamersPanel.Get<GameObject>("Left1");
            this.GamersPanel[2] = gamersPanel.Get<GameObject>("Local");
            this.GamersPanel[3] = gamersPanel.Get<GameObject>("Right1");
            this.GamersPanel[4] = gamersPanel.Get<GameObject>("Right2");
            

            //添加本地玩家
            User localPlayer = ClientComponent.Instance.LocalPlayer;
            Gamer localGamer = GamerFactory.Create(localPlayer.UserID, false);
            AddGamer(localGamer, 2);
            this.GetParent<UI>().GetComponent<GamerComponent>().LocalGamer = localGamer;
        }

        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="gamer"></param>
        /// <param name="index"></param>
        public void AddGamer(Gamer gamer, int index)
        {
            GetParent<UI>().GetComponent<GamerComponent>().Add(gamer, index);
            gamer.GetComponent<GamerUIComponent>().SetPanel(this.GamersPanel[index]);
        }

        /// <summary>
        /// 移除玩家
        /// </summary>
        /// <param name="id"></param>
        public void RemoveGamer(long id)
        {
            Gamer gamer = GetParent<UI>().GetComponent<GamerComponent>().Remove(id);
            gamer.Dispose();
        }

        /// <summary>
        /// 设置倍率
        /// </summary>
        /// <param name="multiples"></param>
        public void SetMultiples(int multiples)
        {
            //this.multiples.gameObject.SetActive(true);
            //this.multiples.text = multiples.ToString();
        }

        /// <summary>
        /// 重置倍率
        /// </summary>
        public void ResetMultiples()
        {
            //this.multiples.gameObject.SetActive(false);
            //this.multiples.text = "1";
        }

        /// <summary>
        /// 退出房间
        /// </summary>
        public void OnQuit()
        {
            //发送退出房间消息
            SessionWrapComponent.Instance.Session.Send(new C2G_ReturnLobby());

            //切换到大厅界面
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UILobby);
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIRoom);
        }

        /// <summary>
        /// 准备
        /// </summary>
        private void OnReady()
        {
            //发送准备
            SessionWrapComponent.Instance.Session.Send(new C2M_GamerReady());
        }
    }
}
