using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for detecting player collision with a ghost.
/// This script must be placed on the player.
/// </summary>

public class GhostCollision : MonoBehaviour
{

    private Collider2D _collidedTarget;
    public Collider2D collidedTarget
    {
        get { return _collidedTarget; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ghost")
        {
            _collidedTarget = collision;
        }
    }

    public void ResetCollider()
    {
        _collidedTarget = null;
    }
}
