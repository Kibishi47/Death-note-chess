using UnityEngine;

public class KiraPiece : Piece
{
    public override bool CanMoveTo(Vector2Int newPosition) {
        return true;
    }
}
