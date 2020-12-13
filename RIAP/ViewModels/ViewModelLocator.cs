// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 14-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 14-11-2020
// ***********************************************************************
// <copyright file="ViewModelLocator.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Usings

using System.Diagnostics.CodeAnalysis;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MvvmDialogs;

#endregion Usings

namespace JaINTP.RIAP.ViewModels
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    ///     <para>
    ///         See http://www.mvvmlight.net
    ///     </para>
    /// </summary>
    public class ViewModelLocator
    {
        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="T:JaINTP.RIAP.ViewModels.ViewModelLocator" /> class.</summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Services.
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService());

            // ViewModels.
            SimpleIoc.Default.Register<MainViewModel>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        ///     Gets the Main property.
        /// </summary>
        /// <value>The main view model.</value>
        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }

        #endregion Methods
    }
}