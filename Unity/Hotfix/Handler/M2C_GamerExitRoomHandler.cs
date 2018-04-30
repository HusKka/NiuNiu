using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_GamerExitRoomHandler : AMHandler<M2C_GamerExitRoom>
    {
        protected override void Run(Session session, M2C_GamerExitRoom message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
            UIRoomComponent uiRoomComponent = uiRoom.GetComponent<UIRoomComponent>();
            uiRoomComponent.RemoveGamer(message.UserID);
        }
    }
}
