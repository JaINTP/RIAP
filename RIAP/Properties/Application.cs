// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 26-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 26-11-2020
// ***********************************************************************
// <copyright file="Application.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.RIAP.Properties
{
    #region Usings

    using System.Collections.ObjectModel;

    #endregion Usings

    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Application
    {
        #region Properties

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Binary)]
        [global::System.Configuration.DefaultSettingValueAttribute(null)]
        public ObservableCollection<string> FileList
        {
            get => ((ObservableCollection<string>)(this[nameof(FileList)]));
            set => this[nameof(FileList)] = value;
        }

        #endregion Properties
    }
}