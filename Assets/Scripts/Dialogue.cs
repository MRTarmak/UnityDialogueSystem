using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    [System.Serializable]
    public class DialogueNode
    {
        public string speakerName;
        [TextArea(3, 5)] public string text;
        public Sprite speakerImage; 
        public DialogueChoice[] choices;
    }
    
    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        public Dialogue nextDialogue;
    }
    
    public DialogueNode[] nodes;
}