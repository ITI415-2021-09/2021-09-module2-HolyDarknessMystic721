using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eScoreEvent {
    draw,
    mine,
    mineGold,
    gameWin,
    gameLoss
}

//ScoreManager handles all the scoring
public class ScoreManager : MonoBehaviour {
    static private ScoreManager S;

    static public int SCORE_FROM_PREV_ROUND;
    static public int HIGH_SCORE;

    [Header("Set Dynamically")]
    //fields to track score info
    public int chain = 0;
    public int scoreRun = 0;
    public int score = 0;

    void Awake() {
        if (S == null) {
            S = this; // Set the private singleton
        } else {
            Debug.LogError("ERROR: ScoreManager.Awake(): S is already set!");
        }

        //chack for a high score in PlayerPrefs
        if (PlayerPrefs.HasKey("ProspectorHighScore")) {
            HIGH_SCORE = PlayerPrefs.GetInt("ProspectorHighScore");
        }

        //add the score from last round, which will be >0 if it is a win
        score += SCORE_FROM_PREV_ROUND;
        //and reset the SCORE_FROM_PREV_ROUND
        SCORE_FROM_PREV_ROUND = 0;
    }

    static public void EVENT (eScoreEvent evt) {
        try { //try-catch stops an error from breaking the program 
            S.Event(evt);
        } catch (System.NullReferenceException nre) {
            Debug.LogError("ScoreManager:Event() called while S = null.\n" + nre);
        }
    }

    void Event (eScoreEvent evt) {
        switch (evt) {
            // same things need to happen whether it's a draw, a win, or a loss.
            case eScoreEvent.draw: //drawing a card
            case eScoreEvent.gameWin: //won the round
            case eScoreEvent.gameLoss: //lose the round
                chain = 0; //resets the score chain
                score += scoreRun; //adds scoreRun to total score
                scoreRun = 0; //adds scoreRun
                break;

            case eScoreEvent.mine: //removes a mine card
                chain++; // increase the score chain
                scoreRun += chain; // add score for this card to run
                break;
        }

        //this second switch statement handles round wins and losses
        switch (evt) {
            case eScoreEvent.gameWin:
                //if its a win, add a score to the next round
                //static fields are NOT reset by SceneManager.LoadScene()
                SCORE_FROM_PREV_ROUND = score;
                print ("You won this round! Round Score: " + score);
                break;

            case eScoreEvent.gameLoss:
                //if its a loss, check against the high score
                if (HIGH_SCORE <= score) {
                    print ("You got the High Score! High Score: " + score);
                    HIGH_SCORE = score;
                    PlayerPrefs.SetInt("ProspectorHighScore", score);
                } else {
                    print ("Your final score for the game was: " + score);
                }
                break;

            default:
                print ("score: " + score + " scoreRun: " + scoreRun + " chain: " + chain);
                break;
        }
    }

    static public int CHAIN {
        get {
            return S.chain;
        }
    }

    static public int SCORE {
        get {
            return S.score;
        }
    }

    static public int SCORE_RUN {
        get {
            return S.scoreRun;
        }
    }
}
