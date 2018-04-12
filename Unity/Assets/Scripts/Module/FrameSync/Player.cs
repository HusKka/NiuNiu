namespace ETModel
{
    [ObjectSystem]
    public class PlayerAwakeSystem : AwakeSystem<Player, long>
    {
        public override void Awake(Player self, long id)
        {
            self.Awake(id);
        }
    }

    public sealed class Player : Entity
	{
		public long playerId { get; set; }

        public void Awake(long id)
        {
            this.playerId = id;
        }

        public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

            playerId = 0;
        }
	}
}