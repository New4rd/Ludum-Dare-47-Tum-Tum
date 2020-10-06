using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyScenesManager : MonoBehaviour
{
    static private MyScenesManager _inst;
    static public MyScenesManager Inst
    {
        get { return _inst; }
    }

    private bool _successSceneIsLoaded;
    public bool successSceneIsLoaded
    {
        get { return _successSceneIsLoaded; }
        set { _successSceneIsLoaded = value; }
    }

    private bool _gameOverSceneIsLoaded;
    public bool gameOverSceneIsLoaded
    {
        get { return _gameOverSceneIsLoaded; }
        set { _gameOverSceneIsLoaded = value; }
    }

    public Animator fadeAnimator;
    public bool loadOperationDone = false;

    private void Awake()
    {
        _inst = this;
    }

    private IEnumerator Start()
    {
        // Loading the curtains scene for future use
        StartCoroutine(LoadScene("Curtains Scene"));
        yield return new WaitUntil(() => loadOperationDone);

        /******************************************************
         * F I R S T   T I T L E   S C R E E N
         *****************************************************/

        // Loading the first title screen
        StartCoroutine (LoadScene("Title1 Scene"));
        yield return new WaitUntil(() => loadOperationDone);
        // Waiting for the player to press space bar
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        // Triggering the animator: the screen fades out
        fadeAnimator.SetTrigger("Fade Out Trigger");
        yield return new WaitForSecondsRealtime(1);
        // Unloading the first title screen
        StartCoroutine(UnloadScene("Title1 Scene"));
        yield return new WaitUntil(() => loadOperationDone);

        /******************************************************
         * S E C O N D   T I T L E   S C R E E N
         *****************************************************/

        // Loading the second title screen
        StartCoroutine(LoadScene("Title2 Scene"));
        yield return new WaitUntil(() => loadOperationDone);
        // Triggering the animator: the screen fades in
        fadeAnimator.SetTrigger("Fade In Trigger");
        yield return new WaitForSecondsRealtime(1);
        // Waiting for the player to press space bar
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        // Triggering the animator: the screen fades out
        fadeAnimator.SetTrigger("Fade Out Trigger");
        yield return new WaitForSecondsRealtime(1);
        // Unloading the second title screen
        StartCoroutine(UnloadScene("Title2 Scene"));
        yield return new WaitUntil(() => loadOperationDone);

        /******************************************************
         * T H I R D   T I T L E   S C R E E N
         *****************************************************/

        // Loading the third title screen
        StartCoroutine(LoadScene("Title3 Scene"));
        // Triggering the animator: the screen fades in
        fadeAnimator.SetTrigger("Fade In Trigger");
        yield return new WaitForSecondsRealtime(1);
        yield return new WaitUntil(() => loadOperationDone);
        // Waiting for the plauer to press space bar
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        CurtainsAnimation.Inst.moveIn = true;
        SoundsManager.Inst.PlaySound("Bell");
        yield return new WaitUntil(() => !CurtainsAnimation.Inst.moveIn);

        StartCoroutine(UnloadScene("Title3 Scene"));
        yield return new WaitUntil(() => loadOperationDone);

        StartCoroutine(LoadScene("House Game Scene", false));
        StartCoroutine(LoadScene("UI Replay Button", false));

        CurtainsAnimation.Inst.moveOut = true;
    }


    public IEnumerator LoadScene (string sceneName, bool waitLoading = true)
    {
        loadOperationDone = false;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (waitLoading)
        {
            yield return new WaitUntil(() => op.isDone);
            loadOperationDone = true;
        }
        else yield break;
    }

    public IEnumerator UnloadScene(string sceneName, bool waitUnloading = true)
    {
        loadOperationDone = false;
        AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.None);
        if (waitUnloading)
        {
            yield return new WaitUntil(() => op.isDone);
            loadOperationDone = true;
        }
        else yield break;
    }
}
