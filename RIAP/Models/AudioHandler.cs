// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 28-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 08-12-2020
// ***********************************************************************
// <copyright file="AudioHandler.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Usings.

using System;
using NAudio.Wave;

#endregion Usings.

namespace JaINTP.RIAP.Models
{
    /// <summary>
    /// Basic class to handle the playing of an audio file.
    /// </summary>
    public class AudioHandler : IDisposable
    {
        #region Constructors.

        /// <summary>
        ///     Initializes a new instance of the AudioHandler class.
        /// </summary>
        /// <param name="filePath">The fully qualified path of an audio file.</param>
        public AudioHandler(string filePath) : this(filePath, 1.0f)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the AudioHandler class.
        /// </summary>
        /// <param name="filePath">The fully qualified path of an audio file.</param>
        /// <param name="volume">The volume to play the audio file at.</param>
        public AudioHandler(string filePath, float volume)
        {
            this.filePath = filePath;
            this.volume = volume;
        }

        #endregion Constructors.

        #region Fields.

        private AudioFileReader fileReader;
        private WaveOutEvent outputDevice;
        private string filePath;
        private float volume;
        private bool disposed = false;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        #endregion Fields.

        #region Properties.

        /// <summary>
        /// Gets or sets file path of the audio file to play.
        /// </summary>
        /// <value>
        /// A fully qualified file path;
        /// </value>
        public string FilePath
        {
            get => filePath;
            set => filePath = value;
        }

        /// <summary>
        /// Gets or sets the volume for the device 1.0 is max.
        /// </summary>
        /// <value>
        /// Float representing the volume of this device.
        /// </value>
        public float Volume
        {
            get => volume;
            set => volume = value;
        }

        #endregion Properties.

        #region Events.

        /// <summary>
        /// Occurs when the playback has stopped automatically.
        /// </summary>
        public event EventHandler<StoppedEventArgs> PlaybackStopped;

        #endregion Events.

        #region Event Handlers.

        /// <summary>
        /// Called when the playback has stopped automatically.
        /// </summary>
        /// <param name="sender">The <see cref="WaveOutEvent"/>.</param>
        /// <param name="e">The <see cref="StoppedEventArgs" /> instance containing the event data.</param>
        private void WaveOutEvent_OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            OnPlaybackStopped(e);
        }

        /// <summary>
        /// Called when the playback has stopped automatically in order to invoke the appropriate event handlers.
        /// </summary>
        /// <param name="e">The <see cref="StoppedEventArgs" /> instance containing the event data.</param>
        private void OnPlaybackStopped(StoppedEventArgs e)
        {
            PlaybackStopped?.Invoke(this, new StoppedEventArgs(e.Exception));
        }

        #endregion Event Handlers.

        #region Methods.

        /// <summary>
        /// Plays the audio file.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public void Play()
        {
            fileReader = new AudioFileReader(FilePath);
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += WaveOutEvent_OnPlaybackStopped;
            outputDevice.Volume = Volume;

            try
            {
                outputDevice.Init(fileReader);
                outputDevice.Play();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error attempting to play audio file.");
            }
        }

        #endregion Methods.

        #region IDispose Interface.

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                fileReader?.Dispose();
                outputDevice?.Dispose();
            }

            disposed = true;
        }

        #endregion IDispose Interface.
    }
}