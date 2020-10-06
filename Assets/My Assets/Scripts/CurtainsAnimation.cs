using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainsAnimation : MonoBehaviour
{
    static private CurtainsAnimation _inst;
    static public CurtainsAnimation Inst
    {
        get { return _inst; }
    }

    [SerializeField] float yTopLimit;
    [SerializeField] float yBottomLimit;

    public bool moveIn;
    public bool moveOut;
    public float speed;

    private void Awake()
    {
        _inst = this;
    }

    private void Update()
    {
        if (moveIn)
        {
            if (transform.position.y > yBottomLimit)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y - speed,
                    transform.position.z);

                if (transform.position.y == yBottomLimit)
                {
                    moveIn = false;
                }
            }
        }

        if (moveOut)
        {
            if (transform.position.y < yTopLimit)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y + speed,
                    transform.position.z);

                if (transform.position.y == yTopLimit)
                {
                    moveOut = false;
                }
            }
        }
    }
}
