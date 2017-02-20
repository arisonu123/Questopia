using UnityEngine;
using System.Collections;

public class cameraController :MonoBehaviour {


    private Vector3 moveOffset;



    [Header("Camera control settings")]
    [SerializeField]
    private float minFov= 15f;
    [SerializeField]
    private float maxFov= 90f;
    [SerializeField]
    private float sensitivity= 10f;
    [SerializeField]
    [Tooltip("This should be the vector3 point that the camera starts at in the very first game scene")]
    private Vector3 camStartPoint=new Vector3(3,5,-10);










    private void Update()
    {
        //modify field of view based on mouse scrollwheel inputw
        float fov = Camera.main.fieldOfView;
        if (Toolbox.GameManager.gameRunning)
        {
            fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;
        }

   
    }

	// Update is called once per frame
	private void LateUpdate () {//make camera follow player
        if (!Toolbox.player)
        {
            return;
            
        }

       transform.position = Toolbox.player.transform.position + moveOffset;



    }

    /// <summary>
    /// adjusts the camera on game start
    /// </summary>
    public void gameStart()
    {
        this.transform.position = camStartPoint;
        moveOffset = transform.position - Toolbox.player.transform.position;
    }
}
