using UnityEngine;

public class CellController : MonoBehaviour
{
    public PoolController pool;

    public void OnMouseDown()
    {
        pool.OnCellClick(transform);
    }
}
