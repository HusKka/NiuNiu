namespace ETModel
{
	[Config(AppType.Client)]
	public partial class RoomConfigCategory : ACategory<RoomConfig>
	{
	}

	public class RoomConfig: IConfig
	{
		public long Id { get; set; }
		public int Multiples;
		public long BasePointPerMatch;
		public long MinThreshold;
	}
}
