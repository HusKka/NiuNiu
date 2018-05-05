﻿using UnityEngine;
using ETModel;
using UnityEngine.UI;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_GamerReconnect_ANttHandler : AMHandler<M2C_GamerReconnect_ANtt>
    {
        protected override void Run(Session session, M2C_GamerReconnect_ANtt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();

            uiRoom.GameObject.Get<GameObject>("ReadyButton").SetActive(false);
            foreach (var gamer in gamerComponent.GetAll())
            {
                //初始化玩家身份
                Identity gamerIdentity = message.GamersIdentity[gamer.UserID];
                HandCardsComponent gamerHandCards = gamer.GetComponent<HandCardsComponent>();
                GamerUIComponent gamerUI = gamer.GetComponent<GamerUIComponent>();
                gamerHandCards.AccessIdentity = gamerIdentity;
                gamerUI.SetIdentity(gamerIdentity);
                //初始化出牌
                if (message.DeskCards.Key == gamer.UserID && gamerIdentity != Identity.None)
                {
                    Card[] deskCards = message.DeskCards.Value;
                    if (deskCards != null)
                    {
                        gamerHandCards.PopCards(deskCards);
                    }
                }
                else if (message.LordCards == null && message.GamerGrabLandlordState.ContainsKey(gamer.UserID))
                {
                    gamer.GetComponent<GamerUIComponent>().SetGrab(message.GamerGrabLandlordState[gamer.UserID]);
                }
            }

            //初始化界面
            UIRoomComponent uiRoomComponent = uiRoom.GetComponent<UIRoomComponent>();
            uiRoomComponent.SetMultiples(message.Multiples);
            //当抢完地主时才能显示托管按钮
            if (message.LordCards != null)
            {
                uiRoomComponent.Interaction.GameStart();
            }

            //初始化地主牌
            if (message.LordCards != null)
            {
                GameObject lordPokers = uiRoom.GameObject.Get<GameObject>("Desk").Get<GameObject>("LordPokers");
                for (int i = 0; i < lordPokers.transform.childCount; i++)
                {
                    Sprite lordCardSprite = CardHelper.GetCardSprite(message.LordCards[i].GetName());
                    lordPokers.transform.GetChild(i).GetComponent<Image>().sprite = lordCardSprite;
                }
            }
        }
    }
}
