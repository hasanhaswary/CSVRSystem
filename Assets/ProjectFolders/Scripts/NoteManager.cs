using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class NoteManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;

    [Header("Firestore")]
    public string collectionName = "UserNotes";
    public string fieldName = "Note";

    private FirebaseAuth auth;
    private FirebaseUser user;
    private FirebaseFirestore db;

    private bool isLoading = false;

    void Start()
    {
        // Initialize Firebase
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        db = FirebaseFirestore.DefaultInstance;

        if (user == null)
        {
            Debug.LogError("No user is logged in!");
            return;
        }

        // Load the stored value from Firestore once
        LoadValue();

        // Update Firestore only when the user finishes editing
        inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    void LoadValue()
    {
        isLoading = true;

        DocumentReference docRef = db.Collection(collectionName).Document(user.UserId);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists && snapshot.ContainsField(fieldName))
                {
                    string value = snapshot.GetValue<string>(fieldName);
                    inputField.text = value;
                }
            }
            else
            {
                Debug.LogError("Failed to load Firestore value: " + task.Exception);
            }

            isLoading = false;
        });
    }

    void OnInputEndEdit(string newValue)
    {
        if (isLoading) return; // prevent writing while loading
        SaveNoteToFirestore(newValue);
    }

    // Call from a button to clear the note
    public void ClearNote()
    {
        if (isLoading) return;

        inputField.text = "";
        SaveNoteToFirestore("");
    }

    // Function to save the note to Firestore
    private void SaveNoteToFirestore(string value)
    {
        DocumentReference docRef = db.Collection(collectionName).Document(user.UserId);
        Dictionary<string, object> update = new Dictionary<string, object>
        {
            { fieldName, value }
        };

        docRef.SetAsync(update, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Firestore updated successfully.");
            }
            else
            {
                Debug.LogError("Failed to update Firestore: " + task.Exception);
            }
        });
    }

    private void OnDestroy()
    {
        // Remove listener to avoid memory leaks
        if (inputField != null)
        {
            inputField.onEndEdit.RemoveListener(OnInputEndEdit);
        }
    }
}
