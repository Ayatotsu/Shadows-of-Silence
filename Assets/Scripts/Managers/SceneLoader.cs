using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("UI References")]
    public CanvasGroup fadeCanvas; // Panel with CanvasGroup for fading
    public Slider progressBar;
    public float fadeDuration = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Fade in (to black)
        yield return StartCoroutine(Fade(1));

        // Show loading UI
        fadeCanvas.blocksRaycasts = true;
        if (progressBar != null) progressBar.gameObject.SetActive(true);

        // Start async load
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressBar != null) progressBar.value = progress;

            // Scene is ready
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.3f); // small delay
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Fade out (back to game)
        yield return StartCoroutine(Fade(0));

        fadeCanvas.blocksRaycasts = false;
        if (progressBar != null) progressBar.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvas.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = targetAlpha;
    }
}
