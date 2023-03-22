using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ShizoGames.ShizoPrefs.Providers
{
    internal static class QuaternionProviders
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            ShizoPrefs.AddProvider<Quaternion>(GetQuaternion, SetQuaternion);
        }
        
        private static void SetQuaternion(string key, Quaternion value)
        {
            PlayerPrefs.SetString(key, value.ToString("F3"));
        }
        
        private static Quaternion GetQuaternion(string key, Quaternion defaultValue = default)
        {
            var quaternionString = PlayerPrefs.GetString(key, defaultValue.ToString());
            
            var quaternionMatch = Regex.Match(quaternionString, 
                @"^\(([0-9]+(?>[.][0-9]+)?),\s?([0-9]+(?>[.][0-9]+)?),\s?([0-9]+(?>[.][0-9]+)?),\s?([0-9]+(?>[.][0-9]+)?)\)$");
            
            if (!quaternionMatch.Success)
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogWarning(
                    $"PlayerPrefs <color=yellow>\"{key}\"</color> value is incorrect <color=red>\"{quaternionString}\"</color>");
                #endif
                
                PlayerPrefs.DeleteKey(key);
                return default;
            }
            
            return new Quaternion(
                float.Parse(quaternionMatch.Groups[1].Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(quaternionMatch.Groups[2].Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(quaternionMatch.Groups[3].Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(quaternionMatch.Groups[4].Value, CultureInfo.InvariantCulture.NumberFormat));
        }
    }
}