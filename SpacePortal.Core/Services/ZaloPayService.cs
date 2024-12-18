using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

using SpacePortal.Core.Helpers.ZaloPayHelper; // HmacHelper, Utils, HttpHelper
using SpacePortal.Core.Helpers.ZaloPayHelper.Crypto;

namespace SpacePortal.Core.Services;
public class ZaloPayService
{
    private readonly string _appId;
    private readonly string _key1;
    private readonly string _redircetUrl;
    public string AppTransIdRaw
    {
        get; set;
    }

    public ZaloPayService(string appID, string key1, string redircetUrl)
    {
        this._appId = appID;
        this._key1 = key1;
        _redircetUrl = redircetUrl;
    }

    public string CallApiToPayment(string itemName, int fee, string description)
    {
        var create_order_url = "https://sb-openapi.zalopay.vn/v2/create";

        Random rnd = new Random();
        var app_trans_id = DateTime.Now.ToString("yyMMdd") + "_" + rnd.Next(1000000);
        this.AppTransIdRaw = app_trans_id;

        var embed_data = new
        {
            redirecturl = this._redircetUrl
        };
        var items = new[] { new { item_name = itemName, item_price = fee } };

        var param = new Dictionary<string, string>();
        param.Add("app_id", this._appId);
        param.Add("app_user", "student_portal_user");
        param.Add("app_time", Utils.GetTimeStamp().ToString());
        param.Add("amount", fee.ToString());
        param.Add("app_trans_id", app_trans_id);
        param.Add("embed_data", JsonConvert.SerializeObject(embed_data));
        param.Add("item", JsonConvert.SerializeObject(items));
        param.Add("description", description);
        param.Add("bank_code", "");
        param.Add("expire_duration_seconds", "300");

        var data = $"{this._appId}|{param["app_trans_id"]}|{param["app_user"]}|{param["amount"]}|{param["app_time"]}|{param["embed_data"]}|{param["item"]}";
        param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, this._key1, data));

        try
        {
            var result = Task.Run(async () => await HttpHelper.PostFormAsync(create_order_url, param)).Result;


            if (result != null)
            {
                if (result.ContainsKey("return_code") && result["return_code"].ToString() == "1")
                {

                    if (result.ContainsKey("order_url"))
                    {
                        return result["order_url"].ToString();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            return string.Empty;
        }

        return string.Empty;
    }

    public async Task<bool> CheckStatusPayment()
    {
        var app_id = this._appId;
        var key1 = this._key1;
        var query_order_url = "https://sb-openapi.zalopay.vn/v2/query";

        if (string.IsNullOrEmpty(this.AppTransIdRaw))
        {
            return false;
        }

        var param = new Dictionary<string, string>
        {
            { "app_id", app_id },
            { "app_trans_id", this.AppTransIdRaw } 
        };

        var data = $"{app_id}|{param["app_trans_id"]}|{key1}";
        param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

        try
        {
            var result = await HttpHelper.PostFormAsync(query_order_url, param);

            if (result != null)
            {
                if (result.ContainsKey("return_code") && result["return_code"].ToString() == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Lỗi khi kiểm tra trạng thái thanh toán: " + ex.Message);
        }

        return false;
    }

}
