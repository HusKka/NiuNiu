namespace ETModel
{
	[ObjectSystem]
	public class PlayerSystem : AwakeSystem<Player, long>
	{
		public override void Awake(Player self, long a)
		{
			self.Awake(a);
		}
	}

	public sealed class Player : Entity
	{
		public long playerId { get; private set; }
		
		public long UnitId { get; set; }

		public void Awake(long playerId)
		{
			this.playerId = playerId;
		}
		
		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
		}
	}
}