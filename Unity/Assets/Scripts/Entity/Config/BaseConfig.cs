namespace ETModel
{
	[Config(AppType.Client)]
	public partial class BaseConfigCategory : ACategory<BaseConfig>
	{
	}

	public class BaseConfig: IConfig
	{
		public long Id { get; set; }
		public int Value1;
		public int Value2;
		public int Value3;
	}
}
