using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Board manager.
/// Directions: 0: →, 1: ↘, 2: ↙, 3: ←, 4: ↖, 5: ↗
/// </summary>


public class BoardManager : MonoBehaviour {
	private const int WIDTH = 4;
	private const int HEIGHT = 4;
	private const float SQRT_3 = 1.732f;

	private Color [] pieceColor = new Color[] {new Color(255, 0, 0, 1f),
											   new Color(0, 0, 255, 1f),};
	private Vector2 [][] initPosition = new Vector2[][] {
		new Vector2[] {
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),},
		new Vector2[] {
			new Vector2((float)(HEIGHT - 1), (float)(WIDTH - 1)),
			new Vector2((float)(HEIGHT - 2), (float)(WIDTH - 1)),
			new Vector2((float)(HEIGHT - 1), (float)(WIDTH - 2)),}};
	private Vector2 [] playerTarget = new Vector2[] {new Vector2 (HEIGHT-1, WIDTH-1), new Vector2 (0, 0)};

	public GameObject block;
	public GameObject piece;
	public Block[,] board = new Block[HEIGHT, WIDTH];

	public Piece [][] pieces = new Piece[2][] { new Piece[] {null, null, null}, 
												new Piece[] {null, null, null}};
	public Piece selectedPiece;

	void Awake () {

	}

	public Block GetBlock(int row, int col) {
		return board [row, col];
	}

	void CreatePiece(int row, int col, int type, int idx) {
		Block blk = GetBlock (row, col);
		float x = blk.x;
		float y = blk.y;

		GameObject obj = Instantiate (piece, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
		Piece pie = obj.GetComponent<Piece> ();
		pie.ChangeColor (pieceColor[type]);
		pie.SetType (type);
		pie.SetIndex (row, col, this);
		pie.SetTarget (playerTarget[type]);

		pieces [type] [idx] = pie;
	}

	void InitBoard() {
		for (int i = 0; i < HEIGHT; ++i) {
			for (int j = 0; j < WIDTH; ++j) {
				float firstX = -i*0.55f/2;
				float firstY = -i*0.55f*SQRT_3/2;
				GameObject obj = Instantiate (block, new Vector3 (firstX + j*0.55f/2, firstY - j*0.55f*SQRT_3/2, 0f), 
					Quaternion.identity) as GameObject;
				board [i, j] = obj.GetComponent<Block> ();
				board [i, j].row = i;
				board [i, j].col = j;
			}
		}

		for (int i = 0; i < HEIGHT; ++i) {
			for (int j = 0; j < WIDTH; ++j) {
				Block blk = GetBlock (i, j);
				if (j + 1 < WIDTH) {
					blk.SetNeighbor (1, board[i, j+1]);
				}
				if (j - 1 >= 0) {
					blk.SetNeighbor (4, board[i, j-1]);
				}
				if (i + 1 < HEIGHT) {
					blk.SetNeighbor (2, board[i+1, j]);
				}
				if (i - 1 >= 0) {
					blk.SetNeighbor (5, board[i-1, j]);
				}
				if (i + 1 < HEIGHT && j - 1 >= 0) {
					blk.SetNeighbor (3, board[i+1, j-1]);
				}
				if (i - 1 >= 0 && j + 1 < WIDTH) {
					blk.SetNeighbor (0, board[i-1, j+1]);
				}
			}
		}
	}

	void InitPieces() {
		for (int i = 0; i < 2; ++i) {
			int idx = 0;
			foreach (Vector2 v in initPosition [i]) {
				CreatePiece ((int)v.x, (int)v.y, i, idx);
				++idx;
			}
		}
	}

	// Use this for initialization
	void Start () {
		InitBoard ();
		InitPieces ();
	}

	public void SetBlockListColor (HashSet <Vector2> availableList, Color color) {
		foreach (Vector2 v in availableList) {
			Block blk = GetBlock ((int)v.x, (int)v.y);
			blk.ChangeColor (color);
		}
	}
}
