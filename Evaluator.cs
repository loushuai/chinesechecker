using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluator {
    private BoardManager board;
    public Evaluator(BoardManager board) {
        this.board = board;
    }
	
    public int Evaluate() {
        int ret = 0;
        int alpha = 1;
        int player = 0;

        for (player = 0; player < 2; ++player) {
            foreach (Piece p in board.pieces[player]) {
                ret += alpha * p.Score();
            }
            alpha = -alpha;
        }

        return -ret;
    }
}
