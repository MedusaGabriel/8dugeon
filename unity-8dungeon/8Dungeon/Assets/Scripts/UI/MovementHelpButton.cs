using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("8Dungeon/UI/Movement Help Button")]
public class MovementHelpButton : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [TextArea]
    [SerializeField] private string customMessage;
    [SerializeField] private UnityEvent onHintsShown;

    public void ShowMovementHints()
    {
        if (gameController != null)
        {
            if (string.IsNullOrWhiteSpace(customMessage))
            {
                gameController.ShowMovementExamples();
            }
            else
            {
                gameController.ShowMovementExamples(customMessage);
            }
        }

        onHintsShown?.Invoke();
    }
}
