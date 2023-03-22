using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ShizoGames.ShizoPrefs.Providers
{
    internal static class VectorProviders
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            ShizoPrefs.AddProvider<Vector2>(GetVector2, SetVector2);
            ShizoPrefs.AddProvider<Vector2Int>(GetVector2Int, SetVector2Int);
            ShizoPrefs.AddProvider<Vector3>(GetVector3, SetVector3);
            ShizoPrefs.AddProvider<Vector3Int>(GetVector3Int, SetVector3Int);
            ShizoPrefs.AddProvider<Vector4>(GetVector4, SetVector4);
        }

        private static void SetVector2(string key, Vector2 value)
        {
            PlayerPrefs.SetString(key, value.ToString("F3"));
        }

        private static void SetVector2Int(string key, Vector2Int value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }

        private static void SetVector3(string key, Vector3 value)
        {
            PlayerPrefs.SetString(key, value.ToString("F3"));
        }
        
        private static void SetVector3Int(string key, Vector3Int value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }
        
        private static void SetVector4(string key, Vector4 value)
        {
            PlayerPrefs.SetString(key, value.ToString("F3"));
        }

        private static Vector2 GetVector2(string key, Vector2 defaultValue = default)
        {
            var vectorString = PlayerPrefs.GetString(key, defaultValue.ToString("F3"));
            
            var vectorMatch = Regex.Match(vectorString, @"^\(([0-9]+(?:[.][0-9]+)?),\s?([0-9]+(?:[.][0-9]+)?)\)$");

            if (!vectorMatch.Success)
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogWarning(
                    $"PlayerPrefs <color=yellow>\"{key}\"</color> value is incorrect <color=red>\"{vectorString}\"</color>");
                #endif
                
                PlayerPrefs.DeleteKey(key);
                return default;
            }
            
            return new Vector2(
                float.Parse(vectorMatch.Groups[1].Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(vectorMatch.Groups[2].Value, CultureInfo.InvariantCulture.NumberFormat));
        }

        private static Vector2Int GetVector2Int(string key, Vector2Int defaultValue = default)
        {
            var vectorString = PlayerPrefs.GetString(key, defaultValue.ToString());
            
            var vectorMatch = Regex.Match(vectorString, @"^\(([0-9]+),\s?([0-9]+)\)$");
            
            if (!vectorMatch.Success)
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogWarning(
                    $"PlayerPrefs <color=yellow>\"{key}\"</color> value is incorrect <color=red>\"{vectorString}\"</color>");
                #endif

                PlayerPrefs.DeleteKey(key);
                return default;
            }
            
            return new Vector2Int(
                int.Parse(vectorMatch.Groups[1].Value), 
                int.Parse(vectorMatch.Groups[2].Value));
        }

        private static Vector3 GetVector3(string key, Vector3 defaultValue = default)
        {
            var vectorString = PlayerPrefs.GetString(key, defaultValue.ToString("F3"));
            
            var vectorMatch = 
                Regex.Match(vectorString, @"^\(([0-9]+(?>[.][0-9]+)?),\s?([0-9]+(?>[.][0-9]+)?),\s?([0-9]+(?>[.][0-9]+)?)\)$");
            
            if (!vectorMatch.Success)
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogWarning(
                    $"PlayerPrefs <color=yellow>\"{key}\"</color> value is incorrect <color=red>\"{vectorString}\"</color>");
                #endif
                
                PlayerPrefs.DeleteKey(key);
                return default;
            }
            
            return new Vector3(
                float.Parse(vectorMatch.Groups[1].Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(vectorMatch.Groups[2].Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(vectorMatch.Groups[3].Value, CultureInfo.InvariantCulture.NumberFormat));
        }

        private static Vector3Int GetVector3Int(string key, Vector3Int defaultValue = default)
        {
            var vectorString = PlayerPrefs.GetString(key, defaultValue.ToString());

            var vectorMatch = Regex.Match(vectorString, @"^\(([0-9]+),\s?([0-9]+),\s?([0-9]+)\)$");

            if (!vectorMatch.Success)
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogWarning(
                    $"PlayerPrefs <color=yellow>\"{key}\"</color> value is incorrect <color=red>\"{vectorString}\"</color>");
                #endif

                PlayerPrefs.DeleteKey(key);
                return default;
            }

            return new Vector3Int(
                int.Parse(vectorMatch.Groups[1].Value), 
                int.Parse(vectorMatch.Groups[2].Value),
                int.Parse(vectorMatch.Groups[3].Value));
        }

        private static Vector4 GetVector4(string key, Vector4 defaultValue = default)
        {
            var vectorString = PlayerPrefs.GetString(key, defaultValue.ToString("F3"));
            
            var vectorMatch = Regex.Match(vectorString, 
                @"^\(([0-9]+(?>[.][0-9]+)?),\s?([0-9]+(?>[.][0-9]+)?),\s?([0-9]+(?>[.][0-9]+)?),\s?([0-9]+(?>[.][0-9]+)?)\)$");
            
            if (!vectorMatch.Success)
            {
                #if DEVELOPMENT_BUILD || UNITY_DEBUG
                Debug.LogWarning(
                    $"PlayerPrefs <color=yellow>\"{key}\"</color> value is incorrect <color=red>\"{vectorString}\"</color>");
                #endif
                
                PlayerPrefs.DeleteKey(key);
                return default;
            }
            
            return new Vector4(
                float.Parse(vectorMatch.Groups[1].Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(vectorMatch.Groups[2].Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(vectorMatch.Groups[3].Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(vectorMatch.Groups[4].Value, CultureInfo.InvariantCulture.NumberFormat));
        }
    }
}