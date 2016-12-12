using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace AmpsBoxSdk.Data
{
    /// <summary>
    /// Amps Signal table is a sparse data structure to represent groups of time points and voltage / TTL logic output. 
    /// </summary>
    [DataContract]
    public class AmpsSignalTable
    {
        [DataMember]
        private Collection<PsgPoint> timePoints;
         
        public AmpsSignalTable()
        {
            this.timePoints = new Collection<PsgPoint>();
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

        public void AddTimePoint(PsgPoint point)
        {
            if (!Enumerable.Contains<int>(this.timePoints.Select(x => x.TimePoint), point.TimePoint))
            {
                this.timePoints.Add(point);
            }
        }

        public void AddTimePoint(int clock, LoopData loopData)
        {
            if (!Enumerable.Contains(this.timePoints.Select(x => x.TimePoint), clock))
            {
                char[] ap = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();
                this.timePoints.Add(new PsgPoint(ap[this.timePoints.Count].ToString(), clock, loopData));
            }
        }

        public void RemoveTimePoint(PsgPoint point)
        {
            if (Enumerable.Contains<int>(this.timePoints.Select(x => x.TimePoint), point.TimePoint))
            {
                this.timePoints.Remove(point);
            }
        }

        public void RemoveTimePoint(int clockToRemove)
        {
            var timePoint = this.timePoints.FirstOrDefault(x => x.TimePoint == clockToRemove);
            if (timePoint != null)
            {
                this.timePoints.Remove(timePoint);
            }
        }

        public void AddSignalTable(AmpsSignalTable signalTable)
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