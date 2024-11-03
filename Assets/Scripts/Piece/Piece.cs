using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public enum PieceType
    {
        Kira,
        Misa,
        L,
        Investigator,
        Civilian,
        Criminal
    }

    [SerializeField]
    private PieceType type;
    public PieceType Type { get { return type; } protected set { type = value; } }

    [SerializeField]
    private string pieceName;
    public string PieceName { get { return pieceName; } protected set { pieceName = value; } }

    public bool IsRevealed { get; protected set; }

    [SerializeField]
    private Vector2Int position;
    public Vector2Int Position { get { return position; } protected set { position = value; } }

    public virtual void Initialize(Vector2Int startPosition)
    {
        Position = startPosition;
        IsRevealed = false;
    }

    public abstract bool CanMoveTo(Vector2Int newPosition);

    public virtual void MoveTo(Vector2Int newPosition)
    {
        Position = newPosition;
    }
}