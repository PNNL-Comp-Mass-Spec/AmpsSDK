// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBoxSignalTableCommandFormatter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The amps box signal Table command formatter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FalkorSDK.Data;
    using FalkorSDK.Data.Elements;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Signals;

    /// <summary>
    /// TODO The amps box signal Table command formatter.
    /// </summary>
    public class AmpsBoxSignalTableCommandFormatter : ISignalTableFormatter<AmpsSignalTable, double>
    {
        #region Fields

        /// <summary>
        /// Table command to format
        /// </summary>
        private readonly string commandFormat;

        private string eventData = string.Empty;

        private int tableName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxSignalTableCommandFormatter"/> class.
        /// </summary>
        public AmpsBoxSignalTableCommandFormatter()
        {
            this.commandFormat = "STBLDAT;{1}{3}{0}{2};";
            this.tableName = 1;
        }

        #endregion

        #region Public Methods and Operators

        #endregion

        public string FormatTable(AmpsSignalTable table, ITimeUnitConverter<double> converter)
        {
            string sTable = string.Empty;
            string tableName = "A";

            var points = table.Points.ToList();
            for (int i = 0; i < points.Count; i++)
            {
                // TODO: Move this if / else into separate function calls to speed up for loop evaluation. 
                if (i == 0)
                {
                    var count = GetLoopCount(points, points[i]);
                    if (count.HasValue)
                    {
                        sTable += "0:[" + tableName;
                        sTable += ":" + count.Value + "," + points[i].TimePoint;
                        tableName = ((int)tableName[0] + 1).ToString();
                    }
                    else
                    {
                        sTable += points[i].TimePoint;
                    }

                    foreach (var dcBiasElement in points[i].DcBiasElements)
                    {
                        if (dcBiasElement.Data != 0)
                        {
                            sTable += ":" + dcBiasElement.Address + ":" + Convert.ToInt32(dcBiasElement.Data);
                        }
                    }

                    foreach (var digitalOutputElement in points[i].DigitalOutputElements)
                    {
                        if (digitalOutputElement.Data)
                        {
                            sTable += ":" + digitalOutputElement.Address +  ":" + Convert.ToInt32(digitalOutputElement.Data);
                        }
                    }
                }

                else
                {
                    var count = GetLoopCount(points, points[i]);
                    if (count.HasValue)
                    {
                        sTable += "," + points[i].TimePoint + ":[" + tableName + ":";
                        sTable += count.Value + ",0";
                        tableName = char.ToString((char)(tableName[0] + 1));
                    }

                    else
                    {
                        sTable += "," + points[i].TimePoint;
                    }

                    foreach (var dcBiasElement in points[i].DcBiasElements)
                    {
                        var point =
                            points[i - 1].DcBiasElements.FirstOrDefault(
                                x => x.Address.SameValueAs(dcBiasElement.Address));
                        if (point != null)
                        {
                            if (!dcBiasElement.Data.SameValueAs(point.Data))
                            {
                                sTable += ":" + dcBiasElement.Address + ":" + dcBiasElement.Data;
                            }
                        }
                    }

                    foreach (var digitalOutputElement in points[i].DigitalOutputElements)
                    {
                        var point =
                            points[i - 1].DigitalOutputElements.FirstOrDefault(
                                x => x.Address.SameValueAs(digitalOutputElement.Address));
                        if (point != null)
                        {
                            sTable += ":" + digitalOutputElement.Address + Convert.ToInt32(digitalOutputElement.Data);
                        }
                    }

                   
                }

                if (points[i].Loop)
                {
                    sTable += "]";
                }

            }

            return sTable;
        }

        private int? GetLoopCount(List<PsgPoint> points, PsgPoint point)
        {
            var reference = ContainsReference(points, point);
            if (reference != null)
            {
                return reference.LoopCount;
            }
            return null;
        }

        private PsgPoint ContainsReference(List<PsgPoint> points, PsgPoint point)
        {
            var reference = points.FirstOrDefault(x => x.LoopName == point.Name);
            return reference;
        }
    }
}