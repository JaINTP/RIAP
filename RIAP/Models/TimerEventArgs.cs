// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 28-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 28-11-2020
// ***********************************************************************
// <copyright file="TimerEventArgs.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Usings.

using System;

#endregion Usings.

namespace JaINTP.RIAP.Models
{
    /// <summary>
    ///     Provides custom event information regarding Timer Events.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TimerEventArgs : EventArgs
    {
        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the TimerEventArgs class.
        /// </summary>
        /// <param name="totalDuration">The total duration of the timer.</param>
        /// <param name="timeRemaining">The remaining time of the Timer.</param>
        public TimerEventArgs(TimeSpan totalDuration, TimeSpan timeRemaining)
        {
            TotalDuration = totalDuration;
            TimeRemaining = timeRemaining;
        }

        #endregion Constructors.

        #region Properties.

        /// <summary>
        /// Gets or sets the total duration.
        /// </summary>
        /// <value>
        /// The total duration.
        /// </value>
        public TimeSpan TotalDuration { get; set; }

        /// <summary>
        /// Gets or sets the remaining time of the timer.
        /// </summary>
        /// <value>
        /// The remaining time of the timer.
        /// </value>
        public TimeSpan TimeRemaining { get; set; }

        #endregion Properties.
    }
}