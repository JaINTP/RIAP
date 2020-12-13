// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 14-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 13-12-2020
// ***********************************************************************
// <copyright file="MainViewModel.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Usings

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using JaINTP.RIAP.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using NAudio.Wave;

#endregion Usings

namespace JaINTP.RIAP.ViewModels
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         <see cref="http://www.galasoft.ch/mvvm" />
    ///     </para>
    /// </summary>
    /// TODO: Fix
    public class MainViewModel
        : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            rand = new Random();

            // Commands.
            AddCommand = new RelayCommand(AddCommandExecute);
            RemoveCommand = new RelayCommand(RemoveCommandExecute);
            StartStopCommand = new RelayCommand(StartStopCommandExecute);

            // Load the settings file.
            settings = UserSettings.Load(configFile);

            // Checks to see if FileList is null. (Settings Hack)
            settings.FileList = settings.FileList ?? new ObservableCollection<string>();

            // Save
            settings.Save(configFile);

            StartStopContent = "Start";
        }

        #region Destructors

        /// <summary>
        /// Finalizes an instance of the MainViewModel class.
        /// </summary>
        ~MainViewModel()
        {
            logger.Debug("Saving all user settings.");
            settings.MinSeconds = MinSeconds;
            settings.MaxSeconds = MaxSeconds;
            settings.MinMinutes = MinMinutes;
            settings.MaxMinutes = MaxMinutes;
            settings.FileList = FileList;
            settings.Volume = Volume;
            settings.Save(configFile);
        }

        #endregion Destructors

        #region Fields.

        // Ugly but idgaf.
        private readonly string configFile = Path.Combine(
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}",
            $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}",
            "Settings.json");

        private readonly UserSettings settings;

        private readonly IDialogService dialogService;
        private AudioHandler audioHandler;
        private TimerHandler timer;
        private readonly Random rand;
        private IList selectedFiles;
        private bool isRunning;
        private string startStopContent;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        #endregion Fields.

        #region Properties.

        /// <summary>
        ///     Gets or sets the minimum minutes.
        /// </summary>
        /// <value>The minimum minutes.</value>
        public int MinMinutes
        {
            get => settings.MinMinutes;
            set
            {
                settings.MinMinutes = value;
                RaisePropertyChanged(nameof(MinMinutes));
            }
        }

        /// <summary>
        ///     Gets or sets the minimum seconds.
        /// </summary>
        /// <value>The minimum seconds.</value>
        public int MinSeconds
        {
            get => settings.MinSeconds;
            set
            {
                settings.MinSeconds = value;
                RaisePropertyChanged(nameof(MinSeconds));
            }
        }

        /// <summary>
        ///     Gets or sets the maximum minutes.
        /// </summary>
        /// <value>The minimum minutes.</value>
        public int MaxMinutes
        {
            get => settings.MaxMinutes;
            set
            {
                settings.MaxMinutes = value;
                RaisePropertyChanged(nameof(MaxMinutes));
            }
        }

        /// <summary>
        ///     Gets or sets the maximum seconds.
        /// </summary>
        /// <value>The maximum seconds.</value>
        public int MaxSeconds
        {
            get => settings.MaxSeconds;
            set
            {
                settings.MaxSeconds = value;
                RaisePropertyChanged(nameof(MaxSeconds));
            }
        }

        /// <summary>
        ///     Gets or sets the ObservableCollection a list of file paths.
        /// </summary>
        /// <value>
        ///     The ObservableCollection containing a list of file paths.
        /// </value>
        public ObservableCollection<string> FileList
        {
            get => settings.FileList;
            set
            {
                settings.FileList = value;
                RaisePropertyChanged(nameof(FileList));
            }
        }

        /// <summary>
        /// Gets or sets the volume level.
        /// </summary>
        /// <value>
        /// The volume level.
        /// </value>
        public float Volume
        {
            get { return settings.Volume; }
            set
            {
                settings.Volume = value;
                RaisePropertyChanged(nameof(Volume));
            }
        }

        /// <summary>
        ///     Gets or sets a list of selected files.
        /// </summary>
        /// <value>
        ///     A list of selected files.
        /// </value>
        public IList SelectedFiles
        {
            get => selectedFiles;
            set => Set(ref selectedFiles, value);
        }

        /// <summary>
        ///     Gets or sets whether the timer is running.
        /// </summary>
        /// <value>
        ///     Whether the timer is running.
        /// </value>
        public bool IsRunning
        {
            get => isRunning;
            set => Set(ref isRunning, value);
        }

        /// <summary>
        /// Gets or sets the start stop content.
        /// </summary>
        /// <value>
        /// The start stop content.
        /// </value>
        public string StartStopContent
        {
            get => startStopContent;
            set => Set(ref startStopContent, value);
        }

        #endregion Properties.

        #region Commands.

        /// <summary>
        ///     Gets the open command.
        /// </summary>
        /// <value>The open command.</value>
        public ICommand AddCommand { get; private set; }

        private void AddCommandExecute()
        {
            var settings = new OpenFileDialogSettings
            {
                Title = "Select Audio File",
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "Audio files |*.mp3;*.wav",
                // BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|
                Multiselect = true
            };

            var success = dialogService.ShowOpenFileDialog(this, settings);

            if (!(bool)success) return;

            foreach (var filename in settings.FileNames)
            {
                if (!File.Exists(filename)) continue;
                if (!FileList.Contains(filename))
                {
                    FileList.Add(filename);
                }
            }
        }

        /// <summary>
        ///     Gets the remove command.
        /// </summary>
        /// <value>The remove command.</value>
        public ICommand RemoveCommand { get; private set; }

        /// <summary>
        /// Removes the command execute.
        /// </summary>
        /// TODO Edit XML Comment Template for RemoveCommandExecute
        private void RemoveCommandExecute()
        {
            if (SelectedFiles == null)
            {
                dialogService.ShowMessageBox(
                    this,
                    "Maybe try selecting a file before attempting to remove one?",
                    "Nope...",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);

                logger.Debug("User didn't select any files to be removed.");
                return;
            }

            foreach (var selectedFile in SelectedFiles)
            {
                FileList.Remove((string)selectedFile);
            }
        }

        /// <summary>
        /// Gets the start stop command.
        /// </summary>
        /// <value>
        /// The start stop command.
        /// </value>
        /// TODO Edit XML Comment Template for StartStopCommand
        public ICommand StartStopCommand { get; private set; }

        /// <summary>
        /// Called when the StartStopCommand is executed.
        /// </summary>
        private void StartStopCommandExecute()
        {
            // Start.
            if (!IsRunning)
            {
                CreateTimer();
            }
            // Stop.
            else
            {
                timer.Stop();
            }

            IsRunning = !IsRunning;
            StartStopContent = IsRunning ? "Stop" : "Start";
        }

        #endregion Commands.

        #region Event Handlers

        // Timer Events.

        private void Timer_OnTimerComplete(object sender, TimerEventArgs e)
        {
            // Play Audio.
            PlayAudioFile();
        }

        private void AudioHandler_OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            // Please stop memory leaks. They hurt my feelings.
            audioHandler.PlaybackStopped -= AudioHandler_OnPlaybackStopped;
            audioHandler.Dispose();

            // Restart Timer.
            timer.TimerComplete -= Timer_OnTimerComplete;
            CreateTimer();
        }

        #endregion Event Handlers

        #region Methods.

        private void CreateTimer()
        {
            var timeSpan = TimeSpan.FromSeconds(rand.Next(MinMinutes * 60 + MinSeconds,
                MaxMinutes * 60 + MaxSeconds));

            timer = new TimerHandler(1, timeSpan);

            // Event Handlers.
            timer.TimerComplete += Timer_OnTimerComplete;

            timer.Start();
        }

        private void PlayAudioFile()
        {
            var fileSelected = FileList.ElementAt(rand.Next(FileList.Count()));

            audioHandler = new AudioHandler(fileSelected, Volume);
            audioHandler.PlaybackStopped += AudioHandler_OnPlaybackStopped;
            audioHandler.Play();
        }

        /// <summary>
        /// Unregisters this instance from the Messenger class.
        /// <para>To cleanup additional resources, override this method, clean
        /// up and then call base.Cleanup().</para>
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
        }

        #endregion Methods.
    }
}