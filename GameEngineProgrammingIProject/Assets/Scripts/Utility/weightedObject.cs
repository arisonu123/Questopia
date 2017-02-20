using UnityEngine;
using System.Collections;

[System.Serializable]
public class weightedObject{
    [SerializeField]
    [Tooltip("The object selected by this choice.")]
#pragma warning disable 649
    private GameObject value;
#pragma warning restore 649
    [SerializeField]
    [Tooltip("The chance to select the value")]
    private double chance = 1.0f;

    private static System.Random rnd = new System.Random();

    public static GameObject select(weightedObject[] objects)
    {
        double[] cdfArray = new double[objects.Length];
        double total = 0;
        for(int index = 0; index < objects.Length; index++)
        {
            total += objects[index].chance;
            cdfArray[index] = total;
        }

        int selectedIndex = System.Array.BinarySearch(cdfArray, rnd.NextDouble() * total);
        if (selectedIndex < 0)
        {
            selectedIndex = ~selectedIndex;
        }
        return objects[selectedIndex].value;
    }

}
