using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_Gameover_NttHandler : AMHandler<M2C_Gameover>
    {
        protected override void Run(Session session, M2C_Gameover message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();
            Identity localGamerIdentity = gamerComponent.LocalGamer.GetComponent<HandCardsComponent>().AccessIdentity;
            UI uiEndPanel = UIEndFactory.Create(UIType.UIEnd, uiRoom, message.Winner == localGamerIdentity);
            UIEndComponent uiEndComponent = uiEndPanel.GetComponent<UIEndComponent>();

            foreach (var gamer in gamerComponent.GetAll())
            {
                gamer.GetComponent<GamerUIComponent>().UpdatePanel();
                gamer.GetComponent<HandCardsComponent>().Hide();
                uiEndComponent.CreateGamerContent(
                    gamer,
                    message.Winner,
                    message.BasePointPerMatch,
                    message.Multiples,
                    message.GamersScore[gamer.UserID]);
            }

            UIRoomComponent uiRoomComponent = uiRoom.GetComponent<UIRoomComponent>();
            uiRoomComponent.Interaction.Gameover();
            uiRoomComponent.ResetMultiples();
        }
    }
}
