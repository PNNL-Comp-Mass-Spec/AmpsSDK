namespace Infrastructure.Io
{
    public interface ICommunicator
    {
        void Write(byte[] value, string separator);

        void WriteEnd(string appendToEnd = null);

        void WriteHeader<T>(T command);

        string ReadLine();

        void Close();

        void Open();
    }
}