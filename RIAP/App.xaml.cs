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

#region Usings

using System.Windows;
using System.Windows.Threading;
using JaINTP.RIAP.Windows;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

#endregion Usings

namespace JaINTP.RIAP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Register with AppCenter
            AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("b6d3e798-014c-40da-ab88-f135ce4f3ae4",
                   typeof(Analytics), typeof(Crashes));

            // Add global exception handling.
            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);

            // Start main window.
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG
            // Don't do shit if debugging.
            e.Handled = false;

#else
            // Report those unhandled exceptions, bruh!
            Crashes.TrackError(e.Exception);
#endif
        }
    }
}