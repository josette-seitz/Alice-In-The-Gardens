using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDiscoBallColors : MonoBehaviour {

	public List<Material> ballColor;
	public List<GameObject> discoBall;
	private int index = 0;

	void OnEnable () {

        StartCoroutine(ChangeDiscoColorFiveSeconds());
		
	}
	
	IEnumerator ChangeDiscoColorFiveSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            ChangeDiscoColor();
        }
    }

    //change disco ball every .5 seconds
    private void ChangeDiscoColor()
	{
        foreach (GameObject disco in discoBall)
        {
            disco.GetComponentInChildren<MeshRenderer>().material = ballColor[index];
        }

        index++;

        if (index == ballColor.Count)
        {
            index = 0;
        }
    }
}
