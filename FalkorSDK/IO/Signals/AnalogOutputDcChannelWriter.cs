namespace FalkorSDK.IO.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    using FalkorSDK.Channel;
    using FalkorSDK.Data.Signals;

    class AnalogOutputDcChannelWriter : IChannelWriter<AOChannel>
    {
        public void Write(string path, IEnumerable<AOChannel> signals)
        {
            var signalsToWrite = new XDocument(new XElement(
                "Signals",
                signals.Select(
                    signal =>
                    new XElement(
                        "signal",
                        new XAttribute("name", signal.Name),
                        new XAttribute("channel", signal.Address)))));

           signalsToWrite.Save(path);
            
        }
    }
}