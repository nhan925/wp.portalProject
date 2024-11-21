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
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using DotNetEnv;
using Mailjet.Client.TransactionalEmails;
using Newtonsoft.Json;

namespace SpacePortal.ViewModels;
public partial class LoginWindowsViewModel : ObservableRecipient
{

    private static LoginWindowsViewModel _instance;
    public static LoginWindowsViewModel Instance => _instance ??= new LoginWindowsViewModel();

    LoginWindowsViewModel() { }

    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmUserName = string.Empty;

    [ObservableProperty]
    private string _confirmOTP = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private string _confirmNewPassword = string.Empty;

    public class InformationForResetPassword()
    {
        public string email
        {
            get; set;
        }
        public string otp
        {
            get; set;
        }
    }

    private InformationForResetPassword info = new();

    public bool CheckLoginWithRawInformation()
    {
        return App.GetService<ApiService>().Login(UserName, Password);
    }


    public bool CheckUserNameAndSendOTP()
    {
        
        info =  App.GetService<ApiService>().Post<InformationForResetPassword>("/rpc/check_username_and_generate_email_otp", new {v_use_rname = ConfirmUserName});

        if (info.email == "" || info.otp == "")
        {
            return false;
        }
        else
        {
            SendOTPMail();
            return true;
        }

    }

    public async void SendOTPMail()
    {
        Debug.WriteLine($"Sending OTP email to {info.email} with OTP: {info.otp}");

        //TODO
    }


    public bool ValidateOTP()
    {
        var info = new
        {
            v_user_name = ConfirmNewPassword,
            v_otp = ConfirmOTP
        };
        return App.GetService<ApiService>().Post<Boolean>("/rpc/validate_email_otp", info);
    }

    public string CheckPasswordAndConfirmNewPassword()
    {
        if (NewPassword != ConfirmNewPassword)
        {
            return "Password and Confirm Password are not the same";
        }

        if (NewPassword.Length < 6)
        {
            return "Password must be at least 6 characters";
        }

        //TODO: Check password strength

        return "success";
    }

    public bool UpdateNewPassword()
    {
        var info = new
        {
            v_user_name = ConfirmUserName,
            v_new_password = NewPassword,
        };
        return App.GetService<ApiService>().Post<Boolean>("/rpc/update_new_password", info);
    }

}

