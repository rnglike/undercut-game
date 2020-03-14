using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float vel;

	public enum modos
	{
		Auto,
		DevTeclado,
		DevMouse
	};
	public modos escolherModos;

	private float terraçoY;
	private float bombaY;

	private Vector3 pos;

	private bool noTeto = false;
	private bool noChao = false;

	//Referências
	private RoomController predio;


	void Awake()
	{
		predio = GameObject.FindGameObjectWithTag("Building").GetComponent<RoomController>();
	}

	void Start()
	{
		pos = transform.position;
		terraçoY = GameObject.FindGameObjectWithTag("Terraço").transform.position.y;
	}

	void Update()
	{
		//Setting bounds.
		if(predio.spawned && bombaY == terraçoY)
		{
			bombaY = GameObject.FindGameObjectWithTag("Sala da Bomba").transform.position.y;
		}
		else if(!predio.spawned)
		{
			bombaY = terraçoY;
		}
	}

	void FixedUpdate()
	{
		if(predio.roomsToSpawn > 0)
		{
			BoundCheck();
	
			//Movimentação da Câmera (manipulando a variável pos)
			if(escolherModos == modos.Auto)
			{
				
			}
			else if(escolherModos == modos.DevTeclado)
			{
				if(Input.GetKey("u") && (!noTeto))
				{
					pos.y += vel;
				}
				else if(Input.GetKey("j") && (!noChao))
				{
					pos.y -= vel;
				}
			}
			else if(escolherModos == modos.DevMouse)
			{
				if
				(
					(!noTeto && Input.mouseScrollDelta.y > 0f) ||
					(!noChao && Input.mouseScrollDelta.y < 0f)
				)
				{
					pos.y += Input.mouseScrollDelta.y * vel;	
				}
			}
		}
		transform.position = pos;
	}

	void BoundCheck()
	{
		if(transform.position.y >= terraçoY)
		{
			if(!noTeto)
			{
				pos.y = terraçoY;
				noTeto = true;
			}
		}
		else
		{
			noTeto = false;
		}

		if(transform.position.y <= bombaY)
		{
			if(!noChao)
			{
				pos.y = bombaY;
				noChao = true;
			}
		}
		else
		{
			noChao = false;
		}
	}
}