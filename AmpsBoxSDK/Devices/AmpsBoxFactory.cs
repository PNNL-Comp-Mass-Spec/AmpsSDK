﻿namespace AmpsBoxSdk.Devices
{
    using AmpsBoxSdk.Commands;

    using FalkorSDK.IO.Ports;

    /// <summary>
    /// Amps Box Factory
    /// The purpose of the factory is to hide the internal structure of the amps box.
    /// </summary>
    public static class AmpsBoxFactory
    {
        //TODO: Make the AmpsBoxFactory the primary way of constructing an AmpsBox object, remove MEF construction methods. 
        public static AmpsBox NewAmpsBox(IAmpsBoxCommunicator<FalkorSerialPort> communicator, IAmpsBoxFormatter formatter)
        {
            return new AmpsBox(communicator, formatter);
        }
    }
}