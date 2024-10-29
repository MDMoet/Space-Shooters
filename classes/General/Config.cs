using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Windows;

namespace Space_Shooters.classes.General
{

    internal static class Config
    {
        public static AppConfig Configuration { get; private set; } = new AppConfig();
        private static readonly string EnvVariableKey = "ENCRYPTION_PASSWORD"; // Use an environment variable for the second part

        internal static void InitializeConfiguration()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            if (File.Exists(filePath))
            {
                LoadEncryptedConfiguration(filePath);
            }
            else
            {
                CreateDefaultConfiguration(filePath);
                LoadEncryptedConfiguration(filePath);
            }
        }

        private static void CreateDefaultConfiguration(string filePath)
        {
            // First part: Game-related configuration
            var configPart1 = new
            {
                Default_Enemy_ms_Movement = 150,
                Default_Enemy_ms_Spawning = 7500,
                Entity_Wave_Amount = 1,
                Bullet_Damage_Multiplier = 1,
                KeyDownSubscribed = false,
                EntitiesLeft = 0,
                Base_Experience_Multiplier = 4,
                Progression_Requirement_Multiplier = 1.4,
                Difficulty_Factor_Increase = 1.3,
                Difficulty = 1,
                ConnectionString = "server=192.168.178.204;database=space_shooters;uid=ps250444;pwd=G7!rE2@9wN^zX3#jU1*QkT5&fL0$;"
            };
            var configPart2 = new
            {
                AllowCommandPrompt = true
            };
            // Encrypt both parts
            string jsonPart1 = JsonSerializer.Serialize(configPart1);
            string jsonPart2 = JsonSerializer.Serialize(configPart2);
         
            string encryptedPart1 = CryptoHelper.Encrypt(jsonPart1);
            string encryptedPart2 = CryptoHelper.EncryptWithEnvKey(jsonPart2);

            // Mash the two encrypted parts together
            string combinedEncrypted = encryptedPart1 + "::" + encryptedPart2;
            // Save to config.json
            File.WriteAllText(filePath, combinedEncrypted);
        }

        private static void LoadEncryptedConfiguration(string filePath)
        {
            string combinedEncrypted = File.ReadAllText(filePath);

            // Split the mashed string into two parts
            string[] parts = combinedEncrypted.Split(new[] { "::" }, StringSplitOptions.None);
            if (parts.Length != 2)
            {
                MessageBox.Show("Invalid configuration format.");
                return;
            }

            // Decrypt the first part (game-related config)
            string decryptedPart1 = CryptoHelper.Decrypt(parts[0]);
            string decryptedPart2 = CryptoHelper.DecryptWithEnvKey(parts[1]);
           
            var configPart1 = JsonSerializer.Deserialize<AppConfig>(decryptedPart1);
            if (configPart1 != null)
            {
                
                // Set the main configuration after both parts are processed
                Configuration = configPart1;

                if (!string.IsNullOrEmpty(decryptedPart2))
                {
                    try
                    {
                        var configPart2 = JsonSerializer.Deserialize<AppConfig>(decryptedPart2);
                        Configuration.AllowCommandPrompt = configPart2.AllowCommandPrompt;
                    }
                    catch (JsonException)
                    {
                        MessageBox.Show("Error: Unable to parse AllowCommandPrompt value.");
                    }
                }
            }
        }



        public static class CryptoHelper
        {
            private static readonly byte[] Salt = Encoding.UTF8.GetBytes("YourFixedSaltHere123");

            // Password is split and obfuscated
            private static readonly string[] PasswordSegments = new string[]
            {
            GetSegment1(),
            Environment.UserName, // Use system-specific value to reconstruct
            GetSegment2()
            };

            private static string GetReconstructedPassword()
            {
                var sb = new StringBuilder();
                foreach (var segment in PasswordSegments)
                {
                    sb.Append(segment);
                }
                return sb.ToString();
            }

            private static string GetSegment1() => "MynPjw9";
            private static string GetSegment2() => "NjQosJhnDwZYGZ";

            public static string Encrypt(string plainText)
            {
                using var aes = Aes.Create();
                var key = new Rfc2898DeriveBytes(GetReconstructedPassword(), Salt, 1000, HashAlgorithmName.SHA256);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using var memoryStream = new MemoryStream();
                using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using var writer = new StreamWriter(cryptoStream);
                    writer.Write(plainText);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }

            public static string EncryptWithEnvKey(string plainText)
            {
                string envPassword = Environment.GetEnvironmentVariable(EnvVariableKey);
                if (string.IsNullOrEmpty(envPassword))
                {
                    return string.Empty; // Return silently if environment variable is missing
                }

                using var aes = Aes.Create();
                var key = new Rfc2898DeriveBytes(envPassword, Salt, 1000, HashAlgorithmName.SHA256);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using var memoryStream = new MemoryStream();
                using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using var writer = new StreamWriter(cryptoStream);
                    writer.Write(plainText);
                }

                string encryptedText = Convert.ToBase64String(memoryStream.ToArray());

                // Check if the encrypted text matches the required value
                string requiredEncryptedPart = "SWdpUjUiyUVHGD/USvi3pnPbP06bqB8Jtd97ar/Gb54=";

                // If it doesn't match, return empty string (do it silently)
                if (encryptedText != requiredEncryptedPart)
                {
                    return string.Empty;
                }

                // Return the encrypted text if it matches
                return encryptedText;
            }

            public static string Decrypt(string cipherText)
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using var aes = Aes.Create();
                var key = new Rfc2898DeriveBytes(GetReconstructedPassword(), Salt, 1000, HashAlgorithmName.SHA256);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using var memoryStream = new MemoryStream(cipherBytes);
                using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using var reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }

            public static string DecryptWithEnvKey(string cipherText)
            {
                string envPassword = Environment.GetEnvironmentVariable(EnvVariableKey);
                if (string.IsNullOrEmpty(envPassword))
                {
                    MessageBox.Show("Environment variable for decryption key is missing.");
                    return string.Empty;
                }

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using var aes = Aes.Create();
                var key = new Rfc2898DeriveBytes(envPassword, Salt, 1000, HashAlgorithmName.SHA256);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using var memoryStream = new MemoryStream(cipherBytes);
                using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using var reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }
        }

        public class AppConfig
        {
            public int Default_Enemy_ms_Movement { get; set; }
            public int Default_Enemy_ms_Spawning { get; set; }
            public int Entity_Wave_Amount { get; set; }
            public int Bullet_Damage_Multiplier { get; set; }
            public bool KeyDownSubscribed { get; set; }
            public int EntitiesLeft { get; set; }
            public int Base_Experience_Multiplier { get; set; }
            public double Progression_Requirement_Multiplier { get; set; }
            public double Difficulty_Factor_Increase { get; set; }
            public int Difficulty { get; set; }
            public bool AllowCommandPrompt { get; set; }
            public string ConnectionString { get; set; }
        }
    }
}