using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Class for tracking a path by a phantom, from a .txt file containing a
/// set of positions and rotations. This script should be placed on ghosts.
/// </summary>

public class FollowTrajectory : MonoBehaviour
{
    /// <summary>
    /// Ghost number, which is used to retrieve the correct text file.
    /// </summary>
    public int ghostIndex;

    public SpriteRenderer spriteRenderer;

    private Vector3 position, rotation;
    private List<Vector3> positionsList, rotationsList;
    private int index = 0;


    private void Start()
    {
        positionsList = new List<Vector3>();
        positionsList = TrajectoriesHandler.Inst.positionsList[ghostIndex - 1];
        rotationsList = new List<Vector3>();
        rotationsList = TrajectoriesHandler.Inst.rotationsList[ghostIndex - 1];
    }


    private void Update()
    {
        if (GameManager.Inst.gameIsRunning)
        {
            transform.position = positionsList[index];
            transform.rotation = Quaternion.Euler(rotationsList[index]);
            index++;
        }

        if (!GameManager.Inst.gameIsRunning)
        {
            transform.position = positionsList[0];
            transform.rotation = Quaternion.Euler(rotationsList[0]);
            index = 1;
        }

        if (index == positionsList.Count)
        {
            index = 0;
        }
    }

}
