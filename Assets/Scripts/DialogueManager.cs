using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Transform choicesContainer;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private Image speakerImage;

    private Dialogue _currentDialogue;
    private int _currentNodeIndex;
    
    public static event Action OnDialogueStart;
    public static event Action OnDialogueEnd;
    
    private TypewriterEffect _typewriter;
    private FadeImageAnimator _imageAnimator;

    private void Awake()
    {
        _typewriter = dialogueText.GetComponent<TypewriterEffect>();
        _imageAnimator = speakerImage.GetComponent<FadeImageAnimator>();
        
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        OnDialogueStart?.Invoke();
        _currentDialogue = dialogue;
        _currentNodeIndex = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(DisplayCurrentNode());
    }

    private IEnumerator DisplayCurrentNode()
    {
        yield return StartCoroutine(ClearButtonsAtEndOfFrame());
        
        Dialogue.DialogueNode node = _currentDialogue.nodes[_currentNodeIndex];
        speakerText.text = node.speakerName;
        dialogueText.text = node.text;
        
        _typewriter.StartTyping(node.text);
        _imageAnimator.FadeTo(node.speakerImage);
        
        if (node.speakerImage != null)
        {
            speakerImage.sprite = node.speakerImage;
            speakerImage.gameObject.SetActive(true);
        }
        else
        {
            speakerImage.gameObject.SetActive(false);
        }

        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var choice in node.choices)
            {
                CreateChoiceButton(choice);
            }
        }
        else
        {
            CreateContinueButton();
        }
    }
    
    private IEnumerator ClearButtonsAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
    
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void CreateChoiceButton(Dialogue.DialogueChoice choice)
    {
        GameObject button = Instantiate(choiceButtonPrefab, choicesContainer);
        button.GetComponentInChildren<TMP_Text>().text = choice.choiceText;
        button.GetComponent<Button>().onClick.AddListener(() => SelectChoice(choice));
        button.AddComponent<ButtonAppearAnimator>();
    }

    private void CreateContinueButton()
    {
        GameObject button = Instantiate(choiceButtonPrefab, choicesContainer);
        button.GetComponentInChildren<TMP_Text>().text = ">>>";
        button.GetComponent<Button>().onClick.AddListener(NextNode);
        button.AddComponent<ButtonAppearAnimator>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _typewriter.SkipTyping();
        }
    }

    private void NextNode()
    {
        _currentNodeIndex++;
        if (_currentNodeIndex < _currentDialogue.nodes.Length)
            StartCoroutine(DisplayCurrentNode());
        else
            EndDialogue();
    }

    private void SelectChoice(Dialogue.DialogueChoice choice)
    {
        if (choice.nextDialogue != null)
            StartDialogue(choice.nextDialogue);
        else
            EndDialogue();
    }

    private void EndDialogue()
    {
        OnDialogueEnd?.Invoke();
        dialoguePanel.SetActive(false);
    }
}