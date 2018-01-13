using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : ClickableMonoBehaviour {
	private SpriteRenderer spriteRenderer;		//Store a component reference to the attached SpriteRenderer.	
	public float width;
	public float height;
	public Block [] neighbors = new Block[6];
	public float x;
	public float y;
	public int occupied = -1; // if occupied by any piece

	void Awake ()
	{
		//Get a component reference to the SpriteRenderer.
		spriteRenderer = GetComponent<SpriteRenderer> ();
		width = spriteRenderer.size.x;
		height = spriteRenderer.size.y;

		for (int i = 0; i < 6; ++i) {
			neighbors [i] = null;
		}

		Transform transform = GetComponent<Transform> ();
		x = transform.position.x;
		y = transform.position.y;
//		Debug.LogFormat ("x: {0}, y: {1}", transform.position.x, transform.position.y);
	}

	public void ChangeColor (int r, int g, int b, float a = 1f) {
		spriteRenderer.color = new Color (r, g, b, a);
	}

	public void SetNeighbor(int direction, Block neighbor) {
		neighbors [direction] = neighbor;
	}

	public Block GetBeighbor(int direction) {
		if (neighbors [direction] == null) {
			return null;
		}
		return neighbors [direction];
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override int OnClick(BoardManager ctx) {
//		Debug.LogFormat ("Block on click");
		if (ctx.selectedPiece == null) {
			return 0;
		}

		ctx.selectedPiece.MoveTo (this.x, this.y);
		ctx.selectedPiece.OnUnSelected ();
		ctx.selectedPiece = null;
		return 0;
	}

	public bool IsOccupied() {
		return occupied != -1;
	}

	public void SetOccupied(int player) {
		this.occupied = player;
	}
}
