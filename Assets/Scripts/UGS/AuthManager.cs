using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();

        // Register the Unity Player Accounts sign-in event handler after services initialization.
        PlayerAccountService.Instance.SignedIn += SignInWithUnityAuth;
    }

    // Call this from a button or other application-specific trigger to begin the sign-in flow.
    public async void StartPlayerAccountsSignInAsync()
    {
        if (PlayerAccountService.Instance.IsSignedIn)
        {
            // If the player is already signed into Unity Player Accounts, proceed directly to the Unity Authentication sign-in.
            SignInWithUnityAuth();
            return;
        }

        try
        {
            // This will open the system browser and prompt the user to sign in to Unity Player Accounts
            await PlayerAccountService.Instance.StartSignInAsync();
        }
        catch (PlayerAccountsException ex)
        {
            // Compare error code to PlayerAccountsErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    async void SignInWithUnityAuth()
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
