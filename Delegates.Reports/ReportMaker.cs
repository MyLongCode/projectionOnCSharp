using System.Text;

namespace Delegates.Reports;

public class ReportMaker
{
    protected Func<string, string> MakeCaption;
    protected Func<string> BeginList;
    protected Func<string, string, string> MakeItem;
    protected Func<string> EndList;
    protected Func<IEnumerable<double>, object> MakeStatistics;
    protected string Caption { get; }
	public ReportMaker(Func<string,string> MakeCaption, Func<string> BeginList
		, Func<string, string, string> MakeItem, Func<string> EndList
		, Func<IEnumerable<double>, object> MakeStatistics, string Caption)
	{
		this.MakeCaption = MakeCaption;
		this.BeginList = BeginList;
		this.MakeItem = MakeItem;
		this.EndList = EndList;
		this.MakeStatistics = MakeStatistics;
		this.Caption = Caption;
	}
	public string MakeReport(IEnumerable<Measurement> measurements)
	{
		var data = measurements.ToList();
		var result = new StringBuilder();
		result.Append(MakeCaption(Caption));
		result.Append(BeginList());
		result.Append(MakeItem("Temperature", MakeStatistics(data.Select(z => z.Temperature)).ToString()));
		result.Append(MakeItem("Humidity", MakeStatistics(data.Select(z => z.Humidity)).ToString()));
		result.Append(EndList());
		return result.ToString();
	}
}

public class ReportHelper
{
    public static object GetMeanAndStd(IEnumerable<double> originalData)
    {
        var data = originalData.ToList();
        var mean = data.Average();
        var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

        return new MeanAndStd
        {
            Mean = mean,
            Std = std
        };
    }

    public static object GetMedian(IEnumerable<double> data)
    {
        var list = data.OrderBy(z => z).ToList();
        if (list.Count % 2 == 0)
            return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
        else
            return list[list.Count / 2];
    }
}

public static class ReportMakerHelper
{
	public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
	{
		var reportMaker = new ReportMaker(
            (caption) => $"<h1>{caption}</h1>",
			() => "<ul>",
			(valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
            () => "</ul>",
			ReportHelper.GetMeanAndStd,
            "Mean and Std"

            );
		return reportMaker.MakeReport(data);
	}

	public static string MedianMarkdownReport(IEnumerable<Measurement> data)
	{
        var reportMaker = new ReportMaker(
            (caption) => $"## {caption}\n\n",
            () => "",
            (valueType, entry) => $" * **{valueType}**: {entry}\n\n",
            () => "",
            ReportHelper.GetMedian,
            "Median"

            );
        return reportMaker.MakeReport(data);
    }

	public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
	{
		var reportMaker = new ReportMaker(
            (caption) => $"## {caption}\n\n",
			() => "",
            (valueType, entry) => $" * **{valueType}**: {entry}\n\n",
			() => "",
			ReportHelper.GetMeanAndStd,
			"Mean and Std"

            );
        return reportMaker.MakeReport(measurements);
    }

	public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
	{
        var reportMaker = new ReportMaker(
            (caption) => $"<h1>{caption}</h1>",
            () => "<ul>",
            (valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
            () => "</ul>",
            ReportHelper.GetMedian,
            "Median"

            );
        return reportMaker.MakeReport(measurements);
    }
}