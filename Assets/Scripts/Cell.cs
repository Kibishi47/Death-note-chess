using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int Position { get; private set; }
    public Piece OccupyingPiece { get; private set; }

    public void Initialize(Vector2Int position)
    {
        Position = position;
    }

    public void SetPiece(Piece piece)
    {
        OccupyingPiece = piece;
        if (piece != null)
        {
            piece.transform.position = transform.position;
        }
    }
}