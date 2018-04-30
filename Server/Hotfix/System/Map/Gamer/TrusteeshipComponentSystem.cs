using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class TrusteeshipComponentStartSystem : StartSystem<TrusteeshipComponent>
    {
        public override void Start(TrusteeshipComponent self)
        {
            self.Start();
        }
    }

    public static class TrusteeshipComponentSystem
    {
        public static async void Start(this TrusteeshipComponent self)
        {
            //玩家所在房间
            Room room = Game.Scene.GetComponent<RoomComponent>().Get(self.GetParent<Gamer>().RoomID);
            OrderControllerComponent orderController = room.GetComponent<OrderControllerComponent>();
            Gamer gamer = self.GetParent<Gamer>();
            bool isStartPlayCard = false;

            while (true)
            {
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);

                if (self.IsDisposed)
                {
                    return;
                }

                if (gamer.UserID != orderController?.CurrentAuthority)
                {
                    continue;
                }

                //自动出牌开关,用于托管延迟出牌
                isStartPlayCard = !isStartPlayCard;
                if (isStartPlayCard)
                {
                    continue;
                }

                ActorProxy actorProxy = Game.Scene.GetComponent<ActorProxyComponent>().Get(gamer.Id);
                //当还没抢地主时随机抢地主
                if (gamer.GetComponent<HandCardsComponent>().AccessIdentity == Identity.None)
                {
                    int randomSelect = RandomHelper.RandomNumber(0, 2);
                    actorProxy.Send(new C2M_GamerGrabLandlordSelect() { IsGrab = randomSelect == 0 });
                    self.Playing = false;
                    continue;
                }

                //自动提示出牌
                M2C_GamerPrompt response = await actorProxy.Call(new C2M_GamerPrompt()) as M2C_GamerPrompt;
                if (response.Error > 0 || response.Cards == null)
                {
                    actorProxy.Send(new C2M_GamerDontPlay());
                }
                else
                {
                    await actorProxy.Call(new C2M_GamerPlayCard() { Cards = response.Cards });
                }
            }
        }
    }
}
