using UnityEngine;
using UnityEngine.UI;

public class StatisticsMenu : MonoBehaviour
{

    public Text WinCounter;
    public Text LoseCounter;
    public Text DrawCounter;

    public void Awake()
    {
        StateController.Instance.EndRoundStart.AddListener(OnScoreChange);
    }

    private void OnDestroy()
    {
        StateController.Instance.EndRoundStart.RemoveListener(OnScoreChange);
    }

    public void OnScoreChange(GameController.GameResult result)
    {
        WinCounter.text = GameController.Instance.Win.ToString();
        LoseCounter.text = GameController.Instance.Lose.ToString();
        DrawCounter.text = GameController.Instance.Draw.ToString();
    }

    public void OnBackClick()
    {
        StateController.Instance.MainMenuState();
    }
}
