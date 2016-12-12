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
            this.digitalOutput = new Dictionary<string, bool>();
            this.dcBias = new Dictionary<string, double>();
            this.PsgPointLoopData = new LoopData();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="name"></param>
       /// <param name="timePoint"></param>
       /// <param name="loopData"></param>
        public PsgPoint(string name, int timePoint, LoopData loopData) : this(name, timePoint)
        {
            this.PsgPointLoopData = loopData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timePoint"></param>
        public PsgPoint(string name, int timePoint) : this()
        {
            this.Name = name;
            this.TimePoint = timePoint;
            this.PsgPointLoopData = new LoopData();

        }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public int TimePoint { get;  set; }

        [DataMember]
        public LoopData PsgPointLoopData { get; private set; }

        public PsgPoint UpdatePsgPointLoopData(LoopData data)
        {
            return new PsgPoint(this.Name, this.TimePoint, data);
        }

        /// <summary>
        /// Adds digital channel to the time point.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="state"></param>
        public void CreateOutput(string address, bool state)
        {
            if (!this.digitalOutput.ContainsKey(address))
            {
                this.digitalOutput.Add(address, state);
            }
        }


        /// <summary>
        /// Finds and updates a digital output.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="state"></param>
        public void UpdateOutput(string channelToUpdate, bool state)
        {
            if (this.digitalOutput.ContainsKey(channelToUpdate))
            {
                this.digitalOutput[channelToUpdate] = state;
            }
        }
        /// <summary>
        /// Finds and removes the output.
        /// </summary>
        /// <param name="address"></param>
        public void RemoveAnalogSignal(string address)
        {
            this.dcBias.Remove(address);
        }

        public void RemoveDigitalSignal(string address)
        {
            this.digitalOutput.Remove(address);
        }

        public void CreateOutput(string address, double volts)
        {
            if (!this.dcBias.ContainsKey(address))
            {
                this.dcBias.Add(address, volts);
            }
        }

        public void UpdateOutput(string address, double volts)
        {
            if (this.dcBias.ContainsKey(address))
            {
                this.dcBias[address] = volts;
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
            this.UpdatePsgPointLoopData(new LoopData(loopCount, point.Name, true));
        }

        public IEnumerable<KeyValuePair<string, double>> DcBiasElements
        {
            get
            {
                return this.dcBias.OrderBy(x => x.Key);
            }
        }

        public IEnumerable<KeyValuePair<string, bool>> DigitalOutputElements
        {
            get
            {
                return this.digitalOutput.OrderBy(x => x.Key);
            }
        } 
    }
}