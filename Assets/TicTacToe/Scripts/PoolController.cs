using UnityEngine;
using System.Collections.Generic;

public class PoolController : MonoBehaviour
{

    public GameObject CirclePrefab;
    public GameObject CrossPrefab;

    public Cell[] Field;

    void Start()
    {

        StateController.Instance.StartGameStart.AddListener(ResetField);

        StateController.Instance.UserTurnStart.AddListener(ActivatePossibleMoves);
        StateController.Instance.UserTurnEnd.AddListener(DisableAllCells);

        StateController.Instance.AITurnStart.AddListener(ChoseAITurn);

        StateController.Instance.EndRoundStart.AddListener(delegate (GameController.GameResult result) { DisableAllCells(); });

        GameController.Instance.ChoseFirstTurn();
    }

    private void OnDestroy()
    {
        StateController.Instance.StartGameStart.RemoveListener(ResetField);

        StateController.Instance.UserTurnStart.RemoveListener(ActivatePossibleMoves);
        StateController.Instance.UserTurnEnd.RemoveListener(DisableAllCells);

        StateController.Instance.AITurnStart.RemoveListener(ChoseAITurn);

        StateController.Instance.EndRoundStart.RemoveAllListeners();
    }

    public void OnCellClick(Transform center)
    {
        foreach (Cell cell in Field)
        {
            if (cell.Center == center)
            {
                if (cell.Figure != GameController.GameFigure.None)
                {
                    return;
                }
                DoMove(GameController.Instance.PlayerFigure, cell);

                if (CheckGameOver())
                {
                    return;
                };
                StateController.Instance.AITurnState();
                return;
            }
        }
    }

    public void ChoseAITurn()
    {
        GameController.GameFigure aiFigure = GameController.Instance.PlayerFigure == GameController.GameFigure.Circle ? GameController.GameFigure.Cross : GameController.GameFigure.Circle;
        switch (GameController.Instance.GameDifficulty)
        {
            case GameController.Difficulty.Easy:
                DoMove(aiFigure, GetRandomPosition());
                break;
            case GameController.Difficulty.Noraml:
                if (Random.Range(0f, 1f) > .5f)
                {
                    DoMove(aiFigure, GetBestPosition());
                }
                else
                {
                    DoMove(aiFigure, GetRandomPosition());
                }
                break;
            case GameController.Difficulty.Hard:
                DoMove(aiFigure, GetBestPosition());
                break;
        }

        if (CheckGameOver())
        {
            return;
        };
        StateController.Instance.UserTurnState();
    }

    private void ResetField()
    {
        foreach (Cell cell in Field)
        {
            cell.Reset();
        }
        GameController.Instance.ChoseFirstTurn();
    }

    private List<Cell> GetOpenPositions(Cell[] field)
    {
        List<Cell> openPos = new List<Cell>();
        foreach (Cell cell in field)
        {
            if (cell.Figure == GameController.GameFigure.None)
            {
                openPos.Add(cell);
            }
        }
        return openPos;
    }

    private void ActivatePossibleMoves()
    {
        foreach (Cell cell in Field)
        {
            if (cell.Figure == GameController.GameFigure.None)
            {
                cell.InteractMode(true);
            }
        }
    }

    private void DisableAllCells()
    {
        foreach (Cell cell in Field)
        {
            if (cell.Figure == GameController.GameFigure.None)
            {
                cell.InteractMode(false);
            }
        }
    }

    private bool CheckGameOver()
    {
        GameController.GameResult result = CheckWin(Field);

        if (result == GameController.GameResult.Win)
        {
            GameController.Instance.UserWin();
            return true;
        }
        if (result == GameController.GameResult.Lose)
        {
            GameController.Instance.UserLose();
            return true;
        }

        if (result == GameController.GameResult.Draw)
        {
            GameController.Instance.DrawGame();
            return true;
        }

        return false;
    }

    private Cell GetRandomPosition()
    {
        var openPos = GetOpenPositions(Field);
        return openPos[Random.Range(0, openPos.Count - 1)];
    }

    private Cell GetBestPosition()
    {
        if (Field[4].Figure == GameController.GameFigure.None)
        {
            return Field[4];

        }

        List<Cell> openPos = GetOpenPositions(Field);
        int[] scores = new int[openPos.Count];

        for (int i = 0; i < openPos.Count; i++)
        {
            scores[i] = NextStep(Cell.DeepClone(Field), openPos[i], GameController.GameFigure.Circle == GameController.Instance.PlayerFigure ? GameController.GameFigure.Cross : GameController.GameFigure.Circle);
        }

        int maxScore = scores[0];
        Cell bestCell = openPos[0];
        for (int i = 1; i < scores.Length; i++)
        {
            if (scores[i] > maxScore)
            {
                maxScore = scores[i];
                bestCell = openPos[i];
            }
        }

        return bestCell;
    }

    public int NextStep(Cell[] field, Cell start, GameController.GameFigure figeru)
    {
        field[start.X * 3 + start.Y].Figure = figeru;

        GameController.GameResult result = CheckWin(field);
        switch (result)
        {
            case GameController.GameResult.Win:
                return -10;
            case GameController.GameResult.Draw:
                return 5;
            case GameController.GameResult.Lose:
                return 10;
            case GameController.GameResult.None:
                GameController.GameFigure enemyFigure = figeru == GameController.GameFigure.Circle ? GameController.GameFigure.Cross : GameController.GameFigure.Circle;
                List<Cell> openPos = GetOpenPositions(field);
                int[] scores = new int[openPos.Count];

                for (int i = 1; i < openPos.Count; i++)
                {
                    scores[i] = NextStep(Cell.DeepClone(field), openPos[i], enemyFigure);
                }

                int sumScore = 0;
                foreach (int score in scores)
                {
                    sumScore += score;
                }

                return sumScore;
            default: return 0;
        }
    }

    private GameController.GameResult CheckWin(Cell[] field)
    {
        GameController.GameFigure figure = GameController.GameFigure.None;

        if (field[0].Figure == field[1].Figure && field[1].Figure == field[2].Figure) // 1 row
        {
            figure = field[0].Figure;
        }
        else if (field[3].Figure == field[4].Figure && field[4].Figure == field[5].Figure) // 2 row
        {
            figure = field[3].Figure;
        }
        else if (field[6].Figure == field[7].Figure && field[7].Figure == field[8].Figure) // 3 row
        {
            figure = field[6].Figure;
        }
        else if (field[0].Figure == field[3].Figure && field[3].Figure == field[6].Figure) // 1 col
        {
            figure = field[0].Figure;
        }
        else if (field[1].Figure == field[4].Figure && field[4].Figure == field[7].Figure) // 2 col
        {
            figure = field[1].Figure;
        }
        else if (field[2].Figure == field[5].Figure && field[5].Figure == field[8].Figure) // 3 col
        {
            figure = field[2].Figure;
        }
        else if (field[0].Figure == field[4].Figure && field[4].Figure == field[8].Figure) // lt up -> rt dw
        {
            figure = field[0].Figure;
        }
        else if (field[2].Figure == field[4].Figure && field[4].Figure == field[6].Figure) // rt up -> lf dw
        {
            figure = field[2].Figure;
        }

        if (figure != GameController.GameFigure.None)
        {
            if (figure == GameController.Instance.PlayerFigure)
            {
                return GameController.GameResult.Win;
            }
            else
            {
                return GameController.GameResult.Lose;
            }
        }
        if (GetOpenPositions(field).Count == 0)
        {
            return GameController.GameResult.Draw;
        }

        return GameController.GameResult.None;
    }

    private void DoMove(GameController.GameFigure figure, Cell cell)
    {
        GameObject prefab = figure == GameController.GameFigure.Circle ? CirclePrefab : CrossPrefab;
        GameObject obj = Instantiate(prefab, cell.Center) as GameObject;
        obj.transform.SetParent(cell.Center);
        cell.Figure = figure;
        (cell.Center.GetComponent<SpriteRenderer>() as SpriteRenderer).enabled = false;
    }

    [System.Serializable]
    public class Cell
    {
        public int X;
        public int Y;
        public Transform Center;

        public GameController.GameFigure Figure;

        public static Cell[] DeepClone(Cell[] origin)
        {
            Cell[] copy = new Cell[origin.Length];

            for (int i = 0; i < origin.Length; i++)
            {
                copy[i] = origin[i].Clone();
            }

            return copy;
        }

        public Cell(int x, int y, Transform center, GameController.GameFigure figure = GameController.GameFigure.None)
        {
            X = x;
            Y = y;
            Center = center;
            Figure = figure;
        }

        public void Reset()
        {
            Figure = GameController.GameFigure.None;
            Transform[] children = Center.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (Center.transform == child.transform)
                {
                    continue;
                }
                DestroyImmediate(child.gameObject);
            }
        }

        public void InteractMode(bool value)
        {
            (Center.GetComponent<SpriteRenderer>() as SpriteRenderer).enabled = value;
            (Center.GetComponent<Collider2D>() as Collider2D).enabled = value;
        }

        public Cell Clone()
        {
            return new Cell(X, Y, Center, Figure);
        }
    }
}
