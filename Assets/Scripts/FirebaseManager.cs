using System;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class FirebaseManager : MonoBehaviour {

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;

    [Header("Managers")]
    public UIManager uiManager;
    public FirebaseScoreManager firebaseScoreManager;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    void Start() {
        Debug.Log("Awake started for FirebaseManager.");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            dependencyStatus = task.Result;
            Debug.Log("Firebase dependencies checked.");
            
            if (dependencyStatus == DependencyStatus.Available) {
                InitializeFirebase();
                Debug.Log("Firebase initialized.");
            } else {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

        Debug.Log("Awake completed for FirebaseManager.");
    }

    private void InitializeFirebase() {
        try {
            Debug.Log("Initializing Firebase Auth.");
            auth = FirebaseAuth.DefaultInstance;

            Debug.Log("Getting current user.");
            User = auth.CurrentUser;

            Debug.Log("Firebase initialization complete.");

            if (User != null) {
                Debug.Log("User is already logged in.");
            } else {
                Debug.Log("No user is currently logged in.");
            }
        } catch (Exception ex) {
            Debug.LogError($"Exception during Firebase initialization: {ex.Message}");
        }
    }

    public void ClearLoginFields() {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }

    public void ClearRegisterFields() {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }

    private bool ValidateEmail(string email) {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }

    public void LoginButton() {
        string email = emailLoginField.text.Trim();
        string password = passwordLoginField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) {
            warningLoginText.text = "Please enter both email and password!";
            StartCoroutine(ClearWarningAfterDelay(warningLoginText));
            return;
        }

        StartCoroutine(Login(email, password));
    }

    public void RegisterButton() {
        string email = emailRegisterField.text.Trim();
        string username = usernameRegisterField.text.Trim();
        string password = passwordRegisterField.text;
        string passwordVerify = passwordRegisterVerifyField.text;

        if (!ValidateEmail(email)) {
            warningRegisterText.text = "Invalid Email Format!";
            StartCoroutine(ClearWarningAfterDelay(warningRegisterText));
            return;
        }

        if (password != passwordVerify) {
            warningRegisterText.text = "Passwords do not match!";
            StartCoroutine(ClearWarningAfterDelay(warningRegisterText));
            return;
        }

        if (string.IsNullOrEmpty(username)) {
            warningRegisterText.text = "Missing Username";
            StartCoroutine(ClearWarningAfterDelay(warningRegisterText));
            return;
        }

        StartCoroutine(Register(email, password, username));
    }

    public void SignOutButton() {
        auth.SignOut();
        uiManager.LoginScreen();
        ClearRegisterFields();
        ClearLoginFields();
        Debug.Log("User signed out successfully.");
    }

    private IEnumerator ClearWarningAfterDelay(TMP_Text warningText) {
        yield return new WaitForSeconds(2);
        warningText.text = "";
    }

    private IEnumerator Login(string email, string password) {
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => LoginTask.IsCompleted);

        if (LoginTask.Exception != null) {
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode) {
                case AuthError.MissingEmail: message = "Missing Email"; break;
                case AuthError.MissingPassword: message = "Missing Password"; break;
                case AuthError.WrongPassword: message = "Wrong Password"; break;
                case AuthError.InvalidEmail: message = "Invalid Email"; break;
                case AuthError.UserNotFound: message = "Account does not exist"; break;
            }
            warningLoginText.text = message;
            StartCoroutine(ClearWarningAfterDelay(warningLoginText));
        } else {
            User = LoginTask.Result.User;
            Debug.Log("User logged in successfully.");

            if (firebaseScoreManager != null) {
                firebaseScoreManager.SetUser(User);
            }

            confirmLoginText.text = "Logged In";
            yield return new WaitForSeconds(1);
            confirmLoginText.text = "";

            ClearLoginFields();
            ClearRegisterFields();
            uiManager.UnitUIScreen();
        }
    }

    private IEnumerator Register(string email, string password, string username) {
        Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null) {
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Register Failed!";
            switch (errorCode) {
                case AuthError.MissingEmail: message = "Missing Email"; break;
                case AuthError.MissingPassword: message = "Missing Password"; break;
                case AuthError.WeakPassword: message = "Weak Password"; break;
                case AuthError.EmailAlreadyInUse: message = "Email Already In Use"; break;
            }
            warningRegisterText.text = message;
            StartCoroutine(ClearWarningAfterDelay(warningRegisterText));
        } else {
            User = RegisterTask.Result.User;

            if (User != null) {
                UserProfile profile = new UserProfile { DisplayName = username };
                Task ProfileTask = User.UpdateUserProfileAsync(profile);
                yield return new WaitUntil(() => ProfileTask.IsCompleted);

                if (ProfileTask.Exception != null) {
                    FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                    warningRegisterText.text = "Username Set Failed!";
                    StartCoroutine(ClearWarningAfterDelay(warningRegisterText));
                } else {
                    Debug.Log("User profile updated successfully.");

                    if (firebaseScoreManager != null) {
                        firebaseScoreManager.SetUser(User);
                    }

                    uiManager.LoginScreen();
                    ClearLoginFields();
                    ClearRegisterFields();
                }
            }
        }
    }
}