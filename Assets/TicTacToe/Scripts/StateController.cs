using UnityEngine;
using UnityEngine.SceneManagement;

public class StateController : MonoSingleton<StateController>
{

    public enum State { MainMenu, OptionsMenu, StartGame, UserTurn, AITurn, EndRound }

    public Event OnMainMenuStart = new Event();
    public Event OnMainMenuEnd = new Event();
    public Event OptionsMenuStart = new Event();
    public Event OptionsMenuEnd = new Event();
    public Event StartGameStart = new Event();
    public Event StartGameEnd = new Event();
    public Event UserTurnStart = new Event();
    public Event UserTurnEnd = new Event();
    public Event AITurnStart = new Event();
    public Event AITurnEnd = new Event();
    public Event<GameController.GameResult> EndRoundStart = new Event<GameController.GameResult>();
    public Event EndRoundEnd = new Event();

    [SerializeField]
    private State _currentState = State.MainMenu;

    public void MainMenuState()
    {
        SetState(State.MainMenu);

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        OnMainMenuStart.Invoke();
    }

    public void OptionMenuState()
    {
        SetState(State.OptionsMenu);

        OptionsMenuStart.Invoke();
    }

    public void StartGameState()
    {
        SetState(State.StartGame);

        if (SceneManager.GetActiveScene().name != "Game")
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        StartGameStart.Invoke();
    }

    public void UserTurnState()
    {
        SetState(State.UserTurn);
        UserTurnStart.Invoke();
    }

    public void AITurnState()
    {
        SetState(State.AITurn);
        AITurnStart.Invoke();
    }

    public void EndRoundState(GameController.GameResult userResult)
    {
        SetState(State.EndRound);
        EndRoundStart.Invoke(userResult);
    }

    protected void SetState(State newState)
    {
        switch (_currentState)
        {
            case State.MainMenu:
                OnMainMenuEnd.Invoke();
                break;
            case State.OptionsMenu:
                OptionsMenuEnd.Invoke();
                break;
            case State.StartGame:
                if (newState == State.MainMenu)
                {
                    GameController.Instance.Reset();
                }

                StartGameEnd.Invoke();
                break;
            case State.UserTurn:
                if (newState == State.MainMenu)
                {
                    GameController.Instance.Reset();
                    UserTurnStart.RemoveAllListeners();
                    UserTurnEnd.RemoveAllListeners();
                }

                UserTurnEnd.Invoke();
                break;
            case State.AITurn:
                AITurnEnd.Invoke();
                break;
            case State.EndRound:
                EndRoundEnd.Invoke();
                break;
        }

        _currentState = newState;
    }

}
