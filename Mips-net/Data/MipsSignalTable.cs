using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mips.Data
{
	[DataContract]
   public class MipsSignalTable
    {
	    [DataMember]
	    private List<PsgPoint> timePoints;

	    private string cachedTable;

	    public MipsSignalTable()
	    {
		    timePoints = new List<PsgPoint>();
	    }

	    private MipsSignalTable(IEnumerable<PsgPoint> timePoints) : this()
	    {
		    this.timePoints.AddRange(timePoints);
	    }

	    public PsgPoint this[string pointName]
	    {
		    get
		    {
			    return timePoints.FirstOrDefault(x => x.Name == pointName);
		    }
	    }

	    public PsgPoint this[int time]
	    {
		    get
		    {
			    return timePoints.FirstOrDefault(x => x.TimePoint == time);
		    }
	    }

	    public MipsSignalTable AddTimePoint(PsgPoint point)
	    {
		    if (!timePoints.Select(x => x.TimePoint).Contains<int>(point.TimePoint))
		    {
			    timePoints.Add(point);
		    }
		    return new MipsSignalTable(timePoints);
	    }

	    public MipsSignalTable AddTimePoint(int clock, LoopData loopData)
	    {
		    if (!timePoints.Select(x => x.TimePoint).Contains(clock))
		    {
			    char[] ap = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();
			    timePoints.Add(new PsgPoint(ap[timePoints.Count].ToString(), clock, loopData));
		    }
		    return new MipsSignalTable(timePoints);
	    }

	    public MipsSignalTable RemoveTimePoint(PsgPoint point)
	    {
		    if (timePoints.Select(x => x.TimePoint).Contains<int>(point.TimePoint))
		    {
			    timePoints.Remove(point);
		    }
		    return new MipsSignalTable(timePoints);
	    }

	    public MipsSignalTable RemoveTimePoint(int clockToRemove)
	    {
		    var timePoint = timePoints.FirstOrDefault(x => x.TimePoint == clockToRemove);
		    if (timePoint != null)
		    {
			    timePoints.Remove(timePoint);
		    }
		    return new MipsSignalTable(timePoints);
	    }

	    public MipsSignalTable AddSignalTable(MipsSignalTable signalTable)
	    {
		     
		    foreach (var psgPoint in signalTable.Points)
		    {
			    var timePoint = Points.FirstOrDefault(x => x.TimePoint == psgPoint.TimePoint);
			    if (timePoint != null)
			    {
				    foreach (var dcBiasElement in timePoint.DcBiasElements)
				    {
					    if (timePoint.DcBiasElements.FirstOrDefault(x => x.Key.Equals(dcBiasElement.Key)).Key
					        == default(string))
					    {
						    timePoint.CreateOutput(dcBiasElement.Key, dcBiasElement.Value);
					    }

				    }

				    foreach (var digitalOutputElement in timePoint.DigitalOutputElements)
				    {
					    if (
						    timePoint.DigitalOutputElements.FirstOrDefault(
							    x => x.Key.Equals(digitalOutputElement.Key)).Key == default(string))
					    {
						    timePoint.CreateOutput(digitalOutputElement.Key, digitalOutputElement.Value);
					    }

				    }
			    }
			    else
			    {
				    // Time point doesn't exist, add it to the signal table. 
				    AddTimePoint(psgPoint);
			    }
		    }

		    return new MipsSignalTable(timePoints);
	    }

	    public string RetrieveTableAsEncodedString()
	    {
		   if (cachedTable != null)
		    {
			    return cachedTable;
		    }
		    StringBuilder builder = new StringBuilder();
		    string tableName = "A";

		    var points = Points.OrderBy(x => x.TimePoint).ToList();
		    for (int i = 0; i < points.Count; i++)
		    {
			    // TODO: Move this if / else into separate function calls to speed up for loop evaluation. 
			    if (i == 0)
			    {
				    var count = GetLoopCount(points, points[i]);
				    if (count.HasValue)
				    {
					    builder.Append("0:[" + tableName);
					    builder.Append(":" + count.Value + "," + points[i].TimePoint);
					    tableName = ((int)tableName[0] + 1).ToString();
				    }
				    else
				    {
					    builder.Append(points[i].TimePoint);
				    }

				    foreach (var dcBiasElement in points[i].DcBiasElements)
				    {
					    builder.Append(":" + dcBiasElement.Key + ":" + Convert.ToInt32(dcBiasElement.Value));
				    }

				    foreach (var digitalOutputElement in points[i].DigitalOutputElements)
				    {
					    builder.Append(
						    ":" + digitalOutputElement.Key + ":" + Convert.ToInt32(digitalOutputElement.Value));
				    }
			    }

			    else
			    {
				    var count = GetLoopCount(points, points[i]);
				    if (count.HasValue)
				    {
					    builder.Append("," + points[i].TimePoint + ":[" + tableName + ":");

					    builder.Append(count.Value + ",0");
					    tableName = char.ToString((char)(tableName[0] + 1));
				    }

				    else
				    {
					    builder.Append("," + points[i].TimePoint);
				    }

				    foreach (var dcBiasElement in points[i].DcBiasElements)
				    {
					    var point =
						    points[i - 1].DcBiasElements.FirstOrDefault(
							    x => x.Equals(dcBiasElement));
					    if (point.Key != default(string))
					    {
						    if (Math.Abs(dcBiasElement.Value - point.Value) > 1e-6)
						    {
							    builder.Append(":" + dcBiasElement.Key + ":" + dcBiasElement.Value);
						    }
					    }

					    else
					    {
						    builder.Append(
							    ":" + dcBiasElement.Key + ":" + dcBiasElement.Value);
					    }
				    }

				    foreach (var digitalOutputElement in points[i].DigitalOutputElements)
				    {
					    var point =
						    points[i - 1].DigitalOutputElements.FirstOrDefault(
							    x => x.Key.Equals(digitalOutputElement.Key));
					    if (point.Key != default(string))
					    {
						    if (digitalOutputElement.Value != point.Value)
						    {
							    builder.Append(
								    ":" + digitalOutputElement.Key + ":" + Convert.ToInt32(digitalOutputElement.Value));
						    }
					    }
					    else
					    {
						    builder.Append(
							    ":" + digitalOutputElement.Key + ":" + Convert.ToInt32(digitalOutputElement.Value));
					    }
				    }


			    }

			    if (points[i].PsgPointLoopData.DoLoop)
			    {
				    builder.Append("]");
			    }

		    }
		    cachedTable = builder.ToString();
		    return cachedTable;
	    }

	    private static int? GetLoopCount(List<PsgPoint> points, PsgPoint point)
	    {
		    foreach (var psgPoint in points)
		    {
			    var psgPointLoopData = psgPoint.PsgPointLoopData;
			    if (psgPointLoopData.DoLoop && psgPointLoopData.LoopToName.Equals(point.Name))
			    {
				    return psgPointLoopData.LoopCount;
			    }
		    }
		    return null;
	    }

	    /// <summary>
	    /// Returns enumeration of points; ascending order by time.
	    /// </summary>
	    public IEnumerable<PsgPoint> Points
	    {
		    get
		    {
			    return timePoints.OrderBy(x => x.TimePoint);
		    }
	    }
	}

	
}
