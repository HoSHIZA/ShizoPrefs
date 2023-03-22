using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ShizoGames.ShizoPrefs
{
    /// <summary>
    /// A static class for managing Unity PlayerPrefs using various providers for different types.
    /// </summary>
    public static class ShizoPrefs
    {
        private static readonly Dictionary<Type, PlayerPrefsProvider> _playerPrefsProviders = 
            new Dictionary<Type, PlayerPrefsProvider>(12);
        
        /// <summary>
        /// Sets the value of a key in PlayerPrefs using a specific provider for the given type.
        /// </summary>
        /// <typeparam name="T">The type of the value to be set.</typeparam>
        /// <param name="key">The PlayerPrefs key.</param>
        /// <param name="value">The value to be set.</param>
        public static void Set<T>(string key, T value)
        {
            if (!_playerPrefsProviders.ContainsKey(typeof(T))) return;

            _playerPrefsProviders[typeof(T)].Setter.Invoke(key, value);
        }
        
        /// <summary>
        /// Sets the value of a key in PlayerPrefs using a serialized object.
        /// </summary>
        /// <typeparam name="T">The type of the value to be set.</typeparam>
        /// <param name="key">The PlayerPrefs key.</param>
        /// <param name="value">The value to be set.</param>
        public static void SetSerializable<T>(string key, T value) where T : class, new()
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            
            formatter.Serialize(stream, value);
            PlayerPrefs.SetString(key, Convert.ToBase64String(stream.ToArray()));
        }
        
        /// <summary>
        /// Gets the value of a key from PlayerPrefs using a specific provider for the given type.
        /// </summary>
        /// <typeparam name="T">The type of the value to be retrieved.</typeparam>
        /// <param name="key">The PlayerPrefs key.</param>
        /// <param name="defaultValue">The default value to be returned if the key is not found.</param>
        /// <returns>Returns the value of the given key, or the default value if the key is not found.</returns>
        public static T Get<T>(string key, T defaultValue = default)
        {
            if (!_playerPrefsProviders.ContainsKey(typeof(T))) return default;
            
            return (T) _playerPrefsProviders[typeof(T)].Getter.Invoke(key, defaultValue);
        }
        
        /// <summary>
        /// Gets the value of a key from PlayerPrefs using a serialized object.
        /// </summary>
        /// <typeparam name="T">The type of the value to be retrieved.</typeparam>
        /// <param name="key">The PlayerPrefs key.</param>
        /// <param name="defaultValue">The default value to be returned if the key is not found.</param>
        /// <returns>Returns the value of the given key, or the default value if the key is not found.</returns>
        public static T GetSerializable<T>(string key, T defaultValue = default) where T : class, new()
        {
            var value = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            
            var formatter = new BinaryFormatter();
            var data = Convert.FromBase64String(value);
            var stream = new MemoryStream(data);

            return (T)formatter.Deserialize(stream);
        }
        
        /// <summary>
        /// Adds a new provider for a specific type to the list of available PlayerPrefs providers.
        /// </summary>
        /// <typeparam name="T">The type of the value that the provider handles.</typeparam>
        /// <param name="getter">A function that retrieves a value from PlayerPrefs for the given key.</param>
        /// <param name="setter">An action that sets a value in PlayerPrefs for the given key.</param>
        public static void AddProvider<T>(Func<string, T, T> getter, Action<string, T> setter)
        {
            AddProvider(typeof(T), 
                (key, value) => getter(key, (T) value), 
                (key, value) => setter(key, (T) value));
        }
        
        /// <summary>
        /// Adds a new provider for a specific type to the list of available PlayerPrefs providers.
        /// </summary>
        /// <param name="type">The type of the value that the provider handles.</param>
        /// <param name="getter">A function that retrieves a value from PlayerPrefs for the given key.</param>
        /// <param name="setter">An action that sets a value in PlayerPrefs for the given key.</param>
        public static void AddProvider(Type type, Func<string, object, object> getter, Action<string, object> setter)
        {
            if (_playerPrefsProviders.ContainsKey(type)) return;
            
            _playerPrefsProviders.Add(type, new PlayerPrefsProvider(getter, setter));
        }

        /// <summary>
        /// Saves all PlayerPrefs to storage.
        /// </summary>
        public static void Save()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Deletes all PlayerPrefs keys and values.
        /// </summary>
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// Removes the specified key and its associated value from the preferences.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
        
        /// <summary>
        /// Checks if the specified key exists in the preferences.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns true if the key exists in the preferences; otherwise false.</returns>
        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
        
        private class PlayerPrefsProvider
        {
            public Func<string, object, object> Getter { get; }
            public Action<string, object> Setter { get; }
            
            public PlayerPrefsProvider(Func<string, object, object> getter, Action<string, object> setter)
            {
                Getter = getter;
                Setter = setter;
            }
        }
    }
}