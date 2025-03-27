using System.Collections;
using UnityEngine;

public class ButtonAppearAnimator : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.1f;
    [SerializeField] private float scaleFrom = 0.8f;
    [SerializeField] private float delay = 0.1f;
    
    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        transform.localScale = Vector3.one * scaleFrom;
        
        yield return new WaitForSeconds(delay);
        
        float timer = 0;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeTime;
            canvasGroup.alpha = progress;
            transform.localScale = Vector3.one * Mathf.Lerp(scaleFrom, 1f, progress);
            yield return null;
        }
        
        Destroy(canvasGroup);
    }
}