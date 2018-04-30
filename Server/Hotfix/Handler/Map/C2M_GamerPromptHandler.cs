using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class C2M_GamerPromptHandler : AMActorRpcHandler<Gamer, C2M_GamerPrompt, M2C_GamerPrompt>
    {
        protected override async Task Run(Gamer gamer, C2M_GamerPrompt message, Action<M2C_GamerPrompt> reply)
        {
            M2C_GamerPrompt response = new M2C_GamerPrompt();
            try
            {
                Room room = Game.Scene.GetComponent<RoomComponent>().Get(gamer.RoomID);
                OrderControllerComponent orderController = room.GetComponent<OrderControllerComponent>();
                DeskCardsCacheComponent deskCardsCache = room.GetComponent<DeskCardsCacheComponent>();

                List<Card> handCards = new List<Card>(gamer.GetComponent<HandCardsComponent>().GetAll());
                CardsHelper.SortCards(handCards);

                if (gamer.UserID == orderController.Biggest)
                {
                    response.Cards = handCards.Where(card => card.CardWeight == handCards[handCards.Count - 1].CardWeight).ToArray();
                }
                else
                {
                    List<Card[]> result = await CardsHelper.GetPrompt(handCards, deskCardsCache, deskCardsCache.Rule);

                    if (result.Count > 0)
                    {
                        response.Cards = result[RandomHelper.RandomNumber(0, result.Count)];
                    }
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
