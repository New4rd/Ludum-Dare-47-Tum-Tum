using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerMovement : MonoBehaviour
{
    public bool move;
    public float zDistance;


    private void Start()
    {
        if (move)
            GetComponent<Camera>().orthographicSize = zDistance;
    }


    void Update()
    {
        if (move)
        {
            transform.position = new Vector3(
            GameManager.Inst.player.transform.position.x,
            GameManager.Inst.player.transform.position.y,
            transform.position.z);
        }
        
    }
}
