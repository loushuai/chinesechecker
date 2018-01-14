using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : ClickableMonoBehaviour {
	private Color [] color = new Color[] {new Color(255f/255, 0, 0, 1f),
								               new Color(0, 0, 255f/255, 1f),};
	private Color [] selectedColor = new Color[] {
		new Color(142f/255, 36f/255, 170f/255, 1f),
		new Color(30f/255, 136f/255, 229f/255, 1f),
	};

	private SpriteRenderer spriteRenderer;
	private int type;
	private float x;
	private float y;
	public int row;
	public int col;
	public int historyRow = -1;
	public int historyCol = -1;

//	private ArrayList avaliableNextMove = new ArrayList();
	private HashSet <Vector2> avaliableNextMove = new HashSet <Vector2> ();

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void ChangeColor (Color color) {
		spriteRenderer.color = color;
	}

	public void SetType (int type) {
		this.type = type;
	}

	public void SetIndex (int row, int col, BoardManager ctx) {
		this.row = row;
		this.col = col;
		ChangePosition (ctx.GetBlock(row, col).x, ctx.GetBlock(row, col).y);
		ctx.GetBlock (row, col).SetOccupied (this.type);
	}

	public void SetPosition (float x, float y) {
		this.x = x;
		this.y = y;
	}

	public void ChangePosition (float x, float y) {
		Transform transform = GetComponent<Transform> ();
		transform.SetPositionAndRotation (new Vector3(x, y, 0f), Quaternion.identity);
		this.x = x;
		this.y = y;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnSelected () {
		ChangeColor (selectedColor[type]);
	}

	public void OnUnSelected () {
		ChangeColor (color[type]);
	}

	public void MoveTo(int row, int col, BoardManager ctx) {
		if (avaliableNextMove.Contains (new Vector2 (row, col)) == false) {
			return;
		}

		ctx.GetBlock (this.row, this.col).UnsetOccupied ();
		this.historyRow = row;
		this.historyCol = col;
		SetIndex (row, col, ctx);
	}

	public void SetAvaliableJump (BoardManager ctx) {
		ctx.SetBlockListColor (avaliableNextMove, new Color (239f/255, 154f/255, 154f/255, 1f));
	}

	public void UnsetAvaliableJump (BoardManager ctx) {
		ctx.SetBlockListColor (avaliableNextMove, new Color (194f/255, 194f/255, 194f/255, 1f));
	}

	public override int OnClick(BoardManager ctx) {
		Debug.LogFormat ("Piece on click");
		if (ctx.selectedPiece != null) {
			Debug.LogFormat ("Already a piece selected");
			ctx.selectedPiece.OnUnSelected ();
			ctx.selectedPiece.UnsetAvaliableJump (ctx);

		} else {
			Debug.LogFormat ("A piece is selected");
		}

		ctx.selectedPiece = this;
		OnSelected();

		ctx.GetBlock(row, col).CalcAvaliableNextMove (avaliableNextMove);
		SetAvaliableJump (ctx);

		return 0;
	}


}
