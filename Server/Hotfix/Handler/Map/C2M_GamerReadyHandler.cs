using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class C2M_GamerReadyHandler : AMActorHandler<Gamer, C2M_GamerReady>
    {
        protected override async Task Run(Gamer gamer, C2M_GamerReady message)
        {
            gamer.IsReady = true;

            Room room = Game.Scene.GetComponent<RoomComponent>().Get(gamer.RoomID);

            //转发玩家准备消息
            C2M_GamerReady transpond = new C2M_GamerReady();
            transpond.UserID = gamer.UserID;
            room.Broadcast(transpond);
            Log.Info($"玩家{gamer.UserID}准备");

            //检测开始游戏
            room.GetComponent<GameControllerComponent>().ReadyStartGame();

            await Task.CompletedTask;
        }
    }
}
