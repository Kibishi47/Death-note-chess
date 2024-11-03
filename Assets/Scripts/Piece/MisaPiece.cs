using UnityEngine;

public class MisaPiece : Piece
{
    public override bool CanMoveTo(Vector2Int newPosition) {
        return true;
    }
}
