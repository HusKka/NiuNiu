using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class C2M_GamerDontPlayHandler : AMActorHandler<Gamer, C2M_GamerDontPlay>
    {
        protected override async Task Run(Gamer gamer, C2M_GamerDontPlay message)
        {
            Room room = Game.Scene.GetComponent<RoomComponent>().Get(gamer.RoomID);
            OrderControllerComponent orderController = room.GetComponent<OrderControllerComponent>();
            if (orderController.CurrentAuthority == gamer.UserID)
            {
                //转发玩家不出牌消息
                C2M_GamerDontPlay transpond = new C2M_GamerDontPlay();
                transpond.UserID = gamer.UserID;
                room.Broadcast(transpond);

                //轮到下位玩家出牌
                orderController.Turn(false);

                //判断是否先手
                bool isFirst = orderController.CurrentAuthority == orderController.Biggest;
                if (isFirst)
                {
                    room.GetComponent<DeskCardsCacheComponent>().Clear();
                }
                room.Broadcast(new M2C_AuthorityPlayCard() { UserID = orderController.CurrentAuthority, IsFirst = isFirst });
            }
            await Task.CompletedTask;
        }
    }
}
