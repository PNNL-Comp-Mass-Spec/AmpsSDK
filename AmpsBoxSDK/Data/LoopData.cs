using System.Runtime.Serialization;

namespace AmpsBoxSdk.Data
{
    [DataContract]
    public class LoopData
    {
        public LoopData()
        {
            this.LoopCount = 1;
            this.LoopToName = string.Empty;
            this.DoLoop = false;
        }

        public LoopData(int loopCount, string loopToName, bool doLoop)
        {
            this.LoopCount = loopCount;
            this.LoopToName = loopToName;
            this.DoLoop = doLoop;
        }
        [DataMember]
        public bool DoLoop { get; }
        [DataMember]
        public string LoopToName { get;}
        [DataMember]
        public int LoopCount { get; }
    }
}