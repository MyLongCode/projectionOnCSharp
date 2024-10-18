namespace Delegates.PairsAnalysis;

public static class Analysis
{
	public static int FindMaxPeriodIndex(params DateTime[] data)
	{
		return new MaxPauseFinder().Analyze(data);
	}

	public static double FindAverageRelativeDifference(params double[] data)
	{
		return new AverageDifferenceFinder().Analyze(data);
	}
}