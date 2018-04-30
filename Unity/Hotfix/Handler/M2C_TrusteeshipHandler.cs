using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_Trusteeship_NttHandler : AMHandler<C2M_Trusteeship>
    {
        protected override void Run(Session session, C2M_Trusteeship message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            GamerComponent gamerComponent = uiRoom.GetComponent<GamerComponent>();
            Gamer gamer = gamerComponent.Get(message.UserID);
            if (gamer.UserID == ClientComponent.Instance.LocalPlayer.UserID)
            {
                UIInteractionComponent interaction = uiRoom.GetComponent<UIRoomComponent>().Interaction;
                interaction.isTrusteeship = message.isTrusteeship;
            }
        }
    }
}
