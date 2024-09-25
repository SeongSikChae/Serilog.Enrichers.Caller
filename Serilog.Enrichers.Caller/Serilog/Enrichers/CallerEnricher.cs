using System.Diagnostics;
using System.Reflection;

namespace Serilog.Enrichers
{
	using Core;
	using Events;

	internal sealed class CallerEnricher : ILogEventEnricher
	{
		const string SourceContextPropertyKey = "SourceContext";
		const string MethodNamePropertyKey = "MethodName";
		const string LineNumberPropertyKey = "LineNumber";
		const string SourceFilePropertyKey = "SourceFile";

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			StackTrace stackTrace;
			if (logEvent.Exception is not null)
				stackTrace = new StackTrace(logEvent.Exception);
			else
				stackTrace = new StackTrace(true);

			if (logEvent.Properties.TryGetValue(SourceContextPropertyKey, out LogEventPropertyValue? sourceContextPropertyValue))
			{
				string? sourceContext = ((ScalarValue)sourceContextPropertyValue).Value?.ToString();
				if (sourceContext is not null)
				{
					StackFrame? stackFrame = stackTrace.GetFrames().FirstOrDefault(f => IsValidStackFrame(f, sourceContext));
					if (stackFrame is not null)
					{
						MethodBase? methodBase = stackFrame.GetMethod();
						if (methodBase is not null)
							logEvent.AddPropertyIfAbsent(new LogEventProperty(MethodNamePropertyKey, new ScalarValue(methodBase.Name)));

						string? fileName = stackFrame.GetFileName();
						if (fileName is not null)
						{
							logEvent.AddPropertyIfAbsent(new LogEventProperty(SourceFilePropertyKey, new ScalarValue(fileName)));
							logEvent.AddPropertyIfAbsent(new LogEventProperty(LineNumberPropertyKey, new ScalarValue(stackFrame.GetFileLineNumber())));
						}
					}
				}
			}
		}

		private static bool IsValidStackFrame(StackFrame stackFrame, string sourceContext)
		{
			MethodBase? methodBase = stackFrame.GetMethod();
			if (methodBase is null)
				return false;
			if (methodBase.DeclaringType is null)
				return false;
			return sourceContext.Equals(methodBase.DeclaringType.FullName);
		}
	}
}
