using UnityEngine;
using System.Collections;

public class UserPerformance {
	// the round number in this game level
	// the round number in this game level
	private int trial;

	private int userChoosenRow;
	private int userChoosenCol;

	private string userChoosenChar;
	// server only allow integer, 0=> choose inncorrect, false, 1=> choose correct = true
	private int trialOutcome;

	//is it the first word? it used in the second game 0=> is not first clicked,  false , 1=> is the first clicked, true
	private int firstTime;

	private string ansWord;
	private int ansRow;
	private int ansCol;
	private float userReactTime;

	public int getTrial() {
		return trial;
	}

	public void setTrial(int trial) {
		this.trial = trial;
	}

	public int getUserChoosenRow() {
		return userChoosenRow;
	}

	public void setUserChoosenRow(int userChoosenRow) {
		this.userChoosenRow = userChoosenRow;
	}

	public int getUserChoosenCol() {
		return userChoosenCol;
	}

	public void setUserChoosenCol(int userChoosenCol) {
		this.userChoosenCol = userChoosenCol;
	}

	public string getUserChoosenChar() {
		return userChoosenChar;
	}

	public void setUserChoosenChar(string userChoosenChar) {
		this.userChoosenChar = userChoosenChar;
	}

	public int getTrialOutcome() {
		return trialOutcome;
	}

	public void setTrialOutcome(int trialOutcome) {
		this.trialOutcome = trialOutcome;
	}

	public int getFirstTime() {
		return firstTime;
	}

	public void setFirstTime(int firstTime) {
		this.firstTime = firstTime;
	}

	public string getAnsWord() {
		return ansWord;
	}

	public void setAnsWord(string ansWord) {
		this.ansWord = ansWord;
	}

	public int getAnsRow() {
		return ansRow;
	}

	public void setAnsRow(int ansRow) {
		this.ansRow = ansRow;
	}

	public int getAnsCol() {
		return ansCol;
	}

	public void setAnsCol(int ansCol) {
		this.ansCol = ansCol;
	}
	public float getUserReactTime(){
		return userReactTime;
	}
	public void setUserReactTime(float userReactTime){
		this.userReactTime = userReactTime;
	}

}

