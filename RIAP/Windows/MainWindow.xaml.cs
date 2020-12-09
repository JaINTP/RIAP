// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 14-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 09-12-2020
// ***********************************************************************
// <copyright file="App.xaml.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Usings.

using Squirrel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;

using Microsoft.AppCenter.Crashes;

#endregion Usings.

namespace JaINTP.RIAP.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = $"RIAP v{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
        }

        #region Fields

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        #endregion Fields

        #region Event Handlers.

        private void LaunchGithub(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/jaintp");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private async void MetroWindow_LoadedAsync(object sender, System.Windows.RoutedEventArgs e)
        {
#if DEBUG
                    // Don't update while in debug mode...
#else
            // Secret Squirrel business ;)
            try
            {
                logger.Debug("Attempting to fetch update information.");
                using (var updateManager =
                    await UpdateManager.GitHubUpdateManager(@"https://github.com/JaINTP/RIAP"))
                {
                    var updates = await updateManager.CheckForUpdate();
                    var lastVersion = updates?.ReleasesToApply?.OrderBy(x => x.Version).LastOrDefault();

                    if (lastVersion == null)
                    {
                        return;
                    }

                    Title = lastVersion?.Version.ToString() == null ? "" : $" Updating to v{lastVersion?.Version.ToString()}";
                    logger.Debug($"Latest version: {lastVersion?.Version.ToString()}");
                    await updateManager.DownloadReleases(new[] { lastVersion });
                    await updateManager.ApplyReleases(updates);
                    await updateManager.UpdateApp();

                    Title = $"Updated to v{lastVersion?.Version.ToString()}. Please restart!";
                    logger.Debug($"Updated to version {lastVersion?.Version.ToString()}");
                }
            }
            catch (WebException ex)
            {
                logger.Error(ex, $"Squirrel had issues updating: {ex.Message}");
                Crashes.TrackError(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{ex.Message}");
                Crashes.TrackError(ex);
            }
            finally
            {
                Title = $"RIAP v{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
            }
#endif
        }

        #endregion Event Handlers.
    }
}