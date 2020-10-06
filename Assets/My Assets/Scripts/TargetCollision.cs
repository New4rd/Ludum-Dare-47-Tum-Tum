using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollision : MonoBehaviour
{
    private Collider2D _collidedTarget;
    public Collider2D collidedTarget
    {
        get { return _collidedTarget; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Si le script se trouve sur l'objet CIBLE
        if (gameObject.tag == "Target")
        {
            // Si le joueur rencontre la cible, elle disparaît
            if (collision.transform.tag == "Player")
            {
                _collidedTarget = collision;
                transform.position = new Vector3(100, 100, 0);
            }
        }
    }

    public void ResetCollider ()
    {
        _collidedTarget = null;
    }
}
