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

	private Vector2 target;

	private SpriteRenderer spriteRenderer;
	private int type;
	private float x;
	private float y;
	public int row;
	public int col;
	public int historyRow = -1;
	public int historyCol = -1;

//	private ArrayList avaliableNextMove = new ArrayList();
	public HashSet <Vector2> avaliableNextMove = new HashSet <Vector2> ();

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void ChangeColor (Color color) {
		spriteRenderer.color = color;
	}

	public void SetType (int type) {
		this.type = type;
	}

	public void SetIndex (int row, int col, BoardManager ctx, bool paint = true) {
		this.row = row;
		this.col = col;
		ChangePosition (ctx.GetBlock(row, col).x, ctx.GetBlock(row, col).y, paint);
		ctx.GetBlock (row, col).SetOccupied (this.type);
	}

	public void SetPosition (float x, float y) {
		this.x = x;
		this.y = y;
	}

	public void ChangePosition (float x, float y, bool paint = true) {
		if (paint) {
			Transform transform = GetComponent<Transform> ();
			transform.SetPositionAndRotation (new Vector3(x, y, 0f), Quaternion.identity);
		}
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

		//debug
		Debug.LogFormat ("Score: {0}, r {1}, c {2}", Score(), row, col);
	}

	public void OnUnSelected () {
		ChangeColor (color[type]);
	}

    public Vector2 MoveTo(int row, int col, BoardManager ctx, bool paint = true) {
		//if (avaliableNextMove.Contains (new Vector2 (row, col)) == false) {
  //          return new Vector2(this.row, this.col) ;
		//}

		ctx.GetBlock (this.row, this.col).UnsetOccupied ();
		this.historyRow = this.row;
		this.historyCol = this.col;
		SetIndex (row, col, ctx, paint);

        return new Vector2(this.historyRow, this.historyCol); 
	}

    public Vector2 StepTo(int row, int col, BoardManager ctx) {
        return MoveTo(row, col, ctx);
    }

	public void UndoMove (BoardManager ctx, bool paint = true) {
		if (historyRow < 0 || historyCol < 0) {
			return;
		}
		ctx.GetBlock (row, col).UnsetOccupied ();
		SetIndex (historyRow, historyCol, ctx, paint);
		historyRow = -1;
		historyCol = -1;
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

	public void SetTarget (Vector2 target) {
		this.target = target;
	}

	public int Score () {
		int d1 = this.row - (int)this.target.x;
		int d2 = this.col - (int)this.target.y;
		return d1*d1 + d2*d2;
	}

	public int Gain (Vector2 move) {
		int d1 = (int)move.x - (int)this.target.x;
		int d2 = (int)move.y - (int)this.target.y;
		return Score() - (d1*d1 + d2*d2);
	}

	public void UpdateAvaliableMove (BoardManager ctx) {
		ctx.GetBlock(row, col).CalcAvaliableNextMove (avaliableNextMove);
	}
}
