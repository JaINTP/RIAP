// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 14-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 13-12-2020
// ***********************************************************************
// <copyright file="SettingsHandler.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Usings

using System.IO;
using Newtonsoft.Json;

#endregion Usings

namespace JaINTP.RIAP.Models
{
    /// <summary>
    /// Class designed to handle the loading and deserializing of json based configuration
    /// files.
    /// Got the idea from https://stackoverflow.com/a/6541739/2695708
    /// </summary>
    /// <typeparam name="T">The Settings Class provided by the user.</typeparam>
    public class SettingsHandler<T> where T : new()
    {
        /// <summary>
        /// Saves the settings file as Json to the specified location.
        /// Creates the parent directory in the path if it doesn't exist.
        /// </summary>
        /// <param name="path">The fully qualified path to the settings file.</param>
        public void Save(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            var jsonSerializer = new JsonSerializer();
            using (var streamWriter = new StreamWriter(path))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonWriter, this);
            }
        }

        /// <summary>
        /// Loads the Json based settings file and creates a settings class from it.
        /// Uses a shitty hack to generate an object of type T via its JsonProperty attributes.
        /// </summary>
        /// <param name="path">The fully qualified path to the settings file.</param>
        public static T Load(string path)
        {
            T t;

            if (File.Exists(path))
            {
                using (var streamReader = new StreamReader(path))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var jsonSerializer = new JsonSerializer();
                    t = jsonSerializer.Deserialize<T>(jsonReader);
                }
            }
            else
            {
                // Cheap hack to have Newtonsoft build out settings file from the JsonProperty.
                // attributes in the Settings class. Will probably throw exceptions if you don't
                // have them...
                t = JsonConvert.DeserializeObject<T>("{}");
            }

            return t;
        }
    }
}