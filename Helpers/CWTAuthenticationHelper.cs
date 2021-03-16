using CWTDesktopDatabase.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CWTAuthentication
{
	class CWTAuthenticationHelper
	{
		private string _keyPhrase = ConfigurationManager.AppSettings["AuthenticationPassPhrase"];
		private byte[] _saltValueBytes = { (byte)0xA9, (byte)0x9B, (byte)0xC8, (byte)0x32, (byte)0x56, (byte)0x35, (byte)0xE3, (byte)0x03 };
		private int _iterationCount = 19;

		LogRepository logRepository = new LogRepository();

		/// <summary>
		/// Encrypt url to simulate CWT Authentication
		/// </summary>
		/// <param name="encodedParameter">The 'a' parameter</param>
		/// <returns></returns>
		public string EncryptURL(string querystring)
		{
			byte[] rawPlaintext = System.Text.Encoding.Unicode.GetBytes(querystring);

			byte[] encryptedString;

			Rfc2898DeriveBytes key1 = new Rfc2898DeriveBytes(_keyPhrase, _saltValueBytes);
			key1.IterationCount = _iterationCount;
			byte[] keyBytes = key1.GetBytes(16);
			byte[] vectorBytes = new byte[16];

			// Create a new instance of the RijndaelManaged
			using (var algorithm = new RijndaelManaged())
			{
				algorithm.KeySize = 256;
				algorithm.BlockSize = 128;
				algorithm.Padding = PaddingMode.PKCS7;
				algorithm.Key = keyBytes;
				algorithm.IV = vectorBytes;

				// Encrypt the string to an array of bytes.
				encryptedString = EncryptStringToBytes(querystring, algorithm.Key, algorithm.IV);
			}

			return Convert.ToBase64String(encryptedString);
		}

		private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
		{
			byte[] encrypted;

			// Create an RijndaelManaged object with the specified key and IV.
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.Key = key;
				rijAlg.IV = iv;

				// Create a decrytor to perform the stream transform.
				ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

				// Create the streams used for encryption.
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							//Write all data to the stream.
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}

			// Return the encrypted bytes from the memory stream.
			return encrypted;
		}

		public string Decrypt(string value)
		{
			string decryptedPhrase = string.Empty;

			try
			{
				value = value.Replace(" ", "+");
				byte[] cipherText = Convert.FromBase64String(value);

				Rfc2898DeriveBytes key1 = new Rfc2898DeriveBytes(_keyPhrase, _saltValueBytes);
				key1.IterationCount = _iterationCount;
				byte[] keyBytes = key1.GetBytes(16);
				byte[] vectorBytes = new byte[16];

				decryptedPhrase = DecryptDataAES(cipherText, keyBytes, vectorBytes);

			}
			catch (Exception ex)
			{
				logRepository.LogError(ex.Message);
			}

			return decryptedPhrase;
		}

		private string DecryptDataAES(byte[] cipherText, byte[] key, byte[] iv)
		{
			string plainText = "";

			try
			{
				using (Rijndael rijndael = Rijndael.Create())
				{
					rijndael.Key = key;
					rijndael.IV = iv;
					rijndael.Padding = PaddingMode.PKCS7;

					ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

					using (MemoryStream msDecrypt = new MemoryStream(cipherText))
					{
						using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							using (StreamReader srDecrypt = new StreamReader(csDecrypt))
							{
								plainText = srDecrypt.ReadToEnd().Trim();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				logRepository.LogError(ex.Message);
			}

			return plainText;
		}
	}
}