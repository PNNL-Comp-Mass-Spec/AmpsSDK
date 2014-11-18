// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The enumeration extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Extensions
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// TODO The enumeration extensions.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        #region Public Methods and Operators

       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="value"></param>
       /// <returns></returns>
        public static ObservableCollection<T> Create<T>(this ObservableCollection<T> value)
        {
            foreach (var x in Enum.GetValues(typeof(T)))
            {
                value.Add((T)x);
            }

            return value;
        }

        #endregion
    }
}