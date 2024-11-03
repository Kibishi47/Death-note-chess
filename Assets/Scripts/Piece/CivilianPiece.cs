using UnityEngine;

public class CivilianPiece : Piece
{
    public override bool CanMoveTo(Vector2Int newPosition) {
        return true;
    }
}
