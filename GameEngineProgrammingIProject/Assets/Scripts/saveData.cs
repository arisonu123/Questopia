using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class saveData{

    //values to save and load
    public string foo;
    public int bar;

	public void Save(string fileName)
    {
        using (FileStream stream = new FileStream(string.Format("{0}/{1}.save", Application.persistentDataPath, fileName), FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
        }
    }

    public static saveData Load(string fileName)
    {
        using (FileStream stream = new FileStream(string.Format("{0}/{1}.save", Application.persistentDataPath, fileName), FileMode.Open, FileAccess.Read))
        {
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream) as saveData;
        }
    }
}
