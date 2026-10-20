using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("References")]
    public NPCInteraction tutorialNpc;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ProgressTutorial()
    {
        if (tutorialNpc != null)
        {
            tutorialNpc.AdvanceDialogueStep();
        }
    }
}
