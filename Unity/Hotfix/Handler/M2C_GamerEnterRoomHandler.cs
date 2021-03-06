﻿using UnityEngine;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_GamerEnterRoomHandler : AMHandler<M2C_GamerEnterRoom>
    {
        protected override void Run(Session session, M2C_GamerEnterRoom message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            UIRoomComponent uiRoomComponent = uiRoom.GetComponent<UIRoomComponent>();
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();

            //从匹配状态中切换为准备状态
            if (uiRoomComponent.Matching)
            {
                uiRoomComponent.Matching = false;
                GameObject matchPrompt = uiRoom.GameObject.Get<GameObject>("MatchPrompt");
                if (matchPrompt.activeSelf)
                {
                    matchPrompt.SetActive(false);
                    uiRoom.GameObject.Get<GameObject>("ReadyButton").SetActive(true);
                }
            }

            int localGamerIndex = message.Gamers.FindIndex(info => info.UserID == gamerComponent.LocalGamer.UserID);
            //添加未显示玩家
            for (int i = 0; i < message.Gamers.Count; i++)
            {
                GamerInfo gamerInfo = message.Gamers[i];
                if (gamerInfo.UserID == 0)
                    continue;
                if (gamerComponent.Get(gamerInfo.UserID) == null)
                {
                    Gamer gamer = GamerFactory.Create(gamerInfo.UserID, gamerInfo.IsReady);
                    if ((localGamerIndex + 1) % 3 == i)
                    {
                        //玩家在本地玩家右边
                        uiRoomComponent.AddGamer(gamer, 2);
                    }
                    else
                    {
                        //玩家在本地玩家左边
                        uiRoomComponent.AddGamer(gamer, 0);
                    }
                }
            }
        }
    }
}
