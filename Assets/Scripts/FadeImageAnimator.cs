using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeImageAnimator : MonoBehaviour
{
    private const float FadeTime = 0.1f;
    
    private Image _image;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = new Color(1, 1, 1, 0);
    }

    public void FadeTo(Sprite newSprite)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        
        _fadeCoroutine = StartCoroutine(FadeSequence(newSprite));
    }
    
    public void SkipFade()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _image.color = new Color(1, 1, 1, 1);
        }
    }

    private IEnumerator FadeSequence(Sprite newSprite)
    {
        if (_image.sprite != null)
        {
            while (_image.color.a > 0)
            {
                _image.color = new Color(1, 1, 1, _image.color.a - Time.deltaTime / FadeTime);
                yield return null;
            }
        }

        _image.sprite = newSprite;

        if (newSprite != null)
        {
            gameObject.SetActive(true);
            while (_image.color.a < 1)
            {
                _image.color = new Color(1, 1, 1, _image.color.a + Time.deltaTime / FadeTime);
                yield return null;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        _fadeCoroutine = null;
    }
}