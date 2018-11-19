using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Vector2 velocity;

    public float smootTimeX;
    public float smootTimeY;
    public GameObject player;
    public Vector3 initialPosition;

    private void Start()
    {
        transform.position = initialPosition;
    }

    // Update is called once per frame
    void LateUpdate () {
		if(player != null)
        {
            MoveCamera();
        }

	}

    public void MoveCamera()
    {
        Vector3 newDirection = Vector3.zero;
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smootTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smootTimeY);

        newDirection.z = transform.position.z;
        if (posX > initialPosition.x)
        {
            newDirection.x = posX;
        }
        else
        {
            newDirection.x = transform.position.x;
        }
        
        //if (posY > initialPosition.y)
        //{
        //    newDirection.y = posY;
        //}
        //else
        //{
            newDirection.y = transform.position.y;

        //}

        transform.position = newDirection;
    }
    
}
