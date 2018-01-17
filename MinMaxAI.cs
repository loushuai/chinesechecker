using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxAI : AI {
    private Piece bestPiece = null;
    private Vector2 bestMove;
    private int maxDepth = 2;
    private Evaluator evaluator;

    public MinMaxAI (BoardManager ctx) : base (ctx) {
        evaluator = new Evaluator(ctx); 
    }

    private int MinMax (int player, int depth) {
        int bestValue;

        if (depth == 0) {
            return evaluator.Evaluate();
        }

        if (player == 0) {
            bestValue = -9999;
        } else {
            bestValue = 9999;
        }

        foreach (Piece piece in ctx.pieces[player]) {
            piece.UpdateAvaliableMove(ctx);
            HashSet<Vector2> avaliableMoves = new HashSet<Vector2>(piece.avaliableNextMove);
            foreach (Vector2 move in avaliableMoves) {
                // debug
                //Debug.LogFormat("Before move: {0}, {1}", piece.row, piece.col);
                Vector2 oldPos = piece.MoveTo((int)move.x, (int)move.y, ctx, false);
                //Debug.LogFormat("After move: {0}, {1}", piece.row, piece.col);

                int value = MinMax((player + 1) % 2, depth - 1); 

                piece.MoveTo((int)oldPos.x, (int)oldPos.y, ctx, false);
                //Debug.LogFormat("After unmove: {0}, {1}", piece.row, piece.col);
                Debug.LogFormat("pos: {0}, {1}, move: {2}, {3}, value {4}", piece.row, piece.col, move.x, move.y, value);

                if (player == 0) {
                    bestValue = Mathf.Max(bestValue, value);
                    if (depth == maxDepth && bestValue == value) {
                        bestPiece = piece;
                        bestMove = move;
                    }
                } else {
                    bestValue = Mathf.Min(bestValue, value);
                }
            }
        }

        return bestValue;
    }

    public override void NextMove(int player) {
        MinMax(player, maxDepth);

        if (bestPiece != null) {
            bestPiece.StepTo((int)bestMove.x, (int)bestMove.y, ctx);
        }
    }
}
