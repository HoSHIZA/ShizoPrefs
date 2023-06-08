using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ShizoGames.ShizoPrefs.Providers
{
    internal static class EnumProviders
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            ShizoPrefs.AddProvider<Enum>(GetEnum, SetEnum);
        }

        private static void SetEnum<T>(string key, T value) where T : Enum
        {
            PlayerPrefs.SetString(key, $"{value.GetType().Name} {value.ToString()}");
        }

        private static TEnum GetEnum<TEnum>(string key, TEnum defaultValue = default)
        {
            if (typeof(TEnum).BaseType != typeof(Enum))
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogError($"GetEnum<T> accepts only Enum");
                #endif
                
                return default;
            }
            
            
            var enumString = PlayerPrefs.GetString(key, $"{defaultValue.GetType().Name} {defaultValue.ToString()}");
            
            var enumMatch = Regex.Match(enumString, @"^([a-zA-Z]{1}[\w0-9]*)\s+([\w0-9]+)$");
            
            if (!enumMatch.Success)
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogWarning(
                    $"PlayerPrefs <color=yellow>\"{key}\"</color> value is incorrect <color=red>\"{enumString}\"</color>");
                #endif
                
                PlayerPrefs.DeleteKey(key);
                return default;
            }

            if (typeof(TEnum).Name != enumMatch.Groups[1].Value)
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogError($"PlayerPrefs <color=yellow>\"{key}\"</color> contains another Enum Type ({typeof(TEnum).Name} => {enumMatch.Groups[1].Value})");
                #endif
                
                return default;
            }
            
            return (TEnum) Enum.Parse(typeof(TEnum), enumMatch.Groups[2].Value, true);
        }
    }
}