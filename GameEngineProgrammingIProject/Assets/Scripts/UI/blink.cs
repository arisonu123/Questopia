using UnityEngine;
using System.Collections;

public class blink : MonoBehaviour
{
    [SerializeField]
#pragma warning disable 649
    private float speed;

#pragma warning restore 649
    private float timer;


    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime * speed;
        if (timer >= .15f)
        {
            transform.GetComponent<SpriteRenderer>().enabled = true;
        }
        if (timer >= .7f)
        {
            transform.GetComponent<SpriteRenderer>().enabled = false;
            timer = 0;
        }
    }
}
