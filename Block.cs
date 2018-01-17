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
	public int row;
	public int col;
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

	public void ChangeColor (Color color) {
		spriteRenderer.color = color;
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

//		ctx.GetBlock (ctx.selectedPiece.row, ctx.selectedPiece.col).UnsetOccupied ();
        ctx.selectedPiece.StepTo (this.row, this.col, ctx);
		ctx.selectedPiece.OnUnSelected ();
		ctx.selectedPiece.UnsetAvaliableJump (ctx);
		ctx.selectedPiece = null;
		return 0;
	}

	public bool IsOccupied() {
		return occupied != -1;
	}

	public void SetOccupied(int player) {
		this.occupied = player;
	}

	public void UnsetOccupied() {
		this.occupied = -1;
	}

	public ArrayList AvaliableJump (int direction, HashSet <int> currentSet) {
		ArrayList ret = new ArrayList();

		if (neighbors[direction] == null || neighbors[direction].IsOccupied() == false) {
			return ret;
		}

		Block next = neighbors[direction].neighbors[direction];

		if (next == null || next.IsOccupied() == true) {
			return ret;
		}

		if (currentSet.Contains((next.row << 6) + next.col)) {
			return ret;
		}

		ret.Add(new Vector2(next.row, next.col));
		currentSet.Add((next.row << 6) + next.col);
		for (int dir = 0; dir < 6; ++dir) {
			ArrayList tmp = next.AvaliableJump (dir, currentSet);
			foreach (Vector2 v in tmp) {
				ret.Add (v);
			}
		}

		return ret;
	}

	public void CalcAvaliableNextMove (HashSet <Vector2> avaliableList) {
		HashSet <int> currentSet = new HashSet <int> ();

		avaliableList.Clear ();
		currentSet.Add ((row << 6) + col);
		for (int dir = 0; dir < 6; ++dir) {
			Block neighbor = neighbors[dir];
			if (neighbor == null) {
				continue;
			} else if (neighbor.IsOccupied () == false) {
				avaliableList.Add (new Vector2 (neighbor.row, neighbor.col));
				currentSet.Add ((neighbor.row << 6) + neighbor.col);
			} else {
				ArrayList tmp = AvaliableJump (dir, currentSet);
				foreach (Vector2 v in tmp) {
					avaliableList.Add (v);
				}
			}
		}
	}
}
