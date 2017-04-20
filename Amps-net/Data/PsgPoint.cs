using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AmpsBoxSdk.Data
{
    /// <summary>
    /// Data structure based off of Gordon Anderson's Psg Point. Will be useful with AMPS boxes, but may need to be generalized for non-AMPS box waveforms. 
    /// </summary>
    [DataContract]
    public class PsgPoint
    {
        private readonly Dictionary<string, bool> digitalOutput;

        private readonly Dictionary<string, double> dcBias;

        private PsgPoint()
        {
            digitalOutput = new Dictionary<string, bool>();
            dcBias = new Dictionary<string, double>();
            PsgPointLoopData = new LoopData();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="name"></param>
       /// <param name="timePoint"></param>
       /// <param name="loopData"></param>
        public PsgPoint(string name, int timePoint, LoopData loopData) : this(name, timePoint)
        {
            PsgPointLoopData = loopData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timePoint"></param>
        public PsgPoint(string name, int timePoint) : this()
        {
            Name = name;
            TimePoint = timePoint;
            PsgPointLoopData = new LoopData();

        }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public int TimePoint { get;  set; }

        [DataMember]
        public LoopData PsgPointLoopData { get; private set; }

        public void UpdatePsgPointLoopData(LoopData data)
        {
            PsgPointLoopData = data;
        }

        /// <summary>
        /// Adds digital channel to the time point.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="state"></param>
        public void CreateOutput(string address, bool state)
        {
            if (!digitalOutput.ContainsKey(address))
            {
                digitalOutput.Add(address, state);
            }
        }


        /// <summary>
        /// Finds and updates a digital output.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="state"></param>
        public void UpdateOutput(string channelToUpdate, bool state)
        {
            if (digitalOutput.ContainsKey(channelToUpdate))
            {
                digitalOutput[channelToUpdate] = state;
            }
        }
        /// <summary>
        /// Finds and removes the output.
        /// </summary>
        /// <param name="address"></param>
        public void RemoveAnalogSignal(string address)
        {
            dcBias.Remove(address);
        }

        public void RemoveDigitalSignal(string address)
        {
            digitalOutput.Remove(address);
        }

        public void CreateOutput(string address, double volts)
        {
            if (!dcBias.ContainsKey(address))
            {
                dcBias.Add(address, volts);
            }
        }

        public void UpdateOutput(string address, double volts)
        {
            if (dcBias.ContainsKey(address))
            {
                dcBias[address] = volts;
            }
        }


        public void ReferenceTimePoint(PsgPoint point, int loopCount)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point));
            }
            if (loopCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(loopCount), "Loop count cannot be < 1!");
            }
           UpdatePsgPointLoopData(new LoopData(loopCount, point.Name, true));
        }

        public IEnumerable<KeyValuePair<string, double>> DcBiasElements
        {
            get
            {
                return dcBias.OrderBy(x => x.Key);
            }
        }

        public IEnumerable<KeyValuePair<string, bool>> DigitalOutputElements
        {
            get
            {
                return digitalOutput.OrderBy(x => x.Key);
            }
        } 
    }
}