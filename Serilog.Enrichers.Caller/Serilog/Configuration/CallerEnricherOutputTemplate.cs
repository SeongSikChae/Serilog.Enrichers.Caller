namespace Serilog.Configuration
{
	/// <summary>
	/// CallerEnricher 에 대한 기본 템플릿
	/// </summary>
	public static class CallerEnricherOutputTemplate
	{
		/// <summary>
		/// CallerEnricher 에 대한 기본 템플릿
		/// </summary>
		public const string Default = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}.{MethodName}:{LineNumber}] {Message}{NewLine}{Exception}";
	}
}
