using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyAI : AI {
	public GreedyAI (BoardManager ctx) : base (ctx) {
	}

	public override void NextMove (int player) {
		Piece bestPiece = null;
		Vector2 bestMove = new Vector2(0, 0);
		int maxGain = 0;

		foreach (Piece piece in ctx.pieces[player]) {
			piece.UpdateAvaliableMove (ctx);
			foreach (Vector2 move in piece.avaliableNextMove) {
				int gain = piece.Gain (move);
				if (gain > maxGain) {
					bestMove = move;
					bestPiece = piece;
					maxGain = gain;
				}
			}
		}

		if (bestPiece != null) {
            bestPiece.StepTo ((int)bestMove.x, (int)bestMove.y, ctx);

            //test
            //Debug.LogFormat("Evaluate: {0}", ctx.Evalueate(player));
		}
	}
}
