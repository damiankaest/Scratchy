using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

public static class FirebaseWrapper
{
    private static FirebaseApp _firebaseApp;

    public static void InitializeFirebase()
    {
        if (_firebaseApp == null)
        {
            string basePath = AppContext.BaseDirectory;
            string jsonPath = Path.Combine(basePath, "Initializer", "serviceAccountKey.json");

            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException($"Die Datei {jsonPath} wurde nicht gefunden.");
            }

            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(jsonPath),
            });
        }
    }

    public static async Task<FirebaseToken> VerifyTokenAsync(string idToken)
    {
        if (_firebaseApp == null)
        {
            throw new InvalidOperationException("FirebaseApp wurde nicht initialisiert. Rufe InitializeFirebase() zuerst auf.");
        }

        var auth = FirebaseAuth.GetAuth(_firebaseApp);
        return await auth.VerifyIdTokenAsync(idToken);
    }
}
