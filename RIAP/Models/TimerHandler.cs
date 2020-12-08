// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 28-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 28-11-2020
// ***********************************************************************
// <copyright file="TimerHandler.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Usings.

using System;
using System.Windows.Threading;

#endregion Usings.

namespace JaINTP.RIAP.Models
{
    /// <summary>
    /// Basic class to handle the manipulation of a DispatcherTimer.
    /// </summary>
    public class TimerHandler
    {
        #region Fields

        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private TimeSpan totalDuration;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the TimerHandler class.
        /// </summary>
        /// <param name="interval">The interval between timer ticks.</param>
        /// <param name="duration">The total duration of the timer.</param>
        public TimerHandler(double interval, TimeSpan duration)
        {
            dispatcherTimer.Interval = TimeSpan.FromSeconds(interval);
            dispatcherTimer.Tick += DispatcherTimer_OnTick;

            TotalDuration = duration;
            Reset();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets the time between ticks.
        /// </summary>
        /// <value>The time between ticks.</value>
        public TimeSpan TimerInterval
        {
            get => dispatcherTimer.Interval;
            set => dispatcherTimer.Interval = value;
        }

        /// <summary>
        ///     Gets the remaining time of the timer.
        /// </summary>
        /// <value>The remaining time of the timer.</value>
        public TimeSpan TimeRemaining { get; private set; }

        /// <summary>
        ///     Gets the total duration of the timer.
        /// </summary>
        /// <value>The total duration of the timer.</value>
        public TimeSpan TotalDuration
        {
            get => totalDuration;
            set
            {
                if (totalDuration != value)
                {
                    totalDuration = value;
                    Reset();
                }
            }
        }

        /// <summary>
        ///     Gets whether the timer is complete.
        /// </summary>
        /// <value>Whether the timer is complete.</value>
        public bool Completed
        {
            get => TimeRemaining <= TimeSpan.Zero;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            dispatcherTimer.Start();
            OnStart();
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void Stop()
        {
            dispatcherTimer.Stop();
            OnStop();
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void Reset()
        {
            Stop();
            TimeRemaining = TotalDuration;
            OnReset();
        }

        #endregion Methods

        #region Events

        /// <summary>
        /// The TimerTick event.
        /// Called for each tick of the timer.
        /// </summary>
        public event EventHandler<TimerEventArgs> TimerTick;

        /// <summary>
        /// The TimerStarted event.
        /// Called  when the timer is started.
        /// </summary>
        public event EventHandler<TimerEventArgs> TimerStarted;

        /// <summary>
        /// The TimerStopped event.
        /// Called when the timer is stopped.
        /// </summary>
        public event EventHandler<TimerEventArgs> TimerStopped;

        /// <summary>
        /// The TimerReset event.
        /// Called when the timer is reset.
        /// </summary>
        public event EventHandler<TimerEventArgs> TimerReset;

        /// <summary>
        /// The TimerComplete event.
        /// Called once the timer has completed its job.
        /// </summary>
        public event EventHandler<TimerEventArgs> TimerComplete;

        #endregion Events

        #region Event Handlers

        /// <summary>
        /// Called on each tick of the <see cref="DispatcherTimer"/>
        /// </summary>
        /// <param name="sender">The <see cref="DispatcherTimer"/>.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void DispatcherTimer_OnTick(object sender, EventArgs e)
        {
            TimeRemaining -= TimerInterval;
            OnTick();

            if (Completed)
            {
                Stop();
                TimeRemaining = TimeSpan.Zero;
                OnComplete();
            }
        }

        /// <summary>
        /// Called on each tick of the timer in order to invoke the appropriate event handlers.
        /// </summary>
        private void OnTick()
        {
            TimerTick?.Invoke(this, new TimerEventArgs(TotalDuration, TimeRemaining));
        }

        /// <summary>
        /// Called when the timer has started in order to invoke the appropriate event handlers.
        /// </summary>
        private void OnStart()
        {
            TimerStarted?.Invoke(this, new TimerEventArgs(TotalDuration, TimeRemaining));
        }

        /// <summary>
        /// Called when the timer is stopped in order to invoke the appropriate event handlers.
        /// </summary>
        private void OnStop()
        {
            TimerStopped?.Invoke(this, new TimerEventArgs(TotalDuration, TimeRemaining));
        }

        /// <summary>
        /// Called when the timer is reset in order to invoke the appropriate event handlers.
        /// </summary>
        private void OnReset()
        {
            TimerReset?.Invoke(this, new TimerEventArgs(TotalDuration, TimeRemaining));
        }

        /// <summary>
        /// Called when the timer completes its work in order to invoke the appropriate event handlers.
        /// </summary>
        private void OnComplete()
        {
            TimerComplete?.Invoke(this, new TimerEventArgs(TotalDuration, TimeRemaining));
        }

        #endregion Event Handlers
    }
}