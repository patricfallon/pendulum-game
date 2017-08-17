using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseLevel : MonoBehaviour
{

	private float resilience;
	private float tenacity;
	private float purpose;
	private float shadow;

	private float health;
	private float attack;
	private float speed;
	private float doubt;

	public float Resilience
	{
		get{ return resilience;}
		set { resilience = value;}
	}

	public float Tenacity
	{
		get{ return tenacity;}
		set { tenacity = value;}
	}

	public float Purpose
	{
		get{ return purpose;}
		set { purpose = value;}
	}

	public float Shadow
	{
		get{ return shadow;}
		set { shadow = value;}
	}

	public float Health
	{
		get{ return health;}
		set { health = value;}
	}

	public float Attack
	{
		get{ return attack;}
		set { attack = value;}
	}

	public float Speed
	{
		get{ return speed;}
		set { speed = value;}
	}

	public float Doubt
	{
		get{ return doubt;}
		set { doubt = value;}
	}

}
