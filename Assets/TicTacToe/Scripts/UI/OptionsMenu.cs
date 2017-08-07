using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public void Awake()
    {
        DeactivateWindow();

        StateController.Instance.OptionsMenuStart.AddListener(ActivateWindow);
        StateController.Instance.OptionsMenuEnd.AddListener(DeactivateWindow);
    }

    public void OnDestroy()
    {
        StateController.Instance.OptionsMenuStart.RemoveListener(ActivateWindow);
        StateController.Instance.OptionsMenuEnd.RemoveListener(DeactivateWindow);
    }

    public void OnEasyClick()
    {
        GameController.Instance.ChangeDifficulty(GameController.Difficulty.Easy);
        StateController.Instance.MainMenuState();
    }

    public void OnNormalClick()
    {
        GameController.Instance.ChangeDifficulty(GameController.Difficulty.Noraml);
        StateController.Instance.MainMenuState();
    }

    public void OnHardClick()
    {
        GameController.Instance.ChangeDifficulty(GameController.Difficulty.Hard);
        StateController.Instance.MainMenuState();
    }

    private void ActivateWindow()
    {
        gameObject.SetActive(true);
    }

    private void DeactivateWindow()
    {
        gameObject.SetActive(false);
    }
}
