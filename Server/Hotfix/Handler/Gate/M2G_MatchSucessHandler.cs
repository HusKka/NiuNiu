using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Gate)]
    public class M2G_MatchSucessHandler : AMActorHandler<User, M2G_MatchSucess>
    {
        protected override async Task Run(User user, M2G_MatchSucess message)
        {
            user.IsMatching = false;
            user.ActorID = message.GamerID;
            Log.Info($"玩家{user.UserID}匹配成功");

            await Task.CompletedTask;
        }
    }
}
