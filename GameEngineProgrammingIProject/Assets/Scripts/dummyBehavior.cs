using UnityEngine;
using System.Collections;

public class dummyBehavior : agent
{
    private void Start()
    {
        Toolbox.UI.registerDummy(this);
    }


    /// <summary>
    /// Plays the hit animation for the training dummy
    /// </summary>
    public void playHitAni()
    {
        this.gameObject.GetComponent<Animation>().Play("Hit");
    }

    protected override void HandleOnDie()
    {
        this.gameObject.GetComponentInParent<dummySpawn>().spawnAfterTime();
        base.HandleOnDie();
    }
}