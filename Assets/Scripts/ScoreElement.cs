using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour {

    public TMP_Text usernameText;
    public TMP_Text ppeText;
    public TMP_Text permitsText;
    public TMP_Text hazardsText;
    public TMP_Text scoreText;

    public void NewScoreElement (string _username, int _ppe, int _permits, int _hazards, int _score) {
        if (usernameText == null || ppeText == null || permitsText == null || hazardsText == null || scoreText == null) {
            Debug.LogError("UI elements in ScoreElement are not assigned!");
            return;
        }

        Debug.Log($"Creating ScoreElement - Username: {_username}, PPE: {_ppe}, Permits: {_permits}, Hazards: {_hazards}, Score: {_score}");

        usernameText.text = _username;
        ppeText.text = _ppe.ToString();
        permitsText.text = _permits.ToString();
        hazardsText.text = _hazards.ToString();
        scoreText.text = _score.ToString();
    }
}