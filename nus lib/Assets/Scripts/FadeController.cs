using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Attach this to your fade panel GameObject (the full-screen Image + CanvasGroup),
// in EVERY scene that needs a fade. Drag the same GameObject into both fields below.
public class FadeController : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public Image fadePanelImage;

    private Color currentFadeColor = Color.black;

    void Awake()
    {
        // start fully transparent and non-blocking so it doesn't eat clicks by accident
        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
            fadePanel.blocksRaycasts = false;
        }
    }

    // Fades the panel from transparent to fully opaque in the given color.
    // Use this to fade TO black/white before a scene transition or story beat.
    public IEnumerator FadeToColor(Color color, float duration)
    {
        currentFadeColor = color;
        fadePanelImage.color = color; // set before fading so it's already the right color, not mid-blend
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

    // Fades the panel from fully opaque back to transparent, revealing the scene.
    // Uses whatever color was last set via FadeToColor, so pair calls together.
    public IEnumerator FadeFromColor(float duration)
    {
        fadePanelImage.color = currentFadeColor;
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
}
