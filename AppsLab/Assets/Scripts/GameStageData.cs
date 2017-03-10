using UnityEngine;
using System.Collections;

public class GameStageData {
	private int gameOneClearedLevels;
	private int gameTwoClearedLevels;
	private int gameThreeClearedLevels;
	private int gameFourClearedLevels;

	public GameStageData (int game1, int game2, int game3, int game4){
		gameOneClearedLevels = game1;
		gameTwoClearedLevels = game2;
		gameThreeClearedLevels = game3;
		gameFourClearedLevels = game4;
	}

	public string printAllLevels (){
		return gameOneClearedLevels + " " + gameTwoClearedLevels + " " + gameThreeClearedLevels + " " + gameFourClearedLevels;
	}
}
