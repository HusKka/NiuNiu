using ETModel;

namespace ETHotfix
{
    [Event(ETModel.EventIdType.SelectHandCard)]
    public class SelectHandCardEvent : AEvent<Card>
    {
        public override void Run(Card card)
        {
            Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom).GetComponent<UIRoomComponent>().Interaction.SelectCard(card);
        }
    }
}
