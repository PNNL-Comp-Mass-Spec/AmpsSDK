// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoopEvent.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LoopEvent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.Data.Events
{
    /// <summary>
    /// TODO The loop event.
    /// </summary>
    public class LoopEvent : SignalTableEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoopEvent"/> class.
        /// </summary>
        /// <param name="repetitions">
        /// The repetitions.
        /// </param>
        public LoopEvent(int repetitions)
        {
            this.Repetitions = repetitions;
        }

        /// <summary>
        /// Gets the repetitions.
        /// </summary>
        public int Repetitions { get; private set; }
    }
}