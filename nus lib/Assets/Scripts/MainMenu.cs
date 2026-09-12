using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource ButtonClick;
    [SerializeField] GameObject FadeOut;

 
    void Start()
    {
        
    }

    public void StartGame()
    {
        ButtonClick.Play();
        StartCoroutine(TransferToFirstScene());
    }

    void Update()
    {
        
    }

    public CanvasGroup fadePanel;
    public Image fadePanelImage;

    private Color currentFadeColor = Color.black;
    public IEnumerator FadeToColor(Color color, float duration)
    {
        fadePanelImage.color = color; // set BEFORE fading so it's already black/white, not mid-blend
        float elapsed = 0f;
        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = true;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }

        fadePanel.alpha = 1f;
    }

    public IEnumerator FadeFromColor(Color color, float duration)
    {
        fadePanelImage.color = color;
        float elapsed = 0f;
        fadePanel.alpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }

        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = false;
    }

    IEnumerator TransferToFirstScene()
    {
        yield return StartCoroutine(FadeToColor(Color.black, 1f));
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);

    }
}
