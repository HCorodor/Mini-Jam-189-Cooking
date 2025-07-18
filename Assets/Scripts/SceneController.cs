using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("Fade")]
    [SerializeField] private GameObject _screenFader;
    private CanvasGroup _fadeGroup;
    private bool _isLoading = false;

    private void Awake()
    {
        if (Instance == null) // Check whether a SceneController exists
        {
            Instance = this; // Makes the current SceneController THE SceneController
            DontDestroyOnLoad(gameObject); // Makes it persist

            GameObject fader = Instantiate(_screenFader); // Spawns in the fade object
            DontDestroyOnLoad(fader); // Makes it persist
            _fadeGroup = fader.GetComponentInChildren<CanvasGroup>();
        }
        else
        {
            Destroy(gameObject); // A SceneController was already detected, remove duplicates
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            ReloadScene(false);
        }
        if (Input.GetKey(KeyCode.E))
        {
            ReloadScene(true);
        }
    }

    public void LoadScene(string sceneName, bool withFade) // Simple function to load a new scene
    {
        if (_isLoading) return;

        if (withFade)
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void ReloadScene(bool withFade) // Simple function to reload the current scene
    {
        if (_isLoading) return;

        if (withFade)
        {
            StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator LoadSceneRoutine(string sceneName) // A function to load a scene while fading in and out using an AsyncOperation (allowing it to track the progress while scenes change)
    {
        _isLoading = true;

        yield return FadeOut(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return FadeIn(1f);

        _isLoading = false;
    }

    private IEnumerator FadeOut(float duration) // While the passed time is not equal to the duration, the screen becomes more and more dark 
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            _fadeGroup.alpha = Mathf.Clamp01(time / duration);
            yield return null;
        }
    }

    private IEnumerator FadeIn(float duration) // While the passed time is not equal to the duration, the screen becomes less and less dark
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            _fadeGroup.alpha = 1f - Mathf.Clamp01(time / duration);
            yield return null;
        }
    }

    public void QuitGame() // Function to quit the game
    {
        Application.Quit();

#if UNITY_EDITOR //If we are testing in Unity, we will exit playmode instead
            UnityEditor.EditorApplication.isPlaying = false;        
#endif
    }
}
