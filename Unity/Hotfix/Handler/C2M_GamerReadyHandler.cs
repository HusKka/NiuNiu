using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class C2M_GamerReadyHandler : AMHandler<C2M_GamerReady>
    {
        protected override void Run(Session session, C2M_GamerReady message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();
            Gamer gamer = gamerComponent.Get(message.UserID);
            gamer.GetComponent<GamerUIComponent>().SetReady();

            //本地玩家准备,隐藏准备按钮
            if (gamer.UserID == gamerComponent.LocalGamer.UserID)
            {
                uiRoom.GetComponent<UIRoomComponent>().readyButton.SetActive(false);
            }
        }
    }
}
