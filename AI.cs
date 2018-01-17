using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI {
	protected BoardManager ctx;

	public AI (BoardManager ctx) {
		this.ctx = ctx;
	}

	public virtual void NextMove (int player) {

	}
}
