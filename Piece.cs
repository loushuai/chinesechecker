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
	private int row;
	private int col;

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

	public void MoveTo(float x, float y) {
		ChangePosition (x, y);
	}

	public override int OnClick(BoardManager ctx) {
		Debug.LogFormat ("Piece on click");
		if (ctx.selectedPiece != null) {
			Debug.LogFormat ("Already a piece selected");
			ctx.selectedPiece.OnUnSelected ();

		} else {
			Debug.LogFormat ("A piece is selected");
		}

		ctx.selectedPiece = this;
		OnSelected();

		return 0;
	}
}
