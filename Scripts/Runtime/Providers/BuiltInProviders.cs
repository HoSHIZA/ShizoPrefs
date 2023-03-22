using UnityEngine;

namespace ShizoGames.ShizoPrefs.Providers
{
    internal static class BuiltInProviders
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            ShizoPrefs.AddProvider<int>(GetInt, SetInt);
            ShizoPrefs.AddProvider<bool>(GetBool, SetBool);
            ShizoPrefs.AddProvider<float>(GetFloat, SetFloat);
            ShizoPrefs.AddProvider<string>(GetString, SetString);
        }
        
        private static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        private static void SetBool(string key, bool value)
        {
            PlayerPrefs.GetInt(key, value ? 1 : 0);
        }

        private static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        private static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        private static int GetInt(string key, int defaultValue = default)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        private static bool GetBool(string key, bool defaultValue = default)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
        }

        private static float GetFloat(string key, float defaultValue = default)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        private static string GetString(string key, string defaultValue = default)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
    }
}