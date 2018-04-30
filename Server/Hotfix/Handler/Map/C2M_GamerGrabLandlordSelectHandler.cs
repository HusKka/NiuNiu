﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class C2M_GamerGrabLandlordSelectHandler : AMActorHandler<Gamer, C2M_GamerGrabLandlordSelect>
    {
        protected override async Task Run(Gamer gamer, C2M_GamerGrabLandlordSelect message)
        {
            Room room = Game.Scene.GetComponent<RoomComponent>().Get(gamer.RoomID);
            OrderControllerComponent orderController = room.GetComponent<OrderControllerComponent>();
            GameControllerComponent gameController = room.GetComponent<GameControllerComponent>();
            if (orderController.CurrentAuthority == gamer.UserID)
            {
                //保存抢地主状态
                orderController.GamerLandlordState[gamer.UserID] = message.IsGrab;

                if (message.IsGrab)
                {
                    orderController.Biggest = gamer.UserID;
                    gameController.Multiples *= 2;
                    room.Broadcast(new M2C_SetMultiples() { Multiples = gameController.Multiples });
                }

                //转发消息
                C2M_GamerGrabLandlordSelect transpond = new C2M_GamerGrabLandlordSelect();
                transpond.IsGrab = message.IsGrab;
                transpond.UserID = gamer.UserID;
                room.Broadcast(transpond);

                if (orderController.SelectLordIndex >= room.Count)
                {
                    /*
                     * 地主：√ 农民1：× 农民2：×
                     * 地主：× 农民1：√ 农民2：√
                     * 地主：√ 农民1：√ 农民2：√ 地主：√
                     * 
                     * */
                    if (orderController.Biggest == 0)
                    {
                        //没人抢地主则重新发牌
                        gameController.BackToDeck();
                        gameController.DealCards();

                        //发送玩家手牌
                        Gamer[] gamers = room.GetAll();
                        Dictionary<long, int> gamerCardsNum = new Dictionary<long, int>();
                        Array.ForEach(gamers, _gamer => gamerCardsNum.Add(_gamer.UserID, _gamer.GetComponent<HandCardsComponent>().GetAll().Length));
                        Array.ForEach(gamers, _gamer =>
                        {
                            ActorProxy actorProxy = _gamer.GetComponent<UnitGateComponent>().GetActorProxy();
                            actorProxy.Send(new M2C_GameStart()
                            {
                                GamerCards = _gamer.GetComponent<HandCardsComponent>().GetAll(),
                                GamerCardsNum = gamerCardsNum
                            });
                        });

                        //随机先手玩家
                        gameController.RandomFirstAuthority();
                        return;
                    }
                    else if ((orderController.SelectLordIndex == room.Count &&
                        ((orderController.Biggest != orderController.FirstAuthority.Key && !orderController.FirstAuthority.Value) ||
                        orderController.Biggest == orderController.FirstAuthority.Key)) ||
                        orderController.SelectLordIndex > room.Count)
                    {
                        gameController.CardsOnTable(orderController.Biggest);
                        return;
                    }
                }

                //当所有玩家都抢地主时先手玩家还有一次抢地主的机会
                if (gamer.UserID == orderController.FirstAuthority.Key && message.IsGrab)
                {
                    orderController.FirstAuthority = new KeyValuePair<long, bool>(gamer.UserID, true);
                }

                orderController.Turn();
                orderController.SelectLordIndex++;
                room.Broadcast(new M2C_AuthorityGrabLandlord() { UserID = orderController.CurrentAuthority });
            }
            await Task.CompletedTask;
        }
    }
}
