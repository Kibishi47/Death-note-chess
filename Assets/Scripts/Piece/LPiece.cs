using UnityEngine;

public class LPiece : Piece
{
    public override bool CanMoveTo(Vector2Int newPosition) {
        return true;
    }
}
