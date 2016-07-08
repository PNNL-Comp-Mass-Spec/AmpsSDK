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
    public static class AmpsBoxSignalTableCommandFormatter 
    {
        #region Public Methods and Operators

        #endregion

        /// <summary>
        /// Formats a provided amps signal table into a string command that can be interpreted by an AMPS / MIPS box.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="converter"></param>
        /// <returns>Command string ready to be sent to AMPS / MIPS box.</returns>
        public static string FormatTable(AmpsSignalTable table, ITimeUnitConverter<double> converter)
        {
            StringBuilder builder = new StringBuilder();
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
                        builder.Append(":" + dcBiasElement.ChannelIdentifier.Address + ":" + Convert.ToInt32(dcBiasElement.Voltage));
                    }

                    foreach (var digitalOutputElement in points[i].DigitalOutputElements)
                    {
                        builder.Append(
                                  ":" + digitalOutputElement.ChannelIdentifier.Address + ":" + Convert.ToInt32(digitalOutputElement.State));
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
                                x => x.ChannelIdentifier.SameValueAs(dcBiasElement.ChannelIdentifier));
                        if (point != null)
                        {
                            if (Math.Abs(dcBiasElement.Voltage - point.Voltage) > 1e-6)
                            {
                                builder.Append(":" + dcBiasElement.ChannelIdentifier.Address + ":" + dcBiasElement.Voltage);
                            }
                        }

                        else
                        {
                            builder.Append(
                            ":" + dcBiasElement.ChannelIdentifier.Address + ":" + dcBiasElement.Voltage);
                        }
                    }

                    foreach (var digitalOutputElement in points[i].DigitalOutputElements)
                    {
                        var point =
                            points[i - 1].DigitalOutputElements.FirstOrDefault(
                                x => x.ChannelIdentifier.SameValueAs(digitalOutputElement.ChannelIdentifier));
                        if (point != null)
                        {
                            if (digitalOutputElement.State == point.State)
                            {
                                builder.Append(
                              ":" + digitalOutputElement.ChannelIdentifier.Address + ":" + Convert.ToInt32(digitalOutputElement.State));
                            }
                        }
                        else
                        {
                            builder.Append(
                            ":" + digitalOutputElement.ChannelIdentifier.Address + ":" + Convert.ToInt32(digitalOutputElement.State));
                        }
                    }

                   
                }

                if (points[i].PsgPointLoopData.DoLoop)
                {
                    builder.Append("]");
                }

            }
            var stringToReturn = string.Format("{0};{1};", "STBLDAT", builder.ToString());
            return stringToReturn;
        }

        /// <summary>
        /// Checks loop data and returns the loop count for a given psg point.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="point"></param>
        /// <returns>The loop count for a matching psg point that contains the loop name of the provided psg point.</returns>
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


    }
}