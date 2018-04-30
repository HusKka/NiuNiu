using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_AuthorityGrabLandlordHandler : AMHandler<M2C_AuthorityGrabLandlord>
    {
        protected override void Run(Session session, M2C_AuthorityGrabLandlord message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();

            if (message.UserID == gamerComponent.LocalGamer.UserID)
            {
                //显示抢地主交互
                uiRoom.GetComponent<UIRoomComponent>().Interaction.StartGrab();
            }
        }
    }
}
