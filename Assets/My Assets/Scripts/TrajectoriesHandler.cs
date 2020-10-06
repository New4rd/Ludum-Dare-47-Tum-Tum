using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TrajectoriesHandler : MonoBehaviour
{
    static private TrajectoriesHandler _inst;
    static public TrajectoriesHandler Inst
    {
        get { return _inst; }
    }


    public List<List<Vector3>> positionsList;
    public List<List<Vector3>> rotationsList;

    private List<Vector3> currentPositionsList;
    private List<Vector3> currentRotationsList;

    private void Awake()
    {
        _inst = this;
        DeleteOldTrajectories();
    }

    private void Update()
    {
        if (GameManager.Inst.gameIsRunning)
        {
            Vector3 playerPos = GameManager.Inst.player.transform.position;
            Vector3 playerRot = GameManager.Inst.player.transform.rotation.eulerAngles;
            currentPositionsList.Add(playerPos);
            currentRotationsList.Add(playerRot);
        }
    }

    public void AddGhostTransforms ()
    {
        positionsList.Add(currentPositionsList);
        rotationsList.Add(currentRotationsList);
    }

    public void CreateTrajectoryLists()
    {
        currentPositionsList = new List<Vector3>();
        currentRotationsList = new List<Vector3>();
    }

    public void DeleteOldTrajectories ()
    {
        positionsList = new List<List<Vector3>>();
        rotationsList = new List<List<Vector3>>();
        CreateTrajectoryLists();
    }
}
