using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class MH2MP_CreateRoomHandler : AMRpcHandler<MH2MP_CreateRoom, MP2MH_CreateRoom>
    {
        protected override async void Run(Session session, MH2MP_CreateRoom message, Action<MP2MH_CreateRoom> reply)
        {
            MP2MH_CreateRoom response = new MP2MH_CreateRoom();
            try
            {
                //创建房间 todo需要加入房间牛牛逻辑
                Room room = ComponentFactory.Create<Room>();
                room.AddComponent<DeckComponent>();
                room.AddComponent<DeskCardsCacheComponent>();
                room.AddComponent<OrderControllerComponent>();
                RoomConfig roomConfig = Game.Scene.GetComponent<ConfigComponent>().Get<RoomConfig>(1);
                room.AddComponent<GameControllerComponent, RoomConfig>(roomConfig);
                await room.AddComponent<ActorComponent>().AddLocation();
                Game.Scene.GetComponent<RoomComponent>().Add(room);

                Log.Info($"创建房间{room.Id}");

                response.RoomID = room.Id;

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
