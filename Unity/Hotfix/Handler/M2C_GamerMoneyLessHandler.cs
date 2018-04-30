using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_GamerMoneyLessHandler : AMHandler<M2C_GamerMoneyLess>
    {
        protected override void Run(Session session, M2C_GamerMoneyLess message)
        {
            long userId = ClientComponent.Instance.LocalPlayer.UserID;
            if (message.UserID == userId)
            {
                //余额不足时退出房间
                UI room = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom);
                room.GetComponent<UIRoomComponent>().OnQuit();
            }
        }
    }
}
