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

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        _currentDialogue = dialogue;
        _currentNodeIndex = 0;
        dialoguePanel.SetActive(true);
        DisplayCurrentNode();
    }

    private void DisplayCurrentNode()
    {
        Dialogue.DialogueNode node = _currentDialogue.nodes[_currentNodeIndex];
        speakerText.text = node.speakerName;
        dialogueText.text = node.text;
        
        if (node.speakerImage != null)
        {
            speakerImage.sprite = node.speakerImage;
            speakerImage.gameObject.SetActive(true);
        }
        else
        {
            speakerImage.gameObject.SetActive(false);
        }

        foreach (Transform child in choicesContainer)
            Destroy(child.gameObject);

        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var choice in node.choices)
            {
                GameObject button = Instantiate(choiceButtonPrefab, choicesContainer);
                button.GetComponentInChildren<TMP_Text>().text = choice.choiceText;
                button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectChoice(choice));
            }
        }
        else
        {
            GameObject button = Instantiate(choiceButtonPrefab, choicesContainer);
            button.GetComponentInChildren<TMP_Text>().text = "Далее";
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(NextNode);
        }
    }

    private void NextNode()
    {
        _currentNodeIndex++;
        if (_currentNodeIndex < _currentDialogue.nodes.Length)
            DisplayCurrentNode();
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
        dialoguePanel.SetActive(false);
    }
}