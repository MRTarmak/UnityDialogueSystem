using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float charsPerSecond = 30f;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private AudioSource audioSource;
    
    private TMP_Text _textField;
    private Coroutine _typingCoroutine;
    private string _currentText;

    private void Awake()
    {
        _textField = GetComponent<TMP_Text>();
        if (audioSource == null && typingSound != null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartTyping(string text)
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);
        
        _currentText = text;
        _typingCoroutine = StartCoroutine(TypeText());
    }

    public void SkipTyping()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _textField.text = _currentText;
        }
    }

    private IEnumerator TypeText()
    {
        _textField.text = "";
        foreach (char c in _currentText.ToCharArray())
        {
            _textField.text += c;
            if (typingSound != null && audioSource != null)
                audioSource.PlayOneShot(typingSound);
            yield return new WaitForSeconds(1f / charsPerSecond);
        }
        _typingCoroutine = null;
    }
}