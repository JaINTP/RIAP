// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 14-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 08-12-2020
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

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
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
            // AppCenter Stuff
            AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("b6d3e798-014c-40da-ab88-f135ce4f3ae4",
                   typeof(Analytics), typeof(Crashes));

            // Squirrel stuff
            try
            {
                logger.Info("Attempting to fetch update information.");
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
#if DEBUG
                    // Don't update while in debug mode...
#else
                    await updateManager.DownloadReleases(new[] { lastVersion });
                    await updateManager.ApplyReleases(updates);
                    var releaseEntry = await updateManager.UpdateApp();
                    Title = $"Updated to v{lastVersion?.Version.ToString()}. Please restart!";
                    logger.Debug($"Updated to version {lastVersion?.Version.ToString()}");
#endif
                }
            }
            catch (WebException ex)
            {
                logger.Error(ex, $"Squirrel had issues updating: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{ex.Message}");
            }
            finally
            {
                Title = $"RIAP v{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
            }
        }

        #endregion Event Handlers.
    }
}