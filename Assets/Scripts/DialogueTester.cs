using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    public Dialogue testDialogue;
    private void Start()
    {
        DialogueManager.Instance.StartDialogue(testDialogue);
    }
}
