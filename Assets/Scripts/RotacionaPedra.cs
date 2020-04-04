using UnityEngine;
using System.Collections;

public class RotacionaPedra : MonoBehaviour {
	
	void Update () {
		transform.Rotate(new Vector3(0, 0, 1), 30 * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag("Player")||(other.gameObject.CompareTag("tiro")))
			{
			Destroy(gameObject);
			}
	}
}
