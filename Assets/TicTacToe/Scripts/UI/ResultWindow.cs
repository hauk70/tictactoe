using UnityEngine;
using UnityEngine.UI;

public class ResultWindow : MonoBehaviour
{

    public Text Status;
    public Text Owner;

    void Start()
    {
        StateController.Instance.EndRoundStart.AddListener(SetTexts);

        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        StateController.Instance.StartGameState();

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StateController.Instance.EndRoundStart.RemoveListener(SetTexts);
    }

    private void SetTexts(GameController.GameResult userResult)
    {
        switch (userResult)
        {
            case GameController.GameResult.Draw:
                Status.text = "DRAW";
                Status.color = Color.red;
                break;
            case GameController.GameResult.Win:
                Status.text = "WIN";
                Status.color = Color.green;
                Owner.text = "User";
                break;
            case GameController.GameResult.Lose:
                Status.text = "WIN";
                Status.color = Color.red;
                Owner.text = "AI";
                break;
        }

        gameObject.SetActive(true);
    }
}
