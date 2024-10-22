using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Threading.Tasks;
using System.Linq;

public class FirebaseScoreManager : MonoBehaviour {

    [Header("Firebase")]
    public FirebaseUser User;
    public DatabaseReference DBreference;

    [Header("User Data")]
    public TMP_Text usernameText;
    public TMP_Text ppeCountText;
    public TMP_Text permitsCountText;
    public TMP_Text hazardsCountText;
    public TMP_Text scoreText;
    public TMP_Text startTimeText;
    public TMP_Text endTimeText;

    [Header("Scoreboard")]
    public GameObject scoreElement;
    public Transform scoreboardContent;

    [Header("Managers")]
    public UIManager uiManager;

    private void Start() {
        InitializeFirebase();
    }

    private void InitializeFirebase() {
        User = FirebaseAuth.DefaultInstance.CurrentUser;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        if (User == null) {
            Debug.Log("No user is currently logged in.");
        } else {
            Debug.Log("User is already logged in.");
            StartCoroutine(LoadUserData());
        }
    }

    public void SetUser(FirebaseUser user) {
        User = user;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(LoadUserData());
    }

    public void FetchLatestUserData() {
        if (User != null) {
            StartCoroutine(LoadUserData());
        } else {
            Debug.LogError("User is not authenticated. Cannot fetch user data.");
        }
    }

    public void SaveStartTime() {
        if (User != null) {
            string startTime = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            StartCoroutine(UpdateStartTime(startTime));
        } else {
            Debug.LogError("User is not authenticated. Cannot save start time.");
        }
    }

    public void SaveEndTime() {
        if (User != null) {
            string endTime = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            StartCoroutine(UpdateEndTime(endTime));
        } else {
            Debug.LogError("User is not authenticated. Cannot save end time.");
        }
    }

    private IEnumerator UpdateStartTime(string startTime) {
        Task startTimeTask = DBreference.Child("users").Child(User.UserId).Child("startTime").SetValueAsync(startTime);
        yield return new WaitUntil(() => startTimeTask.IsCompleted);

        if (startTimeTask.IsFaulted) {
            Debug.LogError($"Failed to update start time: {startTimeTask.Exception}");
        } else {
            Debug.Log("Start time updated successfully.");
        }
    }

    private IEnumerator UpdateEndTime(string endTime) {
        Task endTimeTask = DBreference.Child("users").Child(User.UserId).Child("endTime").SetValueAsync(endTime);
        yield return new WaitUntil(() => endTimeTask.IsCompleted);

        if (endTimeTask.IsFaulted) {
            Debug.LogError($"Failed to update end time: {endTimeTask.Exception}");
        } else {
            Debug.Log("End time updated successfully.");
        }
    }

    public void SaveUserData(string username, int totalPPE, int totalPermits, int totalHazards, int totalScore) {
        if (User != null) {
            string startTime = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            string endTime = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            StartCoroutine(UpdateUserData(username, totalPPE, totalPermits, totalHazards, totalScore, startTime, endTime));
        } else {
            Debug.LogError("User is not authenticated. Cannot save user data.");
        }
    }

    public void ScoreboardButton()
    {        
        StartCoroutine(LoadScoreboardData());
    }

    private IEnumerator LoadUserData() {
        if (usernameText == null || ppeCountText == null || permitsCountText == null || hazardsCountText == null || scoreText == null || startTimeText == null || endTimeText == null) {
            Debug.LogError("One or more UI references are not assigned in the Inspector.");
            yield break;
        }

        if (User == null) {
            Debug.LogError("User is null. Make sure the user is logged in.");
            yield break;
        }

        Task<DataSnapshot> DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning($"Failed to load user data: {DBTask.Exception}");
        } else {
            DataSnapshot snapshot = DBTask.Result;

            string username = snapshot.HasChild("username") ? snapshot.Child("username").Value.ToString() : "Unknown User";
            string ppe = snapshot.HasChild("ppe") ? snapshot.Child("ppe").Value.ToString() : "0";
            string permits = snapshot.HasChild("permits") ? snapshot.Child("permits").Value.ToString() : "0";
            string hazards = snapshot.HasChild("hazards") ? snapshot.Child("hazards").Value.ToString() : "0";
            string score = snapshot.HasChild("score") ? snapshot.Child("score").Value.ToString() : "0";
            string startTime = snapshot.HasChild("startTime") ? snapshot.Child("startTime").Value.ToString() : "Not started yet";
            string endTime = snapshot.HasChild("endTime") ? snapshot.Child("endTime").Value.ToString() : "Not completed yet";

            usernameText.text = $"Username: {User.DisplayName}";
            ppeCountText.text = $"PPE: {ppe}";
            permitsCountText.text = $"Permits: {permits}";
            hazardsCountText.text = $"Hazards: {hazards}";
            scoreText.text = $"Score: {score}";
            startTimeText.text = $"Start Time: {startTime}";
            endTimeText.text = $"End Time: {endTime}";
        }
    }

    private IEnumerator UpdateUserData(string username, int ppe, int permits, int hazards, int score, string startTime, string endTime) {
        var tasks = new List<Task>
        {
            DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(User.DisplayName),
            DBreference.Child("users").Child(User.UserId).Child("ppe").SetValueAsync(ppe),
            DBreference.Child("users").Child(User.UserId).Child("permits").SetValueAsync(permits),
            DBreference.Child("users").Child(User.UserId).Child("hazards").SetValueAsync(hazards),
            DBreference.Child("users").Child(User.UserId).Child("score").SetValueAsync(score),
            DBreference.Child("users").Child(User.UserId).Child("startTime").SetValueAsync(startTime),
            DBreference.Child("users").Child(User.UserId).Child("endTime").SetValueAsync(endTime)
        };

        foreach (Task task in tasks) {
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsFaulted) {
                Debug.LogError($"Failed to update data: {task.Exception}");
            }
        }

        Debug.Log("User data updated successfully.");
    }

    private IEnumerator LoadScoreboardData() {
        Debug.Log("Attempting to load scoreboard data...");

        Task<DataSnapshot> DBTask = DBreference.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning($"Failed to load scoreboard data: {DBTask.Exception}");
        } else {
            DataSnapshot snapshot = DBTask.Result;
            Debug.Log($"Data retrieved. Total users found: {snapshot.ChildrenCount}");

            foreach (Transform child in scoreboardContent.transform) {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>()) {
                Debug.Log($"Processing User: {childSnapshot.Key}");

                string username = childSnapshot.Child("username").Value?.ToString() ?? "Unknown";
                int hazards = int.TryParse(childSnapshot.Child("hazards").Value?.ToString(), out int hz) ? hz : 0;
                int permits = int.TryParse(childSnapshot.Child("permits").Value?.ToString(), out int pm) ? pm : 0;
                int ppe = int.TryParse(childSnapshot.Child("ppe").Value?.ToString(), out int pp) ? pp : 0;
                int score = int.TryParse(childSnapshot.Child("score").Value?.ToString(), out int sc) ? sc : 0;

                Debug.Log($"User: {username}, PPE: {ppe}, Permits: {permits}, Hazards: {hazards}, Score: {score}");

                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, ppe, permits, hazards, score);
            }

            uiManager.ScoreboardUIScreen();
        }
    }
}