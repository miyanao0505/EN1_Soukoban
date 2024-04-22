using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
	// �z��̐錾
	int[] map;
	/*int[,] map;*/

	/// <summary>
	/// �z��̏o��
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
			debugText += "\n";	// ���s*/
			debugText += map[i].ToString() + ", ";
		}
		Debug.Log(debugText);
	}

	/// <summary>
	/// �v���C���[�̃C���f�b�N�X���擾����
	/// </summary>
	/// <returns>�v���C���[�̃C���f�b�N�X�B������Ȃ������ꍇ��-1</returns>
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
	/// �����̈ړ�
	/// </summary>
	/// <param name="number">���݂̒l</param>
	/// <param name="moveFrom">�ړ���</param>
	/// <param name="moveTo">���ݒn</param>
	/// <returns>�ړ����ł������ǂ���</returns>
	bool MoveNumber(int number, int moveFrom, int moveTo)
	{
		// �ړ��悪�͈͊O�Ȃ�ړ��s��
		if (moveTo < 0 || moveTo >= map.Length)
		{
			// �����Ȃ��������ɏ����A���^�[������B�������^�[��
			return false;
		}
		// �ړ����2(��)��������
		if (map[moveTo] == 2)
		{
			// �ǂ̕����ֈړ����邩���Z�o
			int velocity = moveTo - moveFrom;
			// �v���C���[�̈ړ��悩��A����ɐ��2(��)���ړ�������B
			// ���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h��
			// �ĂсA�������ċA���Ă���B�ړ��s��bool�ŋL�^
			bool success = MoveNumber(2, moveTo, moveTo + velocity);
			// ���������ړ����s������A�v���C���[�̈ړ������s
			if (!success) { return false; }
		}
		// �v���C���[�E���ւ�炸�̈ړ�����
		map[moveTo] = number;
		map[moveFrom] = 0;
		return true;
	}

	// Start is called before the first frame update
	void Start()
	{
		// �z��̎��Ԃ̍쐬�Ə�����
		map = new int[]{ 0, 0, 2, 1, 2, 0, 2, 2, 0 };
		/*map = new int[,] {
			{ 0, 0, 0, 0, 0 },
			{ 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0 },
		};*/ 

		//Debug.Log("Hello world!");

		// �z��̏o��
		PrintArray();
    }

	// Update is called once per frame
	void Update()
	{
		// �E�Ɉړ�
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			// ���\�b�h�������������g�p
			int playerIndex = GetPlayerIndex();

			// �ړ��������֐���
			MoveNumber(1, playerIndex, playerIndex + 1);

			// �z��̏o��
			PrintArray();
		}

		// ���Ɉړ�
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			// ���\�b�h�������������g�p
			int playerIndex = GetPlayerIndex();

			// �ړ��������֐���
			MoveNumber(1, playerIndex, playerIndex - 1);

			// �z��̏o��
			PrintArray();
		}
	}
}
