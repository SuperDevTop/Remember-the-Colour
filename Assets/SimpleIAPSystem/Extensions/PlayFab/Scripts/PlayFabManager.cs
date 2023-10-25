/*  This file is part of the "Simple IAP System" project by FLOBUK.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;

namespace SIS
{
    /// <summary>
    /// Empty class and does nothing (when not using PlayFab).
    /// Only present to initialize Unity IAP correctly.
    /// </summary>
    public class PlayFabManager : MonoBehaviour
    {
        /// <summary>
        /// Returns nothing.
        /// </summary>
        public static PlayFabManager GetInstance()
        {
            return null;
        }
    }


    public class PlayFabStore : MonoBehaviour
    {
    }
}