using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteraction : MonoBehaviour
{

    public void ReplayButton ()
    {
        StartCoroutine(GameManager.Inst.ResetGame());
    }
}
