using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float charsPerSecond = 30f;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] [Range(0, 1)] private float volume = 0.5f;
    [SerializeField] private float soundPitchRandomization = 0.03f;
    
    private TMP_Text _textField;
    private Coroutine _typingCoroutine;
    private string _currentText;
    private AudioSource _audioSource;

    private void Awake()
    {
        _textField = GetComponent<TMP_Text>();
        
        if (typingSound != null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.volume = volume;
        }
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
            PlayTypingSound();
            yield return new WaitForSeconds(1f / charsPerSecond);
        }
        _audioSource.Stop();
        _typingCoroutine = null;
    }
    
    private void PlayTypingSound()
    {
        if (typingSound == null || _audioSource == null) return;
        
        _audioSource.pitch = 0.05f + Random.Range(-soundPitchRandomization, soundPitchRandomization);
        
        _audioSource.Stop();
        _audioSource.PlayOneShot(typingSound);
    }
}