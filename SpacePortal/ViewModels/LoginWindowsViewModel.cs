using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Services;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;
public partial class LoginWindowsViewModel: ObservableRecipient
{
    public event EventHandler LoginSuccess;

    [ObservableProperty]
    private string _userName;

    [ObservableProperty]
    private string _password;

    public bool CheckLoginWithRawInformation()
    {
        return App.GetService<ApiService>().Login(UserName, Password);
    }

}
