﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class MP2MH_PlayerEnterRoomHandler : AMActorRpcHandler<Room, MH2MP_PlayerEnterRoom, MP2MH_PlayerEnterRoom>
    {
        protected override async Task Run(Room room, MH2MP_PlayerEnterRoom message, Action<MP2MH_PlayerEnterRoom> reply)
        {
            MP2MH_PlayerEnterRoom response = new MP2MH_PlayerEnterRoom();
            try
            {
                Gamer gamer = room.Get(message.UserID);
                if (gamer == null)
                {
                    //创建房间玩家对象
                    gamer = GamerFactory.Create(message.PlayerID, message.UserID);
                    await gamer.AddComponent<ActorComponent>().AddLocation();
                    gamer.AddComponent<UnitGateComponent, long>(message.SessionID);

                    //加入到房间
                    room.Add(gamer);

                    M2C_GamerEnterRoom broadcastMessage = new M2C_GamerEnterRoom();
                    foreach (Gamer _gamer in room.GetAll())
                    {
                        if (_gamer == null)
                        {
                            //添加空位
                            broadcastMessage.Gamers.Add(null);
                            continue;
                        }

                        //添加玩家信息
                        GamerInfo info = new GamerInfo() { UserID = _gamer.UserID, IsReady = _gamer.IsReady };
                        broadcastMessage.Gamers.Add(info);
                    }

                    //广播房间内玩家消息
                    room.Broadcast(broadcastMessage);

                    Log.Info($"玩家{message.UserID}进入房间");
                }
                else
                {
                    //玩家重连 todo处理牛牛玩家重连逻辑
                    gamer.isOffline = false;
                    gamer.PlayerID = message.PlayerID;
                    gamer.GetComponent<UnitGateComponent>().GateSessionId = message.SessionID;

                    //玩家重连移除托管组件
                    gamer.RemoveComponent<TrusteeshipComponent>();

                    M2C_GamerEnterRoom broadcastMessage = new M2C_GamerEnterRoom();
                    foreach (Gamer _gamer in room.GetAll())
                    {
                        if (_gamer == null)
                        {
                            //添加空位
                            broadcastMessage.Gamers.Add(null);
                            continue;
                        }

                        //添加玩家信息
                        GamerInfo info = new GamerInfo() { UserID = _gamer.UserID, IsReady = _gamer.IsReady };
                        broadcastMessage.Gamers.Add(info);
                    }

                    //发送房间玩家信息
                    ActorProxy actorProxy = gamer.GetComponent<UnitGateComponent>().GetActorProxy();
                    actorProxy.Send(broadcastMessage);

                    Dictionary<long, Identity> gamersIdentity = new Dictionary<long, Identity>();
                    Dictionary<long, int> gamersCardsNum = new Dictionary<long, int>();
                    GameControllerComponent gameController = room.GetComponent<GameControllerComponent>();
                    OrderControllerComponent orderController = room.GetComponent<OrderControllerComponent>();
                    DeskCardsCacheComponent deskCardsCache = room.GetComponent<DeskCardsCacheComponent>();

                    foreach (Gamer _gamer in room.GetAll())
                    {
                        HandCardsComponent handCards = _gamer.GetComponent<HandCardsComponent>();
                        gamersCardsNum.Add(_gamer.UserID, handCards.CardsCount);
                        gamersIdentity.Add(_gamer.UserID, handCards.AccessIdentity);
                    }

                    //发送游戏开始消息
                    M2C_GameStart gameStartNotice = new M2C_GameStart()
                    {
                        GamerCards = gamer.GetComponent<HandCardsComponent>().GetAll(),
                        GamerCardsNum = gamersCardsNum
                    };
                    actorProxy.Send(gameStartNotice);

                    Card[] lordCards = null;
                    if (gamer.GetComponent<HandCardsComponent>().AccessIdentity == Identity.None)
                    {
                        //广播先手玩家
                        actorProxy.Send(new M2C_AuthorityGrabLandlord() { UserID = orderController.CurrentAuthority });
                    }
                    else
                    {
                        if (gamer.UserID == orderController.CurrentAuthority)
                        {
                            //发送可以出牌消息
                            bool isFirst = gamer.UserID == orderController.Biggest;
                            actorProxy.Send(new M2C_AuthorityPlayCard() { UserID = orderController.CurrentAuthority, IsFirst = isFirst });
                        }
                        lordCards = deskCardsCache.LordCards.ToArray();
                    }
                    //发送重连数据
                    M2C_GamerReconnect_ANtt reconnectNotice = new M2C_GamerReconnect_ANtt()
                    {
                        Multiples = room.GetComponent<GameControllerComponent>().Multiples,
                        GamersIdentity = gamersIdentity,
                        DeskCards = new KeyValuePair<long, Card[]>(orderController.Biggest, deskCardsCache.library.ToArray()),
                        LordCards = lordCards,
                        GamerGrabLandlordState = orderController.GamerLandlordState
                    };
                    actorProxy.Send(reconnectNotice);

                    Log.Info($"玩家{message.UserID}重连");
                }

                response.GamerID = gamer.Id;

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
