using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Global class for the management of the current game.
/// </summary>

public class GameManager : MonoBehaviour
{

    static private GameManager _inst;
    /// <summary>
    /// Game manager instance. This instance is a singleton.
    /// </summary>
    static public GameManager Inst
    {
        get { return _inst; }
    }

    /*****************************************************************************
     * P U B L I C   &   S E R I A L I Z E F I E L D   V A R I A B L E S
     ****************************************************************************/

    /// <summary>
    /// Global object bringing together all the points where the target to
    /// catch can appear in the playing area.
    /// </summary>
    [SerializeField] GameObject PlayerSpawnPointsGlobalObject;

    /// <summary>
    /// Global object bringing together all the points where the target to
    /// catch can appear in the playing area.
    /// Warning: the number of spawn points for the target must be greater
    /// than the number of spawn points for the player.
    /// </summary>
    [SerializeField] GameObject TargetSpawnPointsGlobalObject;

    /// <summary>
    /// Player's object to control. 
    /// </summary>
    public GameObject player;

    /// <summary>
    /// Target that the player must catch.
    /// </summary>
    public GameObject target;

    /// <summary>
    /// Boolean indicating whether the program is currently in the game phase.
    /// </summary>
    public bool gameIsRunning;

    /// <summary>
    /// Number of times the player has completed the game phase.
    /// </summary>
    public int gameIteration = 1;

    /// <summary>
    /// Boolean indicating whether the ghost trajectories should be played in
    /// a loop during the game phase.
    /// </summary>
    public bool loopTrajectories;

    /*****************************************************************************
     * P R I V A T E   V A R I A B L E S
     ****************************************************************************/


    private List<GameObject> playerSpawnPoints;
    private List<GameObject> targetSpawnPoints;

    /// <summary>
    /// Parent object, containing all the ghosts instantiated in the game.
    /// </summary>
    private GameObject parentGhostObject;

    private void Awake()
    {
        _inst = this;
        playerSpawnPoints = new List<GameObject>();
        targetSpawnPoints = new List<GameObject>();
    }


    private void Start()
    {
        FillSpawnPoints();      // We're filling all the spawn points...
        StartCoroutine(Game()); // ... then the game is starting
        gameIteration = 1;
        parentGhostObject = new GameObject("Parent Ghost Object");
    }


    /// <summary>
    /// Function to reset all previously saved game progress.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetGame ()
    {
        // If the success message was previously loaded, we're unloading it
        if (MyScenesManager.Inst.successSceneIsLoaded)
        {
            StartCoroutine(MyScenesManager.Inst.UnloadScene("UI Success Scene"));
            MyScenesManager.Inst.successSceneIsLoaded = false;
        }

        // If the game-over message was previously loaded, we're unloading it
        if (MyScenesManager.Inst.gameOverSceneIsLoaded)
        {
            StartCoroutine(MyScenesManager.Inst.UnloadScene("UI Gameover Scene"));
            MyScenesManager.Inst.gameOverSceneIsLoaded = false;
        }

        // Deleting all the previous ghosts
        DeleteGhosts();

        // Deleting all the previous trajectory files
        TrajectoriesHandler.Inst.DeleteOldTrajectories();

        // Filling all of the player/targer spawn points
        FillSpawnPoints();

        gameIsRunning = false; gameIteration = 1;
        ScoreManager.Inst.UpdateScore(0);

        // Curtains play in
        CurtainsAnimation.Inst.moveIn = true;
        SoundsManager.Inst.PlaySound("Bell");
        yield return new WaitUntil(() => !CurtainsAnimation.Inst.moveIn);

        // Game restarts
        StartCoroutine(Game());
    }


    /// <summary>
    /// the main function of the game. It is an iterative function, remembering
    /// itself until the game is over.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Game ()
    {
        SpawnTarget();

        // We choose a random spawn point from the list
        int randomIndex = Random.Range(0, playerSpawnPoints.Count);
        SpawnOnPoint(playerSpawnPoints[randomIndex]);

        // remove the removed point, to prevent the player from spawning several times in the same place
        playerSpawnPoints.RemoveAt(randomIndex);

        // from the second level, we create the ghost displays
        if (gameIteration > 1) InitializeGhost();


        // If we're on the first level, we pull back the curtain
        if (gameIteration == 1)
        {
            CurtainsAnimation.Inst.moveOut = true;
            yield return new WaitUntil(() => !CurtainsAnimation.Inst.moveOut);
        }

        // the player must move for the game to start
        yield return new WaitUntil(() =>
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow));

        gameIsRunning = true;

        // we wait for the player to catch the mouse or meet a ghost
        yield return new WaitUntil(()
            =>
            target.GetComponent<TargetCollision>().collidedTarget != null ||
            player.GetComponent<GhostCollision>().collidedTarget != null);

        gameIsRunning = false;

        // if the player has encountered a ghost, end of the game
        if (player.GetComponent<GhostCollision>().collidedTarget != null)
        {
            Debug.Log("GAME OVER!");
            CurtainsAnimation.Inst.moveIn = true;
            yield return new WaitUntil(() => !CurtainsAnimation.Inst.moveIn);
            StartCoroutine(MyScenesManager.Inst.LoadScene("UI Gameover Scene"));
            CurtainsAnimation.Inst.moveOut = true;
            yield return new WaitUntil(() => !CurtainsAnimation.Inst.moveOut);
            SoundsManager.Inst.PlaySound("Gameover sound");
            MyScenesManager.Inst.gameOverSceneIsLoaded = true;
            yield break;
        }

        /* Going here means that the player caught the target */

        // Playing meow sound
        SoundsManager.Inst.PlaySound("Meow2");
        TrajectoriesHandler.Inst.AddGhostTransforms();
        TrajectoriesHandler.Inst.CreateTrajectoryLists();
        ScoreManager.Inst.UpdateScore(gameIteration);
        gameIteration++;
        target.GetComponent<TargetCollision>().ResetCollider();

        yield return new WaitForSecondsRealtime(1);

        // If the player has caught all possible targets, he wins
        if (playerSpawnPoints.Count == 0 || targetSpawnPoints.Count == 0)
        {
            Debug.Log("SUCCESS!");
            StartCoroutine(MyScenesManager.Inst.LoadScene("UI Success Scene"));
            SoundsManager.Inst.PlaySound("TADA");
            MyScenesManager.Inst.successSceneIsLoaded = true;
            yield break;
        }

        // Otherwise, we restart the game and go to the next iteration
        StartCoroutine(Game());
    }


    /// <summary>
    /// This function retrieves all the spawn points stored in the
    /// "super-objects" passed as parameters, to store them in the lists
    /// local to the script.
    /// </summary>
    private void FillSpawnPoints ()
    {
        foreach (Transform child in PlayerSpawnPointsGlobalObject.transform)
        {
            playerSpawnPoints.Add(child.gameObject);
        }

        foreach (Transform child in TargetSpawnPointsGlobalObject.transform)
        {
            targetSpawnPoints.Add(child.gameObject);
        }
    }


    private void SpawnOnPoint (GameObject spawnPoint)
    {
        player.transform.position = spawnPoint.transform.position;
    }

    private void InitializeGhost ()
    {
        GameObject ghost = Resources.Load("Prefabs/Ghost Player") as GameObject;
        ghost.GetComponent<FollowTrajectory>().ghostIndex = gameIteration-1;
        GameObject inst = Instantiate(ghost);
        inst.transform.parent = parentGhostObject.transform;
    }

    private void DeleteGhosts()
    {
        foreach (Transform child in parentGhostObject.transform)
        {
            Destroy(child.gameObject);
        }
    }


    /// <summary>
    /// Function that places the target on a spawn point drawn randomly in
    /// the list. The point drawn is then removed from the list.
    /// </summary>
    private void SpawnTarget ()
    {
        int randomIndex = Random.Range(0, targetSpawnPoints.Count);

        Vector3 targetRandomPos = new Vector3(
            targetSpawnPoints[randomIndex].transform.position.x,
            targetSpawnPoints[randomIndex].transform.position.y,
            0);

        target.transform.position = targetRandomPos;
        targetSpawnPoints.RemoveAt(randomIndex);
    }
}
