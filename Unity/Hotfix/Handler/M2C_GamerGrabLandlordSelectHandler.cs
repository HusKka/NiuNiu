using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_GamerGrabLandlordSelectHandler : AMHandler<C2M_GamerGrabLandlordSelect>
    {
        protected override void Run(Session session, C2M_GamerGrabLandlordSelect message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();
            Gamer gamer = gamerComponent.Get(message.UserID);
            if (gamer != null)
            {
                if (gamer.UserID == gamerComponent.LocalGamer.UserID)
                {
                    uiRoom.GetComponent<UIRoomComponent>().Interaction.EndGrab();
                }
                gamer.GetComponent<GamerUIComponent>().SetGrab(message.IsGrab);
            }
        }
    }
}
