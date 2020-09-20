using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Ciber_Turtle.IO
{
	public static class IO
	{
		public enum EncodingMethod
		{
			Default,
			ASCII,
			Unicode,
			UTF32,
			UTF7,
			UTF8,
		}

		#region Varibles > Public
		public static string BASE_PATH
		{
			get => m_BASE_PATH;
			set
			{
				if (value.Last() == '/') m_BASE_PATH = value;
				else m_BASE_PATH = $"{value}/";
			}
		}
		public static bool enableBasicEncyption = false;
		public static EncodingMethod encoding = EncodingMethod.ASCII;
		// public static Thread writeThread { get => m_writeThread; }
		#endregion

		#region Varibles Private
		static string m_BASE_PATH = $"{Application.dataPath}/";
		// static Thread m_writeThread;
		#endregion

		public static void CreateFolder(string path)
		{
			Directory.CreateDirectory(BASE_PATH + path);
		}

		public static void RenameFolder(string path, string newName)
		{
			Directory.Move(BASE_PATH + path, BASE_PATH + path.Remove(path.Length - path.Split('/').Last().Length) + newName);
		}

		public static void RenameFile(string path, string newName)
		{
			File.Move(BASE_PATH + path, BASE_PATH + path.Remove(path.Length - path.Split('/').Last().Length) + newName);
		}

		public static void WriteString(string path, string data, bool encrypt = false)
		{
			if (encrypt)
			{
				File.WriteAllText(path, Encrypt(data));
			}
			else
			{
				File.WriteAllText(path, data);
			}
		}

		public static void WriteStringAsync(string path, string data, bool encrypt = false)
		{
			if (encrypt)
			{
				File.WriteAllText(path, Encrypt(data));
			}
			else
			{
				File.WriteAllText(path, data);
			}
		}

		public static string ReadString(string path, bool encrypt = false)
		{
			if (encrypt)
			{
				return Decrypt(File.ReadAllText(BASE_PATH + path));
			}
			else
			{
				return File.ReadAllText(BASE_PATH + path);
			}
		}

		/// <summary>
		/// 	Converts an object to json and writes it to a file in the given path
		/// </summary>
		/// <param name="path">Path to save the object (includes the name and extention of the file and starts with <c>BASE_PATH</c>)</param>
		/// <param name="encrypt">Set true to kinda encrypt the file</param>
		/// <param name="obj">Object to save</param>
		public static void WriteJson(string path, object obj, bool encrypt = false)
		{
			System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
			time.Start();

			WriteString(BASE_PATH + path, JsonUtility.ToJson(obj), encrypt);

			time.Stop();
			Debug.Log($"{time.ElapsedMilliseconds}ms");
		}

		/// <summary>
		/// 	Reads a file at the given path and converts it into the object based on the json
		/// </summary>
		/// <param name="path">Path the file is stored (includes the name and extention of the file and starts with <c>BASE_PATH</c>)</param>
		/// <param name="encrypted">Set true to dencrypt the file</param>
		/// <typeparam name="T">The object to convert the json to</typeparam>
		/// <returns>The object with it's parameters set to the json data</returns>
		public static T ReadJson<T>(string path, bool encrypted = false)
		{
			System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
			time.Start();

			T data = JsonUtility.FromJson<T>(ReadString(BASE_PATH + path, encrypted));

			time.Stop();
			Debug.Log($"{time.ElapsedMilliseconds}ms");

			return data;
		}

		/// <summary>
		///   Encrypts a string of text into a string bytes.
		/// </summary>
		/// <param name="text">Text to encrypt</param>
		/// <returns>String of text</returns>
		public static string Encrypt(string text)
		{
			byte[] bytes = new Byte[0];
			switch (encoding)
			{
				case EncodingMethod.ASCII:
					bytes = Encoding.UTF8.GetBytes(text);
					break;
				case EncodingMethod.Unicode:
					bytes = Encoding.Unicode.GetBytes(text);
					break;
				case EncodingMethod.UTF32:
					bytes = Encoding.UTF32.GetBytes(text);
					break;
				case EncodingMethod.UTF7:
					bytes = Encoding.UTF7.GetBytes(text);
					break;
				case EncodingMethod.UTF8:
					bytes = Encoding.UTF8.GetBytes(text);
					break;
				default:
					bytes = Encoding.Default.GetBytes(text);
					break;
			}

			string checker = Convert.ToByte((Mathf.FloorToInt(bytes.Select(x => Convert.ToInt32(x)).Sum()) % 255)).ToString();

			Debug.Log($"{string.Join(' '.ToString(), bytes.Select(x => (x + 1).ToString()).ToArray())} {checker}");
			return $"{string.Join(' '.ToString(), bytes.Select(x => (x + 1).ToString()).ToArray())} {checker}";
		}

		/// <summary>
		///   Decrypts a string of bytes to a string of text.
		/// </summary>
		/// <param name="text">Encrypted string of bytes</param>
		/// <returns>Decrypted string of text</returns>
		public static string Decrypt(string text)
		{
			byte[] bytes = text.Split(' ').Select(x => Convert.ToByte(Convert.ToInt32(x) - 1)).ToArray();
			int num = bytes.Last();
			int sum = (Mathf.FloorToInt(bytes.Select(x => Convert.ToInt32(x)).Sum()) % 255) - (num + 1);
			if (sum == num)
			{
				bytes = bytes.Take(bytes.Length - 1).ToArray();
				switch (encoding)
				{
					case EncodingMethod.ASCII:
						return Encoding.ASCII.GetString(bytes);
					case EncodingMethod.Unicode:
						return Encoding.Unicode.GetString(bytes);
					case EncodingMethod.UTF32:
						return Encoding.UTF32.GetString(bytes);
					case EncodingMethod.UTF7:
						return Encoding.UTF7.GetString(bytes);
					case EncodingMethod.UTF8:
						return Encoding.UTF8.GetString(bytes);
					default:
						return Encoding.Default.GetString(bytes);
				}
			}
			else
			{
				Debug.LogError("<color=red>File is Corrupt</color>");
				return null;
			}
		}
	}
}