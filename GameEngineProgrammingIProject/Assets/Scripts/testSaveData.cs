using UnityEngine;
using System.Collections;

public class testSaveData : MonoBehaviour {
  private void Start()
    {
        saveData newSaveData = new saveData();
        newSaveData.foo = "Hello foo";
        newSaveData.bar = 10;
        newSaveData.Save("mySave");

        saveData loadedData = saveData.Load("mySave");
        Debug.LogFormat("The loaded has a value of {0} for foo and {1} for bar", loadedData.foo, loadedData.bar);

    }
}
