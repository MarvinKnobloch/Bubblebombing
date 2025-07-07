using UnityEngine;
using System;
using System.Collections;

public class Zahlenrad : MonoBehaviour
{
	public float rotationSpeed = 600;
	public float rotationCount = 7;
	public float slowDown = 250;
	public int targetNumber = 1;
	public int valueOffset = 0;

	public float speed;

	public Action<int> onValueChanged;

	public int Roll()
	{
		targetNumber = UnityEngine.Random.Range(1, 5);
		gameObject.SetActive(true);
		StartCoroutine(Rotate());
		return targetNumber;
	}

	[ContextMenu("ForceShow1")]
	public void ForceShow1()
	{
		ForceShowValue(1);
	}

	[ContextMenu("ForceShow2")]
	public void ForceShow2()
	{
		ForceShowValue(2);
	}

	[ContextMenu("ForceShow3")]
	public void ForceShow3()
	{
		ForceShowValue(3);
	}

	[ContextMenu("ForceShow4")]
	public void ForceShow4()
	{
		ForceShowValue(4);
	}

	public void ForceShowValue(int value)
	{
		targetNumber = value;
		float angle = (targetNumber + valueOffset - 1) * 90;
		transform.localRotation = Quaternion.Euler(angle, 0, 0);
	}

	IEnumerator Rotate()
	{
		speed = rotationSpeed;
		if (rotationCount < 1) rotationCount = 1;
		float leftRotations = rotationCount * 360f;
		float angleCorrection = (targetNumber + valueOffset - 1) * 90;
		//float angle = transform.localRotation.eulerAngles.x;
		while (leftRotations > 720)
		{
			leftRotations -= Time.deltaTime * speed;
			transform.localRotation = Quaternion.Euler(-leftRotations + angleCorrection, 0, 0);
			yield return null;
		}
		// Slow down
		while (leftRotations > 0)
		{
			leftRotations -= Time.deltaTime * speed;
			transform.localRotation = Quaternion.Euler(-leftRotations + angleCorrection, 0, 0);
			speed -= Time.deltaTime * slowDown;
			yield return null;
		}

		//ebug.Log("Value: " + targetNumber);
		onValueChanged?.Invoke(targetNumber);
	}
}
