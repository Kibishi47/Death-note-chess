using UnityEngine;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour
{
    public int Width = 8;
    public int Height = 8;
    public GameObject CellPrefab;
    public string Theme = "Classic"; // Can be changed in the inspector

    [Header("Frame Sprites")]
    public Sprite TopFrame;
    public Sprite RightFrame;
    public Sprite BottomFrame;
    public Sprite LeftFrame;
    public Sprite CornerTopLeft;
    public Sprite CornerTopRight;
    public Sprite CornerBottomRight;
    public Sprite CornerBottomLeft;

    [Header("Cell Sprite")]
    public Sprite CellSprite;

    private Cell[,] cells;
    private List<Piece> pieces;

    // Constants for frame dimensions
    private const float HORIZONTAL_FRAME_WIDTH = 1.09f;
    private const float HORIZONTAL_FRAME_HEIGHT = 0.74f;
    private const float VERTICAL_FRAME_WIDTH = 0.74f;
    private const float VERTICAL_FRAME_HEIGHT = 1.09f;
    private const float CORNER_SIZE = 0.74f;
    private const float CELL_SIZE = 1.09f;
    private float horizontalFrameWidth;
    private float horizontalFrameHeight;
    private float verticalFrameWidth;
    private float verticalFrameHeight;
    private float cornerSize;
    private float cellSize;

    void Start()
    {
        LoadThemeSprites();
        SetInitialDimensions();
        if (IsBoardLargerThanCamera()) {
            CalculateCellSizeToFitCamera();
        }
        InitializeBoard();
    }

    void SetInitialDimensions()
    {
        horizontalFrameWidth = HORIZONTAL_FRAME_WIDTH;
        horizontalFrameHeight = HORIZONTAL_FRAME_HEIGHT;
        verticalFrameWidth = VERTICAL_FRAME_WIDTH;
        verticalFrameHeight = VERTICAL_FRAME_HEIGHT;
        cornerSize = CORNER_SIZE;
        cellSize = CELL_SIZE;
    }


    bool IsBoardLargerThanCamera()
    {
        Camera cam = Camera.main;
        if (cam != null && cam.orthographic)
        {
            float cameraHeight = cam.orthographicSize * 2;
            float cameraWidth = cameraHeight * cam.aspect;

            float boardWidth = Width * HORIZONTAL_FRAME_WIDTH + 2 * CORNER_SIZE;
            float boardHeight = Height * VERTICAL_FRAME_HEIGHT + 2 * CORNER_SIZE;

            // Retourner true si la grille est plus grande que la vue de la caméra
            return boardWidth > cameraWidth || boardHeight > cameraHeight;
        }
        return false;
    }

    void CalculateCellSizeToFitCamera()
    {
        Camera cam = Camera.main;
        if (cam != null && cam.orthographic)
        {
            float cameraHeight = cam.orthographicSize * 2;
            float cameraWidth = cameraHeight * cam.aspect;

            float boardWidth = Width * cellSize + 2 * cornerSize;
            float boardHeight = Height * cellSize + 2 * cornerSize;

            float widthFactor = cameraWidth / boardWidth;
            float heightFactor = cameraHeight / boardHeight;
            float resizeFactor = Mathf.Min(widthFactor, heightFactor);

            cellSize *= resizeFactor;
            horizontalFrameWidth *= resizeFactor;
            horizontalFrameHeight *= resizeFactor;
            verticalFrameWidth *= resizeFactor;
            verticalFrameHeight *= resizeFactor;
            cornerSize *= resizeFactor;
        }
    }

    void LoadThemeSprites()
    {
        TopFrame = Resources.Load<Sprite>($"Images/Boards/Frames/{Theme}/Top");
        RightFrame = Resources.Load<Sprite>($"Images/Boards/Frames/{Theme}/Right");
        BottomFrame = Resources.Load<Sprite>($"Images/Boards/Frames/{Theme}/Bottom");
        LeftFrame = Resources.Load<Sprite>($"Images/Boards/Frames/{Theme}/Left");
        CornerTopLeft = Resources.Load<Sprite>($"Images/Boards/Frames/{Theme}/CornerTopLeft");
        CornerTopRight = Resources.Load<Sprite>($"Images/Boards/Frames/{Theme}/CornerTopRight");
        CornerBottomRight = Resources.Load<Sprite>($"Images/Boards/Frames/{Theme}/CornerBottomRight");
        CornerBottomLeft = Resources.Load<Sprite>($"Images/Boards/Frames/{Theme}/CornerBottomLeft");
        CellSprite = Resources.Load<Sprite>($"Images/Boards/Cells/{Theme}/Cell");
    }

    void InitializeBoard()
    {
        cells = new Cell[Width, Height];
        pieces = new List<Piece>();

        CreateFrame();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                CreateCell(x, y);
            }
        }
    }

    void CreateFrame()
    {
        float boardWidth = Width * horizontalFrameWidth + 2 * cornerSize;
        float boardHeight = Height * verticalFrameHeight + 2 * cornerSize;
        // Top Left of the final frame
        float initialX = -boardWidth / 2;
        float initialY = boardHeight / 2;

        float halfCornerSize = cornerSize / 2;

        // Corners
        CreateFramePart(CornerTopLeft, new Vector3(initialX + halfCornerSize, initialY - halfCornerSize));
        CreateFramePart(CornerTopRight, new Vector3(initialX - halfCornerSize + boardWidth, initialY - halfCornerSize));
        CreateFramePart(CornerBottomLeft, new Vector3(initialX + halfCornerSize, initialY + halfCornerSize - boardHeight));
        CreateFramePart(CornerBottomRight, new Vector3(initialX - halfCornerSize + boardWidth, initialY + halfCornerSize - boardHeight));

        // Top & Bottom frames
        for (int i = 0 ; i < Width ; i++) {
            float boardX = initialX + i * horizontalFrameWidth + cornerSize;
            float halfHorizontalFrameWidth = horizontalFrameWidth / 2;
            float halfHorizontalFrameHeight = horizontalFrameHeight / 2;

            CreateFramePart(TopFrame, new Vector3(boardX + halfHorizontalFrameWidth, initialY - halfHorizontalFrameHeight), i.ToString());
            CreateFramePart(BottomFrame, new Vector3(boardX + halfHorizontalFrameWidth, initialY + halfHorizontalFrameHeight - boardHeight), i.ToString());
        }

        // Left & Right frames
        for (int i = 0 ; i < Height ; i++) {
            float boardY = initialY - (i * verticalFrameHeight + cornerSize);
            float halfVerticalFrameWidth = verticalFrameWidth / 2;
            float halfVerticalFrameHeight = verticalFrameHeight / 2;

            CreateFramePart(LeftFrame, new Vector3(initialX + halfVerticalFrameWidth, boardY - halfVerticalFrameHeight), i.ToString());
            CreateFramePart(RightFrame, new Vector3(initialX - halfVerticalFrameWidth + boardWidth, boardY - halfVerticalFrameHeight), i.ToString());
        }
    }

    void CreateFramePart(Sprite sprite, Vector3 position, string name = "")
    {
        if (name != "") {
            name = "_" + name;
        }
        GameObject framePart = new GameObject($"Frame_{sprite.name}{name}");
        framePart.transform.SetParent(transform);
        framePart.transform.localPosition = position;

        SpriteRenderer spriteRenderer = framePart.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = 1;
    }

    void CreateCell(int x, int y)
    {
        float boardWidth = Width * cellSize;
        float boardHeight = Height * cellSize;
        float initialX = -boardWidth / 2;
        float initialY = boardHeight / 2;
        float halfCellSize = cellSize / 2;

        Vector3 cellPosition = new Vector3(initialX + (x * cellSize + halfCellSize), initialY - (y * cellSize + halfCellSize), 0);
        GameObject cellObject = Instantiate(CellPrefab, cellPosition, Quaternion.identity, transform);

        Cell cell = cellObject.GetComponent<Cell>();
        cell.Initialize(new Vector2Int(x, y));
        cells[x, y] = cell;

        SpriteRenderer spriteRenderer = cellObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = cellObject.AddComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = CellSprite;
        spriteRenderer.sortingOrder = 1;
    }

    public void PlacePiece(Piece piece, Vector2Int position)
    {
        if (IsValidPosition(position))
        {
            cells[position.x, position.y].SetPiece(piece);
            pieces.Add(piece);
            piece.Initialize(position);
        }
    }

    public bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < Width && position.y >= 0 && position.y < Height;
    }

    public Cell GetCell(Vector2Int position)
    {
        if (IsValidPosition(position))
        {
            return cells[position.x, position.y];
        }
        return null;
    }

    public List<Vector2Int> GetValidMoves(Piece piece)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2Int newPosition = new Vector2Int(x, y);
                if (piece.CanMoveTo(newPosition) && cells[x, y].OccupyingPiece == null)
                {
                    validMoves.Add(newPosition);
                }
            }
        }

        return validMoves;
    }

    public void MovePiece(Piece piece, Vector2Int newPosition)
    {
        if (IsValidPosition(newPosition) && piece.CanMoveTo(newPosition))
        {
            cells[piece.Position.x, piece.Position.y].SetPiece(null);
            cells[newPosition.x, newPosition.y].SetPiece(piece);
            piece.MoveTo(newPosition);
        }
    }

    public Piece GetPieceAt(Vector2Int position)
    {
        if (IsValidPosition(position))
        {
            return cells[position.x, position.y].OccupyingPiece;
        }
        return null;
    }

    public List<Piece> GetAllPieces()
    {
        return new List<Piece>(pieces);
    }

    public void RemovePiece(Piece piece)
    {
        if (pieces.Contains(piece))
        {
            cells[piece.Position.x, piece.Position.y].SetPiece(null);
            pieces.Remove(piece);
            Destroy(piece.gameObject);
        }
    }
}