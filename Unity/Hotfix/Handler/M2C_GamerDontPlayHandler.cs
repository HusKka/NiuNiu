using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_GamerDontPlayHandler : AMHandler<C2M_GamerDontPlay>
    {
        protected override void Run(Session session, C2M_GamerDontPlay message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();
            Gamer gamer = gamerComponent.Get(message.UserID);
            if (gamer != null)
            {
                if (gamer.UserID == gamerComponent.LocalGamer.UserID)
                {
                    uiRoom.GetComponent<UIRoomComponent>().Interaction.EndPlay();
                }
                gamer.GetComponent<HandCardsComponent>().ClearPlayCards();
                gamer.GetComponent<GamerUIComponent>().SetDiscard();
            }
        }
    }
}
