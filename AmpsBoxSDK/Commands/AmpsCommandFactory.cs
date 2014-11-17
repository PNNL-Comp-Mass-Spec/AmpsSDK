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
    using System.Collections.Generic;

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

            AmpsCommandProvider provider;

            // providerMap.Add(provider.GetSupportedVersions().ToLower(), provider);

            // provider = new BetaCommandProvider();
            // providerMap.Add(provider.GetSupportedVersions().ToLower(), provider);
            provider = new GammaCommandProvider();
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
            AmpsCommandProvider provider = null;

            version = version.ToLower();

            if (providerMap.ContainsKey(version))
            {
                return providerMap[version];
            }

            return new GammaCommandProvider();
        }

        #endregion
    }
}