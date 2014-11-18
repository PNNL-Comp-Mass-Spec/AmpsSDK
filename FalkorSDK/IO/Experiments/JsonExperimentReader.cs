// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonExperimentReader.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The json experiment reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Experiments
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters;

    using FalkorSDK.Data.Experiment;

    using Newtonsoft.Json;

    /// <summary>
    /// TODO The json experiment reader.
    /// </summary>
    public class JsonExperimentReader : IExperimentReader
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The read.
        /// </summary>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        /// <returns>
        /// The <see cref="Experiment"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public Experiment Read(string path)
        {
            try
            {
                using (var reader = File.OpenText(path))
                {
                    var jReader = new JsonTextReader(reader);

                    var serializer = new JsonSerializer
                                         {
                                             MissingMemberHandling = MissingMemberHandling.Ignore, 
                                             NullValueHandling = NullValueHandling.Ignore, 
                                             PreserveReferencesHandling = PreserveReferencesHandling.All, 
                                             TypeNameAssemblyFormat = FormatterAssemblyStyle.Full, 
                                             TypeNameHandling = TypeNameHandling.All, 
                                         };

                    // TODO: Handle when there is an error with loading a device.
                    var data = serializer.Deserialize(jReader, typeof(Experiment)) as Experiment;
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}