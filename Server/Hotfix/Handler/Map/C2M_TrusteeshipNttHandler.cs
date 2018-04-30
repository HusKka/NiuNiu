using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class C2M_TrusteeshipNttHandler : AMActorHandler<Gamer, C2M_Trusteeship>
    {
        protected override async Task Run(Gamer gamer, C2M_Trusteeship message)
        {
            Room room = Game.Scene.GetComponent<RoomComponent>().Get(gamer.RoomID);
            //是否已经托管
            bool isTrusteeship = gamer.GetComponent<TrusteeshipComponent>() != null;
            if (message.isTrusteeship && !isTrusteeship)
            {
                gamer.AddComponent<TrusteeshipComponent>();
                Log.Info($"玩家{gamer.UserID}切换为自动模式");
            }
            else if (isTrusteeship)
            {
                gamer.RemoveComponent<TrusteeshipComponent>();
                Log.Info($"玩家{gamer.UserID}切换为手动模式");
            }

            //这里由服务端设置消息UserID用于转发
            C2M_Trusteeship transpond = new C2M_Trusteeship();
            transpond.isTrusteeship = message.isTrusteeship;
            transpond.UserID = gamer.UserID;
            //转发消息
            room.Broadcast(transpond);

            if (isTrusteeship)
            {
                OrderControllerComponent orderController = room.GetComponent<OrderControllerComponent>();
                if (gamer.UserID == orderController.CurrentAuthority)
                {
                    bool isFirst = gamer.UserID == orderController.Biggest;
                    ActorProxy actorProxy = gamer.GetComponent<UnitGateComponent>().GetActorProxy();
                    actorProxy.Send(new M2C_AuthorityPlayCard() { UserID = orderController.CurrentAuthority, IsFirst = isFirst });
                }
            }

            await Task.CompletedTask;
        }
    }
}
