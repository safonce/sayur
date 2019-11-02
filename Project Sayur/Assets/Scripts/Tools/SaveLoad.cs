using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveLoad
{
	const string fileName = "/PlayerData.dat";

	public static void Save ()
	{
		if (Game.current != null)
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + fileName);
			bf.Serialize (file, Game.current);
			file.Close ();
		}
	}

	public static void Load ()
	{
		if (IsFileExist ())
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
			Game.current = (Game)bf.Deserialize (file);
			file.Close ();
		}
	}

	public static bool IsFileExist ()
	{
		return File.Exists (Application.persistentDataPath + fileName);
	}
}
