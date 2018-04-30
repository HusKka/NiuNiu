using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_AuthorityPlayCard_NttHandler : AMHandler<M2C_AuthorityPlayCard>
    {
        protected override void Run(Session session, M2C_AuthorityPlayCard message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();
            Gamer gamer = gamerComponent.Get(message.UserID);
            if (gamer != null)
            {
                //重置玩家提示
                gamer.GetComponent<GamerUIComponent>().ResetPrompt();

                //当玩家为先手，清空出牌
                if (message.IsFirst)
                {
                    gamer.GetComponent<HandCardsComponent>().ClearPlayCards();
                }

                //显示出牌按钮
                if (gamer.UserID == gamerComponent.LocalGamer.UserID)
                {
                    UIInteractionComponent interaction = uiRoom.GetComponent<UIRoomComponent>().Interaction;
                    interaction.IsFirst = message.IsFirst;
                    interaction.StartPlay();
                }
            }
        }
    }
}
