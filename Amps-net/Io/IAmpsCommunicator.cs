using System;
using AmpsBoxSdk.Commands;

namespace AmpsBoxSdk.Io
{
    public interface IAmpsCommunicator
    {
        void Write(byte[] value, string separator);

        void WriteEnd(string appendToEnd = null);

        void WriteHeader(AmpsCommand command);

        string ReadLine();

        void Close();

        void Open();

        IObservable<string> MessageSources { get; }
  }
}