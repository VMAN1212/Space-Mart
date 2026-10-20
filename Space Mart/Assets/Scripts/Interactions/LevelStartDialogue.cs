using UnityEngine;
using System.Collections;

public class LevelStartDialogue : MonoBehaviour
{
    [Header("Opening Dialogue Data")]
    public Dialogue openingDialogue; 

    private IEnumerator Start()
    {
        yield return null;

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(openingDialogue);
        }
    }
}