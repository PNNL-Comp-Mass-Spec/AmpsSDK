// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UimfTests.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The uimf tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDKTest.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.Serialization.Json;

    /// <summary>
    /// TODO The uimf tests.
    /// </summary>
    public class UimfTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The test uimf mongo.
        /// </summary>
        public void TestUimfMongo()
        {
           
        }

        #endregion
    }

    /// <summary>
    /// TODO The test class.
    /// </summary>
    public class TestClass
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestClass"/> class.
        /// </summary>
        public TestClass()
        {
            this.Id = Guid.NewGuid();
            this.Data = new Dictionary<uint, Dictionary<int, int>>();
            for (uint i = 0; i < 148000; i += 10)
            {
                this.Data.Add(i, new Dictionary<int, int>());
                for (int j = 0; j < 360; j++)
                {
                    this.Data[i].Add(j, j);
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public Dictionary<uint, Dictionary<int, int>> Data { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        #endregion
    }
}