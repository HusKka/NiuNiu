using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_SetMultiplesHandler : AMHandler<M2C_SetMultiples>
    {
        protected override void Run(Session session, M2C_SetMultiples message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            uiRoom.GetComponent<UIRoomComponent>().SetMultiples(message.Multiples);
        }
    }
}
