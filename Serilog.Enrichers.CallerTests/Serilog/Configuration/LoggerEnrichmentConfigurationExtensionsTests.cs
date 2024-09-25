using System.Diagnostics;

namespace Serilog
{
	using Core;
	using Events;
	using Serilog.Formatting;
	using Serilog.Parsing;
	using System.Collections.Generic;

	namespace Sinks
	{
		internal sealed class OutputTemplateRenderer(MessageTemplate messageTemplate, IFormatProvider? formatProvider) : ITextFormatter
		{
			public void Format(LogEvent logEvent, TextWriter output)
			{
				foreach (MessageTemplateToken token in messageTemplate.Tokens)
					token.Render(logEvent.Properties, output, formatProvider);
			}
		}

		internal sealed class TraceSink(OutputTemplateRenderer formatter) : ILogEventSink
		{
			public void Emit(LogEvent logEvent)
			{
				using StringWriter sw = new StringWriter();
				formatter.Format(logEvent, sw);
				Trace.Write(sw.ToString());
			}
		}
	}

	namespace Configuration
	{
		using Serilog.Parsing;
		using Sinks;

		internal static class LoggerSinkConfigurationExtensions
		{
			public static LoggerConfiguration Trace(this LoggerSinkConfiguration sinkConfiguration, string outputTemplate, IFormatProvider? formatProvider = null)
			{
				MessageTemplate messageTemplate = new MessageTemplateParser().Parse(outputTemplate);
				return sinkConfiguration.Sink(new TraceSink(new OutputTemplateRenderer(messageTemplate, formatProvider)));
			}
		}
	}

	namespace Configuration.Tests
	{
		[TestClass]
		public class LoggerEnrichmentConfigurationExtensionsTests
		{
			[TestMethod]
			public void WithCallerTest()
			{
				using Core.Logger logger = new LoggerConfiguration()
					.Enrich.WithProperty("SourceContext", null)
					.Enrich.WithCaller()

					.WriteTo.Trace(outputTemplate: CallerEnricherOutputTemplate.Default).CreateLogger();
				logger.ForContext<LoggerEnrichmentConfigurationExtensionsTests>().Information("TEST");
			}
		}
	}

	
}