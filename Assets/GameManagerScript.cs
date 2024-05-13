using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
	// 配列の宣言
	int[,] map;             // レベルデザイン用の配列
	GameObject[,] field;    // ゲーム管理用の配列
    public GameObject playerPrefab;
	public GameObject boxPrefab;

    /// <summary>
    /// 配列の出力
    /// </summary>
    void PrintArray()
	{
		/*string debugText = "";
		// 二重for文で2次元配列の情報を出力
		for (int y = 0; y < map.GetLength(0); y++) 
		{
			for(int x = 0; x < map.GetLength(1); x++)
			{
				debugText += map[y,x].ToString() + ", ";
			}
			debugText += "\n";	// 改行
		}
		Debug.Log(debugText);*/
	}

	/// <summary>
	/// プレイヤーのインデックスを取得する
	/// </summary>
	/// <returns>プレイヤーのインデックス。見つからなかった場合は-1</returns>
	Vector2Int GetPlayerIndex()
	{
		for (int y = 0; y < field.GetLength(0); y++)
		{
			for (int x = 0; x < field.GetLength(1); x++)
			{
				if (field[y,x] == null)
				{
					continue;
				}
				if (field[y,x].tag == "Player")
				{
					return new Vector2Int(x, y);
				}
			}
			
		}
		// Vector2Int型の(-1, -1)の値を作成する
		return new Vector2Int(-1, -1);
	}

	/// <summary>
	/// 数字の移動
	/// </summary>
	/// <param name="number">現在の値</param>
	/// <param name="moveFrom">移動先</param>
	/// <param name="moveTo">現在地</param>
	/// <returns>移動ができたかどうか</returns>
	bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
	{
		// 移動先が範囲外なら移動不可
		if(moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
		if(moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }	
		// nullチェックをしてからタグのチェックを行う
		if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
		{
			Vector2Int velocity = moveTo - moveFrom;
			bool success = MoveNumber("Box", moveTo, moveTo + velocity);
			if (!success) { return false; }
		}
		// プレイヤー・箱関わらずの移動処理
		field[moveFrom.y, moveFrom.x].transform.position = IndexToPosition(moveTo);
		field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
		field[moveFrom.y, moveFrom.x] = null;
		return true;
	}

	/// <summary>
	/// 座標へ変更
	/// </summary>
	/// <param name="index">現在地</param>
	/// <returns>座標</returns>
	Vector3 IndexToPosition(Vector2Int index)
	{
		return new Vector3(
			index.x - map.GetLength(1) / 2 + 0.5f,
			-index.y + map.GetLength(0) / 2,
			0);
	}

	// Start is called before the first frame update
	void Start()
	{
		// 配列の実態の作成と初期化
		map = new int[,] {
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 2, 0, 0, 0, 0, 0, 0, 2 },
			{ 0, 2, 0, 0, 0, 0, 0, 2, 0 },
			{ 0, 2, 0, 0, 1, 0, 2, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 2, 0, 2, 0, 0, 0 },
            { 0, 0, 0, 0, 2, 0, 0, 0, 0 },
        };
		field = new GameObject
		[
			map.GetLength(0),
			map.GetLength(1)
		];

		for (int y = 0; y < map.GetLength(0); y++)
		{
			for (int x = 0; x < map.GetLength(1); x++)
			{
				if (map[y,x] == 1)
				{
					field[y, x] = Instantiate(
						playerPrefab,
						IndexToPosition(new Vector2Int(x, y)),
						Quaternion.identity
					) ;
				}
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        IndexToPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                    );
                }
            }
			
		}
	}

	// Update is called once per frame
	void Update()
	{
		// 右に移動
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			// メソッド化した処理を使用
			Vector2Int playerIndex = GetPlayerIndex();

			// 移動処理を関数化
			MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
		}

		// 左に移動
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			// メソッド化した処理を使用
			Vector2Int playerIndex = GetPlayerIndex();

			// 移動処理を関数化
			MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));
		}

		// 上に移動
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			// メソッド化した処理を使用
			Vector2Int playerIndex = GetPlayerIndex();

			// 移動処理を関数化
			MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
		}

		// 下に移動
		if( Input.GetKeyDown(KeyCode.DownArrow))
		{
			// メソッド化した処理を使用
			Vector2Int playerIndex = GetPlayerIndex();

			// 移動処理を関数化
			MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));
		}
	}
}
