using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Compilation;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
	// �z��̐錾
	int[,] map;             // ���x���f�U�C���p�̔z��
	GameObject[,] field;    // �Q�[���Ǘ��p�̔z��
	public GameObject playerPrefab;
	public GameObject boxPrefab;
	public GameObject goalPrefab;

	public GameObject clearText;

	public GameObject particlePrefab;

	/// <summary>
	/// �z��̏o��
	/// </summary>
	void PrintArray()
	{
		/*string debugText = "";
		// ��dfor����2�����z��̏����o��
		for (int y = 0; y < map.GetLength(0); y++) 
		{
			for(int x = 0; x < map.GetLength(1); x++)
			{
				debugText += map[y,x].ToString() + ", ";
			}
			debugText += "\n";	// ���s
		}
		Debug.Log(debugText);*/
	}

	/// <summary>
	/// �v���C���[�̃C���f�b�N�X���擾����
	/// </summary>
	/// <returns>�v���C���[�̃C���f�b�N�X�B������Ȃ������ꍇ��-1</returns>
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
		// Vector2Int�^��(-1, -1)�̒l���쐬����
		return new Vector2Int(-1, -1);
	}

	/// <summary>
	/// �����̈ړ�
	/// </summary>
	/// <param name="number">���݂̒l</param>
	/// <param name="moveFrom">�ړ���</param>
	/// <param name="moveTo">���ݒn</param>
	/// <returns>�ړ����ł������ǂ���</returns>
	bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
	{
		for (int i = 0; i < 5; i++)
		{
			Instantiate(
				particlePrefab,
				IndexToPosition(moveFrom),
				Quaternion.identity
			) ;
		}

		// �ړ��悪�͈͊O�Ȃ�ړ��s��
		if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
		if(moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }	
		// null�`�F�b�N�����Ă���^�O�̃`�F�b�N���s��
		if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
		{
			Vector2Int velocity = moveTo - moveFrom;
			bool success = MoveNumber("Box", moveTo, moveTo + velocity);
			if (!success) { return false; }
		}
		// �v���C���[�E���ւ�炸�̈ړ�����
		//field[moveFrom.y, moveFrom.x].transform.position = IndexToPosition(moveTo);
		Vector3 moveToPosition = IndexToPosition(moveTo);
		field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
		field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
		field[moveFrom.y, moveFrom.x] = null;

		return true;
	}

	/// <summary>
	/// ���W�֕ύX
	/// </summary>
	/// <param name="index">���ݒn</param>
	/// <returns>���W</returns>
	Vector3 IndexToPosition(Vector2Int index)
	{
		return new Vector3(
			index.x - map.GetLength(1) / 2 + 0.5f,
			-index.y + map.GetLength(0) / 2,
			0);
	}


	bool IsCleard()
	{
		// Vector2Int�^�̉ϒ��z��̍쐬
		List<Vector2Int> goals = new List<Vector2Int>();

		for (int y = 0; y < map.GetLength(0); y++)
		{
			for(int x = 0; x < map.GetLength(1); x++)
			{
				// �i�[�ꏊ���ۂ��𔻒f
				if (map[y, x] == 3)
				{
					// �i�[�ꏊ�̃C���f�b�N�X���T���Ă���
					goals.Add(new Vector2Int(x, y));
				}
			}
		}

		// �v�f����goals.Count�Ŏ擾
		for(int i = 0; i < goals.Count; i++) 
		{
			GameObject f = field[goals[i].y, goals[i].x];
			if (f == null || f.tag != "Box")
			{
				// 1�ł���������������������B��
				return false;
			}
		}
		// �������B���łȂ���Ώ����B��
		return true;
	}

	// Start is called before the first frame update
	void Start()
	{
		Screen.SetResolution(1280, 720, false);

		// �z��̎��Ԃ̍쐬�Ə�����
		map = new int[,] {
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 3, 2, 0, 0, 1, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 3, 2, 0, 0, 0, 2, 3 },
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
				if (map[y, x] == 3)
				{
					Instantiate(
						goalPrefab,
						IndexToPosition(new Vector2Int(x, y)) + new Vector3(0, 0, 0.01f),
						Quaternion.identity
					);
				}
			}
			
		}
	}

	// Update is called once per frame
	void Update()
	{
		// �E�Ɉړ�
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			// ���\�b�h�������������g�p
			Vector2Int playerIndex = GetPlayerIndex();

			// �ړ��������֐���
			MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));

			// �����N���A���Ă�����
			if (IsCleard())
			{
				// �Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
				clearText.SetActive(true);
			}
		}

		// ���Ɉړ�
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			// ���\�b�h�������������g�p
			Vector2Int playerIndex = GetPlayerIndex();

			// �ړ��������֐���
			MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));

			// �����N���A���Ă�����
			if (IsCleard())
			{
				// �Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
				clearText.SetActive(true);
			}
		}

		// ��Ɉړ�
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			// ���\�b�h�������������g�p
			Vector2Int playerIndex = GetPlayerIndex();

			// �ړ��������֐���
			MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));

			// �����N���A���Ă�����
			if (IsCleard())
			{
				// �Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
				clearText.SetActive(true);
			}
		}

		// ���Ɉړ�
		if( Input.GetKeyDown(KeyCode.DownArrow))
		{
			// ���\�b�h�������������g�p
			Vector2Int playerIndex = GetPlayerIndex();

			// �ړ��������֐���
			MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));

			// �����N���A���Ă�����
			if (IsCleard())
			{
				// �Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
				clearText.SetActive(true);
			}
		}
	}
}
