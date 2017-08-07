using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void Awake()
    {
        StateController.Instance.OnMainMenuStart.AddListener(ActivateWindow);
        StateController.Instance.OnMainMenuEnd.AddListener(DeactivateWindow);
    }

    public void OnDestroy()
    {
        StateController.Instance.OnMainMenuStart.RemoveListener(ActivateWindow);
        StateController.Instance.OnMainMenuEnd.RemoveListener(DeactivateWindow);
    }

    public void OnStartGameClick()
    {
        StateController.Instance.StartGameState();
    }

    public void OnOptionsClick()
    {
        StateController.Instance.OptionMenuState();
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
