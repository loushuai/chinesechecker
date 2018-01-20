using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject boardPrefab;
	private GameObject board;
	private BoardManager boardManager;
	private AI simpleai;
    private AI alphabetaai;

	void Awake () {
		this.board = Instantiate (boardPrefab, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
		this.boardManager = this.board.GetComponent <BoardManager> ();
        //this.ai = new GreedyAI (this.boardManager);
        this.simpleai = new GreedyAI(this.boardManager);
        this.alphabetaai = new MinMaxAI(this.boardManager);
	}

	void DetectClick () {
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Mouse down" + Input.mousePosition);

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				Debug.Log ("Hit by mouse\t");
				ClickableMonoBehaviour clickable = hit.transform.GetComponent <ClickableMonoBehaviour> ();
				clickable.OnClick (this.boardManager);
			} else {
				// just for test
                //if (boardManager.Evalue(0) >= -12 ) {
                //    simpleai.NextMove(0);
                //} else {
                    alphabetaai.NextMove(0);    
                //}				
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		DetectClick ();
	}
}
