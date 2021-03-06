﻿using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    public class MP2MH_PlayerExitRoomHandler : AMRpcHandler<MP2MH_PlayerExitRoom, MH2MP_PlayerExitRoom>
    {
        protected override void Run(Session session, MP2MH_PlayerExitRoom message, Action<MH2MP_PlayerExitRoom> reply)
        {
            MH2MP_PlayerExitRoom response = new MH2MP_PlayerExitRoom();
            try
            {
                MatchRoomComponent matchRoomComponent = Game.Scene.GetComponent<MatchRoomComponent>();
                Room room = matchRoomComponent.Get(message.RoomID);

                //移除玩家对象
                Gamer gamer = room.Remove(message.UserID);
                Game.Scene.GetComponent<MatchComponent>().Playing.Remove(gamer.UserID);
                gamer.Dispose();

                Log.Info($"Match：同步玩家{message.UserID}退出房间");

                if (room.Count == 0)
                {
                    //当房间中没有玩家时回收
                    matchRoomComponent.Recycle(room.Id);
                    Log.Info($"回收房间{room.Id}");
                }

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
