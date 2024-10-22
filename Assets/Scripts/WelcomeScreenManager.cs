// using UnityEngine;
// using TMPro;
// using Firebase;
// using Firebase.Auth;

// public class WelcomeScreenManager : MonoBehaviour
// {
//     [Header("Welcome Message")]
//     public TMP_Text welcomeMessageText; // Reference to the text object that will display the welcome message
//     public string defaultWelcomeMessage = "Welcome, "; // Default message to be displayed before the username

//     // Firebase Auth
//     private FirebaseAuth auth;
//     private FirebaseUser user;

//     [Header("Scene Change Manager")]
//     public SceneChangeManager sceneChangeManager; // Reference to the SceneChangeManager for scene transitions

//     private void Start()
//     {
//         // Initialize Firebase Auth
//         auth = FirebaseAuth.DefaultInstance;
//         user = auth.CurrentUser;

//         // Set the welcome message with the user's display name
//         if (user != null)
//         {
//             string username = user.DisplayName;
//             welcomeMessageText.text = defaultWelcomeMessage + username;
//         }
//         else
//         {
//             Debug.LogError("No user is logged in.");
//         }
//     }

//     // Method to handle logging out
//     public void LogoutButton()
//     {
//         auth.SignOut(); // Log out the user
//         Debug.Log("User logged out successfully.");

//         // Return to the login screen after logout
//         if (sceneChangeManager != null)
//         {
//             sceneChangeManager.Scene1(); // This should navigate back to your MainMenu or Login screen
//         }
//         else
//         {
//             Debug.LogError("SceneChangeManager is not assigned!");
//         }
//     }
// }
