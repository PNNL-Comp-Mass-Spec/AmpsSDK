using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AmpsBoxSdk.Data
{
    /// <summary>
    /// Amps Signal table is a sparse data structure to represent groups of time points and voltage / TTL logic output. 
    /// </summary>
    [DataContract]
    public class AmpsSignalTable
    {
        [DataMember]
        private List<PsgPoint> timePoints;

        private string cachedTable;

        public AmpsSignalTable()
        {
            this.timePoints = new List<PsgPoint>();
        }

        private AmpsSignalTable(IList<PsgPoint> timePoints) : this()
        {
            for (int i = 0; i < timePoints.Count; i++)
            {
                timePoints.Add(timePoints[i]);
            }
        }

        public PsgPoint this[string pointName]
        {
            get
            {
                return this.timePoints.FirstOrDefault(x => x.Name == pointName);
            }
        }

        public PsgPoint this[int time]
        {
            get
            {
               return this.timePoints.FirstOrDefault(x => x.TimePoint == time);
            }
        }

        public AmpsSignalTable AddTimePoint(PsgPoint point)
        {
            if (!this.timePoints.Select(x => x.TimePoint).Contains<int>(point.TimePoint))
            {
                this.timePoints.Add(point);
            }
            return new AmpsSignalTable(this.timePoints);
        }

        public AmpsSignalTable AddTimePoint(int clock, LoopData loopData)
        {
            if (!this.timePoints.Select(x => x.TimePoint).Contains(clock))
            {
                char[] ap = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();
                this.timePoints.Add(new PsgPoint(ap[this.timePoints.Count].ToString(), clock, loopData));
            }
            return new AmpsSignalTable(this.timePoints);
        }

        public AmpsSignalTable RemoveTimePoint(PsgPoint point)
        {
            if (this.timePoints.Select(x => x.TimePoint).Contains<int>(point.TimePoint))
            {
                this.timePoints.Remove(point);
            }
            return new AmpsSignalTable(this.timePoints);
        }

        public AmpsSignalTable RemoveTimePoint(int clockToRemove)
        {
            var timePoint = this.timePoints.FirstOrDefault(x => x.TimePoint == clockToRemove);
            if (timePoint != null)
            {
                this.timePoints.Remove(timePoint);
            }
            return new AmpsSignalTable(this.timePoints);
        }

        public AmpsSignalTable AddSignalTable(AmpsSignalTable signalTable)
        {
            // TODO: Make AmpsSignalTables immutable. 
            foreach (var psgPoint in signalTable.Points)
            {
                var timePoint = this.Points.FirstOrDefault(x => x.TimePoint == psgPoint.TimePoint);
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
                    this.AddTimePoint(psgPoint);
                }
            }

            return new AmpsSignalTable(this.timePoints);
        }

        public string RetrieveTableAsEncodedString()
        {
            if (cachedTable != null)
            {
                return this.cachedTable;
            }
            StringBuilder builder = new StringBuilder();
            string tableName = "A";

            var points = this.Points.ToList();
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
            cachedTable = $"STBLDAT;{builder};";
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
                return this.timePoints.OrderBy(x => x.TimePoint);
            }
        }
    }
}