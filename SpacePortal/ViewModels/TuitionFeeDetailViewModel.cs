using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.DataAccess;
using SpacePortal.Models;
using System.Diagnostics;
using SpacePortal.Core.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using DotNetEnv;
using Microsoft.Windows.ApplicationModel.Resources;

namespace SpacePortal.ViewModels;

public partial class TuitionFeeDetailViewModel : ObservableRecipient
{

    public TuitionFeeDetailInformations Informations
    {
        get; set;
    }

    public ObservableCollection<TuitionFeeDetailCourse> courseDetail
    {
        get; set;
    }


    public void LoadInformations(int semesterID, string year, int semester, int totalCourse, int totalTuitionFee)
    {

        courseDetail = (App.GetService<IDao<TuitionFeeDetailCourse>>() as TuitionFeeDetailCourseDao).GetCourseList(year, semester);
        Informations = new TuitionFeeDetailInformations();
        Informations.semesterID = semesterID;
        Informations.year = year;
        Informations.semester = semester;
        Informations.totalCourse = totalCourse;
        Informations.totalTuitionFee = totalTuitionFee;
    }

    public string appTransIdRaw { get; set; }

    public ZaloPayService zaloPayService => App.GetService<ZaloPayService>();

    public string CallApiToPayment()
    {
        ResourceLoader resourceLoader = new ResourceLoader();
        var description01 = resourceLoader.GetString("TuitionFeeDetail_ZaloPayDescription_01/Text");
        var description02 = resourceLoader.GetString("TuitionFeeDetail_ZaloPayDescription_02/Text");
        var description03 = resourceLoader.GetString("TuitionFeeDetail_ZaloPayDescription_03/Text");
        var description04 = resourceLoader.GetString("TuitionFeeDetail_ZaloPayDescription_04/Text");

        var description = $"{description01} {description03} {Informations.semester} - {description04} {Informations.year}";
        var amount = Informations.totalTuitionFee;
        var itemName = $"{description02} {Informations.semester} - {description04} {Informations.year}";

        var order_url = zaloPayService.CallApiToPayment(itemName, amount, description);
        return order_url;
    }

    public async Task<bool> CheckPaymentStatus()
    {
        var result = await zaloPayService.CheckStatusPayment();
        return result;
    }


    public void AddPaymentHistoryToDatabase(bool is_success)
    {
        var info = new
        {
            v_amount = Informations.totalTuitionFee,
            v_is_success = is_success,
            v_semester_id = Informations.semesterID
        };
        var result = App.GetService<ApiService>().Post<Boolean>("/rpc/add_new_tuition_fee_for_payment_history", info);
    }

}
