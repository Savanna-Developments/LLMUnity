using System;
using UnityEngine;

namespace LLMUnity
{
    [Serializable]
    public struct LLMSerializedSettings
    {
        private const string PLAYER_PREFS_KEY = "LLMSettings";

        public static string ServerCommand
        {
            set { SetServerCommand(value); }
            get { return Deserialize().serverCommand; }
        }

        [SerializeField] private string serverCommand;
        [SerializeField] private string hardwareHash;
        [SerializeField] private string version;

        public void Serialize()
        {
            string json = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(PLAYER_PREFS_KEY, json);
        }

        private static void SetServerCommand(string value)
        {
            LLMSerializedSettings settings = Deserialize();
            settings.serverCommand = value;
            settings.hardwareHash = LLMUnitySetup.GetHardwareHash();
            settings.version = LLMVersion.VERSION;
            settings.Serialize();
        }

        public static bool HasServerCommand()
        {
            // Check if the key exists
            bool hasKey = PlayerPrefs.HasKey(PLAYER_PREFS_KEY);
            if (!hasKey) return false;

            LLMSerializedSettings settings = Deserialize();
            // Check if the hash matches
            bool hashMatches = settings.hardwareHash.Equals(LLMUnitySetup.GetHardwareHash());
            if (!hashMatches) return false;
            // Check if the version matches
            bool versionMatches = settings.version.Equals(LLMVersion.VERSION);
            if (!versionMatches) return false;

            return true;
        }

        public static LLMSerializedSettings Deserialize()
        {
            if (!HasServerCommand())
            {
                return new LLMSerializedSettings();
            }
            else
            {
                string json = PlayerPrefs.GetString(PLAYER_PREFS_KEY);
                return JsonUtility.FromJson<LLMSerializedSettings>(json);
            }

        }
    }
}
