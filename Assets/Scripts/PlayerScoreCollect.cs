using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "score")
        {
            ScoreManager.scoreCount +=10;
        }
    }
}
