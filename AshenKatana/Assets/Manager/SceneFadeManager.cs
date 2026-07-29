using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager Instance { get; private set; }

    [Header("Configurações de Fade")]
    [SerializeField] private Image fadeImage;         // Image preta, full screen, dentro de um Canvas próprio
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        // Padrão singleton persistente
        if (Instance != null && Instance != this)
        {
           
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Fade in na primeira cena em que esse manager existe
        StartCoroutine(Fade(1f, 0f));
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Toda vez que uma cena nova carrega, faz o fade in
        StartCoroutine(Fade(1f, 0f));
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return Fade(0f, 1f);
        SceneManager.LoadScene(sceneName);
        // Não precisa dar Fade(1,0) aqui: o OnSceneLoaded já cuida disso quando a cena nova carregar
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float t = 0f;
        Color c = fadeImage.color;

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true; // bloqueia cliques durante o fade

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // unscaled: funciona mesmo com Time.timeScale = 0 (painel de missão pausado)
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = targetAlpha;
        fadeImage.color = c;

        if (targetAlpha <= 0f)
            fadeImage.raycastTarget = false;
    }
}