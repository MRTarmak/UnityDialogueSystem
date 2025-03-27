using UnityEngine;

public class ButtonAppearAnimator : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.1f;
    [SerializeField] private float scaleFrom = 0.8f;
    
    private CanvasGroup _canvasGroup;
    private float _animationProgress;

    private void Awake()
    {
        _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one * scaleFrom;
    }

    private void Update()
    {
        if (_animationProgress >= 1f) return;
        
        _animationProgress += Time.deltaTime / fadeTime;
        _animationProgress = Mathf.Clamp01(_animationProgress);
        
        _canvasGroup.alpha = _animationProgress;
        transform.localScale = Vector3.one * Mathf.Lerp(scaleFrom, 1f, _animationProgress);
    }

    public void SkipAnimation()
    {
        if (_animationProgress >= 1f) return;
        
        _canvasGroup.alpha = 1f;
        transform.localScale = Vector3.one;
        _animationProgress = 1f;
    }
}