using UnityEngine;
using System.Collections;

public class GameUtils  {

	public static int findRowPosition(int currentRow, int currentCol, int position ){
		int purePosition = position % (currentRow * currentCol);
		if (purePosition == 0) {
			purePosition = currentRow * currentCol;
		}
		if (purePosition % currentCol != 0) {
			return (purePosition / currentCol + 1);
		}
		if (purePosition % currentCol == 0) {
			return (purePosition / currentCol);
		}
		return 0;
	}

	public static int findColPosition(int currentRow, int currentCol, int position ){
		int purePosition = position % (currentRow * currentCol);
		if (purePosition == 0) {
			purePosition = currentRow * currentCol;
		}
		if (purePosition % currentCol != 0) {
			return purePosition % currentCol;
		}
		if (purePosition % currentCol == 0) {
			return currentCol;
		}
		return 0; 
	}



}
