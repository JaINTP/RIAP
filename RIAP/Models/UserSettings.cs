// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 14-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 13-12-2020
// ***********************************************************************
// <copyright file="UserSettings.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Usings

using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;

#endregion Usings

namespace JaINTP.RIAP.Models
{
    /// <summary>
    /// Basic bitch class representing specific user changeable settings.
    /// </summary>
    public class UserSettings
        : SettingsHandler<UserSettings>
    {
        #region Properties.

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MinMinutes { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MaxMinutes { get; set; }

        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MinSeconds { get; set; }

        [DefaultValue(10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MaxSeconds { get; set; }

        [DefaultValue(1f)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Volume { get; set; }

        // Cbf figuring out WHAT to use, so we'll just go null, like my dating life...
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public ObservableCollection<string> FileList { get; set; }

        #endregion Properties.
    }
}