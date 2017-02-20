using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadScenes : MonoBehaviour {
#pragma warning disable 649



    [SerializeField]
    [Tooltip("Game Objects that will be turned on and off")]
    [Header("Game object storage")]
    private GameObject mainCam;
    #pragma warning restore 649
    

    private bool loading;



   
    /// <summary>
    /// Gets whether or not a scene is loading
    /// </summary>
    /// <value>Whether or not a scene is loading</value>
    public bool isLoading
    {
        get { return loading; }
    }

   
   

    /// <summary>
    /// Loads a scene
    /// </summary>
    /// <param name="sceneToLoad">The scene name to load</param>
    public void load(string screenToLoad)
    {
        if (!loading)
        {
            StartCoroutine(loadScene(screenToLoad));
        }
    }
 
    private IEnumerator loadScene(string sceneToLoad)
    {

        loading = true;


        AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
        Toolbox.UI.turnOnPlayerStats();
        Toolbox.QuestManager.activateQuestListUI();
        op.allowSceneActivation = false;
        GameObject scene = GameObject.FindGameObjectWithTag("scene");
        if (scene != null)
        {
            Destroy(scene.gameObject);
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<health>().modify(-4000);
           // Destroy(enemy);
        }

        foreach (Transform trans in Toolbox.invManager.gameObject.transform)
        {
           trans.gameObject.SetActive(false);
        }

       
        Toolbox.GameManager.loadScreen.SetActive(true);
        mainCam.SetActive(false);
        if (Toolbox.player)
        {
            Toolbox.player.enabled = false;
        }
        if (op != null)
        {

            while (op.progress < 0.8)
            {

                yield return null;




            }

        }

      

        op.allowSceneActivation = true;
        while (!op.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }

       


        mainCam.SetActive(true);
        if (Toolbox.player&&sceneToLoad!="mainMenu"&&sceneToLoad!="victoryScreen")
        {
            Toolbox.player.enabled = true;
        }
        Toolbox.GameManager.loadScreen.SetActive(false);
        if (sceneToLoad == "caveScene")
        {
            Toolbox.player.transform.position = GameObject.Find("playerSpawn").transform.position;
            Toolbox.GameManager.spawnPoint = GameObject.Find("playerSpawn").transform;
        }
        else
        {
            if (Toolbox.GameManager.gameRunning == true&&sceneToLoad=="gameScene")//load from cave scene to gameScene
            {
             
                Toolbox.player.transform.position = GameObject.Find("playerSpawn").transform.position;
                Toolbox.GameManager.spawnPoint = GameObject.Find("playerStartSpawn").transform;
              

                Toolbox.player.topLadder = GameObject.Find("LadderCombinationSample Long").transform.Find("topReached").transform.gameObject;
                Toolbox.player.topLadder.SetActive(false);
                Toolbox.player.botLadder = GameObject.Find("LadderCombinationSample Long").transform.Find("bottomReached").transform.gameObject;
                Toolbox.player.botLadder.SetActive(false);

             
            }
            else if (sceneToLoad=="victoryScreen" ||sceneToLoad =="mainMenu")//load from caveScene or gameScene to either victoryScreen or mainMenu
            {
               //do nothing
              
            }
            else//load from main menu to gameScene
            {
          
                Toolbox.GameManager.spawnPoint = GameObject.Find("playerStartSpawn").transform;
                Toolbox.GameManager.startGame();
                Toolbox.player.transform.position = Toolbox.GameManager.spawnPoint.position;

                Toolbox.player.topLadder = GameObject.Find("LadderCombinationSample Long").transform.Find("topReached").transform.gameObject;
                Toolbox.player.topLadder.SetActive(false);
                Toolbox.player.botLadder = GameObject.Find("LadderCombinationSample Long").transform.Find("bottomReached").transform.gameObject;
                Toolbox.player.botLadder.SetActive(false);
            
            }
          
          

        }

        loading = false;
        Toolbox.UI.turnOnPlayerStats();
        Toolbox.QuestManager.activateQuestListUI();
        yield return null;

     
    }


}
