using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Services;
using SpacePortal.Models;
using Syncfusion.XlsIO;
using Newtonsoft.Json.Linq;
using DotNetEnv;
using Newtonsoft.Json;
using System.Windows.Input;
using Windows.Devices.PointOfService;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Security.Cryptography;
using SpacePortal.Contracts.Services;
using Syncfusion.XlsIO.Implementation.Security;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Text.RegularExpressions;
using Sprache;
using Windows.Storage;
using WinRT.SpacePortalVtableClasses;
namespace SpacePortal.ViewModels;
public partial class LoginWindowsViewModel : ObservableRecipient
{
    ResourceLoader resourceLoader = new();
    private static LoginWindowsViewModel _instance;
    public static LoginWindowsViewModel Instance => _instance ??= new LoginWindowsViewModel();

    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isRememberMe = false;

    [ObservableProperty]
    private string _confirmUserName = string.Empty;

    [ObservableProperty]
    private string _confirmOTP = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private string _confirmNewPassword = string.Empty;

    [ObservableProperty]
    private string _emailNotificationCaption = string.Empty;

    [ObservableProperty]
    private string _countDownNotificationCaption = string.Empty;

    private string ResetPasswordToken = string.Empty;

    public int CountDownTime = 60;

    public bool FirstTime { get; set; } = true;

    public LaunchActivatedEventArgs LaunchArgs { get; set; }

    private LoginWindowsViewModel() { }

    public void ResetInstance()
    {
        var newViewModel = new LoginWindowsViewModel();
        newViewModel.FirstTime = FirstTime;
        newViewModel.LaunchArgs = LaunchArgs;
        _instance = newViewModel;     
    }

    public bool CheckLoginWithRawInformation()
    {
        var result = App.GetService<ApiService>().Login(UserName, Password);
        if (result)
        {
            if (IsRememberMe == true)
            {
               SaveLoginInfoToLocal();
            }
        }
        return result;
    }

    public bool CheckUserNameAndSendOTP()
    {
        var info = new
        {
            v_user_name = ConfirmUserName
        };
        var result = App.GetService<ApiService>().Post<Boolean>("/rpc/check_username_and_send_otp", info);
        return result;
    }

    public bool ValidateOTP()
    {
        var info = new
        {
            v_user_name = ConfirmUserName,
            v_otp = ConfirmOTP
        };
        ResetPasswordToken = App.GetService<ApiService>().Post<string>("/rpc/validate_otp", info);
        Debug.WriteLine($"ResetPasswordToken: {ResetPasswordToken}");
        Debug.WriteLine("Confirm UserName: " + ConfirmUserName);
        Debug.WriteLine("OTP: " + ConfirmOTP);
        if (ResetPasswordToken == "")
        {
            return false;
        }
        return true;
    }

    public string CheckPasswordAndConfirmNewPassword()
    {
        var message = "success";
        if (NewPassword != ConfirmNewPassword)
        {
            message = resourceLoader.GetString("Login_Error_ResetPassword01/Text");
            return message;
        }

        if (NewPassword.Length == 0)
        {
            message = resourceLoader.GetString("Login_Error_ResetPassword02/Text");
            return message;
        }

        Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        if (!regex.IsMatch(NewPassword))
        {
            message = resourceLoader.GetString("Login_Error_ResetPassword03/Text");
            return message;
        }
        return message;
    }

    public bool UpdateNewPassword()
    {
        var info = new
        {
            v_user_name = ConfirmUserName,
            v_reset_password_token = ResetPasswordToken,
            v_new_password = NewPassword,
        };
        var result = App.GetService<ApiService>().Post<Boolean>("/rpc/update_new_password", info);
        return result;
    }

    public void SetEmailNotificationCaption()
    {
        var UserEmail = App.GetService<ApiService>().Post<string>("/rpc/get_user_email", new { v_user_name = ConfirmUserName });
        Debug.WriteLine(UserEmail);
        var caption = resourceLoader.GetString("Login_Caption_OTP/Text") + UserEmail;
        EmailNotificationCaption = caption;
    }



    public async Task<bool> LoginWithOutlook()
    {
        var clientID = Env.GetString("AZURE_CLIENT_ID");
        var tenantID = Env.GetString("AZURE_TENANT_ID");
        string[] scpopes = { Env.GetString("AZURE_SCOPE") };

        var app = PublicClientApplicationBuilder.Create(clientID)
            .WithTenantId(tenantID)
            .WithRedirectUri("http://localhost")
            .Build();


        var accounts = await app.GetAccountsAsync();
        var authResult = await app.AcquireTokenInteractive(scpopes).ExecuteAsync();

        var accessToken = authResult.AccessToken;
        var result = App.GetService<ApiService>().LoginWithOutlook(accessToken);

        return result;
    }

    private void encodingPassword(string passwordRaw)
    {
        var passwordInBytes = Encoding.UTF8.GetBytes(passwordRaw);
        var entropyInBytes = new byte[20];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(entropyInBytes);
        }

        var encryptedPasswordInBytes = ProtectedData.Protect(
            passwordInBytes,
            entropyInBytes,
            DataProtectionScope.CurrentUser
        );

        var encryptedPasswordBase64 = Convert.ToBase64String(encryptedPasswordInBytes);
        var entropyInBase64 = Convert.ToBase64String(entropyInBytes);

        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["PasswordInBase64"] = encryptedPasswordBase64;
        localSettings.Values["EntropyInBase64"] = entropyInBase64;
    }

    private string decodingPassword()
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        var encryptedPasswordBase64 = (string)localSettings.Values["PasswordInBase64"];
        var entropyInBase64 = (string)localSettings.Values["EntropyInBase64"];
       
        try
        {
            var encryptedPasswordInBytes = Convert.FromBase64String(encryptedPasswordBase64);
            var entropyInBytes = Convert.FromBase64String(entropyInBase64);

            var passwordInBytes = ProtectedData.Unprotect(
                encryptedPasswordInBytes,
                entropyInBytes,
                DataProtectionScope.CurrentUser
            );

            return Encoding.UTF8.GetString(passwordInBytes);
        }
        catch (FormatException)
        {
            // Handle the exception or log it
            return "";
        }
    }


    private void SaveLoginInfoToLocal()
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["UserName"] = UserName;
        encodingPassword(Password);
    }

    public void LoadLoginInfoFromLocal()
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        if (localSettings.Values.ContainsKey("UserName") && localSettings.Values.ContainsKey("PasswordInBase64") && localSettings.Values.ContainsKey("EntropyInBase64")
            && !String.IsNullOrEmpty(localSettings.Values["UserName"] as string) 
                && !String.IsNullOrEmpty(localSettings.Values["PasswordInBase64"] as string) 
                && !String.IsNullOrEmpty(localSettings.Values["EntropyInBase64"] as string))
        {
            UserName = (string)localSettings.Values["UserName"];
            Password = decodingPassword();
        }
    }

    public void ClearLoginInfo()
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["UserName"] = String.Empty;
        localSettings.Values["PasswordInBase64"] = String.Empty;
        localSettings.Values["EntropyInBase64"] = String.Empty;
    }
}

