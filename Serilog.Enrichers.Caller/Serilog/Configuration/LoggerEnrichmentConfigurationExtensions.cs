namespace Serilog.Configuration
{
	using Enrichers;

	/// <summary>
	/// CallerEnricher 기능을 추가하기 위한  LoggerEnrichmentConfiguration 확장
	/// </summary>
	public static class LoggerEnrichmentConfigurationExtensions
	{
		/// <summary>
		/// CallerEnricher 를 포함하기 위한 설정 Chain
		/// </summary>
		/// <param name="enrichmentConfiguration"></param>
		/// <returns></returns>
		public static LoggerConfiguration WithCaller(this LoggerEnrichmentConfiguration enrichmentConfiguration)
		{
			ArgumentNullException.ThrowIfNull(enrichmentConfiguration);
			return enrichmentConfiguration.With<CallerEnricher>();
		}
	}
}
