using UnityEngine;

public class GameController : MonoSingleton<GameController>
{

    public enum Difficulty { Easy, Noraml, Hard }
    public enum GameFigure { Circle, Cross, None }
    public enum GameResult { Win, Draw, Lose, None }
    public enum TurnOwner { User, AI }

    [SerializeField]
    public int Win { get; private set; }
    [SerializeField]
    public int Lose { get; private set; }
    [SerializeField]
    public int Draw { get; private set; }
    public Difficulty GameDifficulty { get; private set; }

    public GameFigure PlayerFigure;

    private GameResult _lastGameResult;

    public override void Awake()
    {
        GameDifficulty = Difficulty.Easy;
        _lastGameResult = GameResult.None;
    }

    public void ChoseFirstTurn()
    {
        switch (_lastGameResult)
        {
            case GameResult.Lose:
                PlayerFigure = GameFigure.Circle;
                StateController.Instance.AITurnState();
                break;
            case GameResult.Draw:
                if (PlayerFigure == GameFigure.Circle)
                {
                    PlayerFigure = GameFigure.Cross;
                    StateController.Instance.UserTurnState();
                }
                else
                {
                    PlayerFigure = GameFigure.Circle;
                    StateController.Instance.AITurnState();
                }
                break;
            case GameResult.Win:
                PlayerFigure = GameFigure.Cross;
                StateController.Instance.UserTurnState();
                break;
            case GameResult.None:
                PlayerFigure = GameFigure.Cross;
                StateController.Instance.UserTurnState();
                break;
        }
    }

    public void ChangeDifficulty(Difficulty value)
    {
        GameDifficulty = value;
    }

    public void UserWin()
    {
        Win++;
        _lastGameResult = GameResult.Win;
        StateController.Instance.EndRoundState(GameResult.Win);
    }

    public void UserLose()
    {
        Lose++;
        _lastGameResult = GameResult.Lose;
        StateController.Instance.EndRoundState(GameResult.Lose);
    }

    public void DrawGame()
    {
        Draw++;
        _lastGameResult = GameResult.Draw;
        StateController.Instance.EndRoundState(GameResult.Draw);
    }

    public void Reset()
    {
        Win = 0;
        Lose = 0;
        Draw = 0;
    }
}
