using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxAI : AI {
    const int POSINF = 9999;
    const int NEGINF = -9999;
    const int WINVALUE = 8888;

    Piece bestPiece = null;
    Vector2 bestMove;
    int maxDepth = 5;

    public MinMaxAI (BoardManager ctx) : base (ctx) {
        //evaluator = new Evaluator(ctx); 
    }

    int MinMax (int player, int depth) {
        int bestValue;

        if (depth == 0) {
            return this.ctx.Evalue(0); // Fixme: always player 0?
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

    int AlphaBeta(int player, int depth, int alpha, int beta) {
        int bestValue = 0;

        if (this.ctx.Evalue(0) == -2) {
            return WINVALUE;
        }

        if (depth == 0) {
            return this.ctx.Evalue(0); // Fixme: always player 0?
        }

        if (player == 0) {
            bestValue = NEGINF;
        } else {
            bestValue = POSINF;
        }

        foreach (Piece piece in ctx.pieces[player]) {
            piece.UpdateAvaliableMove(ctx);
            HashSet<Vector2> avaliableMoves = new HashSet<Vector2>(piece.avaliableNextMove);
            foreach (Vector2 move in avaliableMoves) {
                Vector2 oldPos = piece.MoveTo((int)move.x, (int)move.y, ctx, false);

                int value = AlphaBeta((player + 1) % 2, depth - 1, alpha, beta);
                value -= (maxDepth - depth);

                piece.MoveTo((int)oldPos.x, (int)oldPos.y, ctx, false);

                //Debug.LogFormat("*** player {0}, depth {1}, value {2}", player, depth, value); 

                if (player == 0) {
                    bestValue = Mathf.Max(bestValue, value);
                    alpha = Mathf.Max(alpha, bestValue);

                    if (depth == maxDepth && bestValue == value) {
                        bestPiece = piece;
                        bestMove = move;

                        //Debug.LogFormat("*** player {0}, depth {1}, value {2}, from {5} {6}, to {3} {4}", player, depth, value, move.x, move.y, oldPos.x, oldPos.y); 
                    }
                } else {
                    bestValue = Mathf.Min(bestValue, value);
                    beta = Mathf.Min(beta, bestValue);
                }

                //Debug.LogFormat("**** bestvalue {0}, alpha {1}, beta {2}", bestValue, alpha, beta);

                if (beta <= alpha) {
                    break;
                }
            }
        }

        return bestValue;
    }

    public override void NextMove(int player) {
        //MinMax(player, maxDepth);
        AlphaBeta(player, maxDepth, NEGINF, POSINF);

        if (bestPiece != null) {
            bestPiece.MoveTo((int)bestMove.x, (int)bestMove.y, ctx);
        }
    }
}
