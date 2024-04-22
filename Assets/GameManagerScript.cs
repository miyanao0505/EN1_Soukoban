using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
	// 配列の宣言
	int[] map;
	/*int[,] map;*/

	/// <summary>
	/// 配列の出力
	/// </summary>
	void PrintArray()
	{
		string debugText = "";
		for(int i = 0; i < map.GetLength(0); i++)
		/* for(int y = ; y < map.GetLength(0); y++) */
		{
			/*for(int x = 0; x < map.GetLength(1); x++)
			{
				deebugText += map.[y,x].ToString() + ", ";
			}
			debugText += "\n";	// 改行*/
			debugText += map[i].ToString() + ", ";
		}
		Debug.Log(debugText);
	}

	/// <summary>
	/// プレイヤーのインデックスを取得する
	/// </summary>
	/// <returns>プレイヤーのインデックス。見つからなかった場合は-1</returns>
	int GetPlayerIndex()
	{
		for (int i = 0; i < map.Length; i++)
		{
			if (map[i] == 1)
			{
				return i;
			}
		}
		return -1;
	}

	/// <summary>
	/// 数字の移動
	/// </summary>
	/// <param name="number">現在の値</param>
	/// <param name="moveFrom">移動先</param>
	/// <param name="moveTo">現在地</param>
	/// <returns>移動ができたかどうか</returns>
	bool MoveNumber(int number, int moveFrom, int moveTo)
	{
		// 移動先が範囲外なら移動不可
		if (moveTo < 0 || moveTo >= map.Length)
		{
			// 動けない条件を先に書き、リターンする。早期リターン
			return false;
		}
		// 移動先に2(箱)が居たら
		if (map[moveTo] == 2)
		{
			// どの方向へ移動するかを算出
			int velocity = moveTo - moveFrom;
			// プレイヤーの移動先から、さらに先へ2(箱)を移動させる。
			// 箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを
			// 呼び、処理が再帰している。移動不可をboolで記録
			bool success = MoveNumber(2, moveTo, moveTo + velocity);
			// もし箱が移動失敗したら、プレイヤーの移動も失敗
			if (!success) { return false; }
		}
		// プレイヤー・箱関わらずの移動処理
		map[moveTo] = number;
		map[moveFrom] = 0;
		return true;
	}

	// Start is called before the first frame update
	void Start()
	{
		// 配列の実態の作成と初期化
		map = new int[]{ 0, 0, 2, 1, 2, 0, 2, 2, 0 };
		/*map = new int[,] {
			{ 0, 0, 0, 0, 0 },
			{ 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0 },
		};*/ 

		//Debug.Log("Hello world!");

		// 配列の出力
		PrintArray();
    }

	// Update is called once per frame
	void Update()
	{
		// 右に移動
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			// メソッド化した処理を使用
			int playerIndex = GetPlayerIndex();

			// 移動処理を関数化
			MoveNumber(1, playerIndex, playerIndex + 1);

			// 配列の出力
			PrintArray();
		}

		// 左に移動
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			// メソッド化した処理を使用
			int playerIndex = GetPlayerIndex();

			// 移動処理を関数化
			MoveNumber(1, playerIndex, playerIndex - 1);

			// 配列の出力
			PrintArray();
		}
	}
}
