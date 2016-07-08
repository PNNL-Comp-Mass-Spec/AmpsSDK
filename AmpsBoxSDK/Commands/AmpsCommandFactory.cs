// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsCommandFactory.cs" company="">
//   
// </copyright>
// <summary>
//   Creates an AMPS Command Provider based on the version returned from the device.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Creates an AMPS Command Provider based on the version returned from the device.
    /// </summary>
    public class AmpsCommandFactory
    {
        #region Static Fields

        /// <summary>
        /// TODO The m_provider map.
        /// </summary>
        private static readonly Dictionary<string, AmpsCommandProvider> providerMap;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="AmpsCommandFactory"/> class.
        /// </summary>
        static AmpsCommandFactory()
        {
            providerMap = new Dictionary<string, AmpsCommandProvider>();

            //TODO: Use MEF to supply providers. 
            var provider = new GammaCommandProvider();
          //  var mipsProvider = new MipsAlphaCommandProvider();
            providerMap.Add(provider.GetSupportedVersions().ToLower(), provider);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The create command provider.
        /// </summary>
        /// <param name="version">
        /// TODO The version.
        /// </param>
        /// <returns>
        /// The <see cref="AmpsCommandProvider"/>.
        /// </returns>
        public static AmpsCommandProvider CreateCommandProvider(string version)
        {
            version = version.ToLower();
            foreach (var key in providerMap.Keys)
            {
                var match = Regex.Match(key, @"\d+(\.\d{1,2}(\w))?", RegexOptions.IgnoreCase);
                if (match.Value == version)
                {
                    return providerMap[version];
                }
            }

            return providerMap["1.23"];
        }

        #endregion
    }
}