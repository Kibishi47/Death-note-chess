using UnityEngine;

public class InvestigatorPiece : Piece
{
    public override bool CanMoveTo(Vector2Int newPosition) {
        return true;
    }
}
