namespace AmpsBoxSdk.Devices
{
    public interface IAmpsBox
    {
        string GetConfig();

        IAmpsBoxCommunicator Communicator { get; }

    }
}