using System.Text.RegularExpressions;
using UnityEngine;

namespace ShizoGames.ShizoPrefs.Providers
{
    internal static class ResolutionProviders
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            ShizoPrefs.AddProvider<Resolution>(GetResolution, SetResolution);
        }
        
        private static void SetResolution(string key, Resolution value)
        {
            PlayerPrefs.SetString(key, value.ToString());
            
        }
        
        private static Resolution GetResolution(string key, Resolution defaultValue = default)
        {
            var resolutionString = PlayerPrefs.GetString(key, defaultValue.ToString());
            
            var resolutionMatch = Regex.Match(resolutionString, @"^([0-9]+)\s?x\s?([0-9]+)\s?@\s?([0-9]+)(?:[Hh][Zz])?$");
            
            if (!resolutionMatch.Success)
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogWarning(
                    $"PlayerPrefs <color=yellow>\"{key}\"</color> value is incorrect <color=red>\"{resolutionString}\"</color>");
                #endif
                
                PlayerPrefs.DeleteKey(key);
                return default;
            }
            
            var resolution = new Resolution
            {
                width = int.Parse(resolutionMatch.Groups[1].Value),
                height = int.Parse(resolutionMatch.Groups[2].Value),
#if UNITY_2022_2_OR_NEWER
                refreshRate = int.Parse(resolutionMatch.Groups[3].Value)
#else
                refreshRate = int.Parse(resolutionMatch.Groups[3].Value)
#endif
            };
            
            return resolution;
        }
    }
}