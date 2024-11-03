using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameBoard Board;
    public GameObject KiraPrefab;
    public GameObject MisaPrefab;
    public GameObject LPrefab;
    public GameObject InvestigatorPrefab;
    public GameObject CivilianPrefab;
    public GameObject CriminalPrefab;

    private List<Piece> kiraPieces;
    private List<Piece> lPieces;
    private List<Piece> aiPieces;

    private bool isKiraTurn = true;

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        kiraPieces = new List<Piece>();
        lPieces = new List<Piece>();
        aiPieces = new List<Piece>();

        // Place Kira's pieces
        PlacePiece(KiraPrefab, new Vector2Int(0, 0));
        PlacePiece(MisaPrefab, new Vector2Int(1, 0));

        // Place L's pieces
        PlacePiece(LPrefab, new Vector2Int(7, 7));
        for (int i = 0; i < 5; i++)
        {
            PlacePiece(InvestigatorPrefab, new Vector2Int(6, i));
        }

        // Place AI pieces (civilians and criminals)
        // You'll need to implement logic to place these randomly
    }

    void PlacePiece(GameObject piecePrefab, Vector2Int position)
    {
        GameObject pieceObject = Instantiate(piecePrefab);
        Piece piece = pieceObject.GetComponent<Piece>();
        Board.PlacePiece(piece, position);

        if (piece.Type == Piece.PieceType.Kira || piece.Type == Piece.PieceType.Misa)
        {
            kiraPieces.Add(piece);
        }
        else if (piece.Type == Piece.PieceType.L || piece.Type == Piece.PieceType.Investigator)
        {
            lPieces.Add(piece);
        }
        else
        {
            aiPieces.Add(piece);
        }
    }

    public void EndTurn()
    {
        isKiraTurn = !isKiraTurn;
        // Implement turn logic here
    }

    // Implement other game logic methods here
}