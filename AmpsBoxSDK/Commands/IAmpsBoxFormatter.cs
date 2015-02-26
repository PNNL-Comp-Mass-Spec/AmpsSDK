using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Commands
{
    public interface IAmpsBoxFormatter
    {
        #region Methods
        /// <summary>
        /// Method responsible for building communicator specific command strings.
        /// </summary>
        /// <param name="commandType">Type of command to be formed.</param>
        /// <param name="commandData">Data to be contained in the command.</param>
        /// <returns></returns>
        string BuildCommunicatorCommand(AmpsCommandType commandType, object commandData);
        #endregion
        
        #region Properties

        #endregion

    }
}
