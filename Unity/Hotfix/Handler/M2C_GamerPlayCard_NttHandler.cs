using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_GamerPlayCard_NttHandler : AMHandler<M2C_GamerPlayCard_Ntt>
    {
        protected override void Run(Session session, M2C_GamerPlayCard_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();
            Gamer gamer = gamerComponent.Get(message.UserID);
            if (gamer != null)
            {
                gamer.GetComponent<GamerUIComponent>().ResetPrompt();

                if (gamer.UserID == gamerComponent.LocalGamer.UserID)
                {
                    UIInteractionComponent interaction = uiRoom.GetComponent<UIRoomComponent>().Interaction;
                    interaction.Clear();
                    interaction.EndPlay();
                }

                HandCardsComponent handCards = gamer.GetComponent<HandCardsComponent>();
                handCards.PopCards(message.Cards);
            }
        }
    }
}
