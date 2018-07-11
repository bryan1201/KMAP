using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Jayrock.Json;
using System.Net;
using Jayrock.Json.Conversion;
using System.IO;
using System.Web;
using System.Collections;
using System.Collections.Specialized;

namespace KMAPRequest
{
    public partial class Form1 : Form
    {
        private string DataFormat = "JSON";                             // (必須)API資料傳輸格式，建議預設使用JSON
        private string userid = "admin";                                // (必須)KM系統中有權限讀寫的帳號，建議使用系統管理者帳號
        private string tenant = "default";
        private string KMUrl = "http://km.inventec.com/ESP/api/";       // (必須)KM Server Site的API虛擬目錄URL路徑
        private string GlobalCurrentDocumentId = "";                    // 暫時無作用,不需要去動它
        private string GlobalCurrentCategoryId = "1";                   // 暫時無作用,不需要去動它
        private string GlobalSearchKeyword = "KM";
        private string API_Key = "1722e63fda0740e4aff711836d9ca0bc";    // (必須)KM系統中已註冊並啟用的API Key


        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            this.JsonResultTxt.Text = string.Empty;

            WebClient client2 = new WebClient();
            client2.Encoding = Encoding.UTF8;
            string targetAdvSearchUrl = GetServiceUrl(ServiceType.GetAdvancedResult);

            ArrayList al = new ArrayList();
            NameValueCollection nvc = new NameValueCollection();

            nvc.Add("enabletagsynonyms", "false");
            nvc.Add("enablekeywordsynonyms", "false");
            nvc.Add("containchildcategory", "false");
            nvc.Add("containchildfolder", "false");
            nvc.Add("keyword", this.txtAdvKeyword.Text.Trim());

            if (this.txtAdvFolderID.Text.Trim() != string.Empty)
            {
                nvc.Add("folder", this.txtAdvFolderID.Text.Trim());
            }

            string uploadData = "";
            foreach (string k in nvc.Keys)
            {
                al.Add(k + "=" + nvc[k]);
            }
            uploadData = string.Join("&", (string[])al.ToArray(typeof(string)));


            byte[] bytesAdvSearch = Encoding.UTF8.GetBytes(uploadData);
            byte[] bytesAdvSearchResult = null;
            string stringAdvSearchResult = "";

            try
            {
                client2.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                bytesAdvSearchResult = client2.UploadData(targetAdvSearchUrl, bytesAdvSearch);
                stringAdvSearchResult = Encoding.UTF8.GetString(bytesAdvSearchResult);
            }
            catch (Exception ex)
            {
                string err = "GetAdvancedResult " + ex.ToString();
                return;
            }
            finally
            {
                this.JsonResultTxt.Text = stringAdvSearchResult;
            }
        }

        private string GetServiceUrl(ServiceType serviceType)
        {
            string ServiceUrl = "";

            switch (serviceType)
            {
                case ServiceType.GetRootFolder:
                    ServiceUrl = GetKmUrl() + "folder/root/public?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.UploadFile:
                    ServiceUrl = GetKmUrl() + "upload?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AcquireDocumentDraft:
                    ServiceUrl = GetKmUrl() + "document/acquirenewdocumentdraft?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.SubmitNewDocument:
                    ServiceUrl = GetKmUrl() + "document/new?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AllDocumentClass:
                    ServiceUrl = GetKmUrl() + "documentclass/all/enabled?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetCategoryInfo:
                    ServiceUrl = GetKmUrl() + "category/" + GlobalCurrentCategoryId + "?load_path=False&shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetSearchExBySimple:
                    ServiceUrl = GetKmUrl() + "search/ext/simple/" + GlobalSearchKeyword + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetAdvancedResult:
                    ServiceUrl = GetKmUrl() + "search/advancedresult" + "?shell=true&tid=0&api_key=" + API_Key + "&who=" + userid + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetDocument:
                    ServiceUrl = GetKmUrl() + "document/" + GlobalCurrentDocumentId.ToString() + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AddUser:
                    ServiceUrl = GetKmUrl() + "user/add" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetUser:
                    ServiceUrl = GetKmUrl() + "user/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetUserBySubjectId:
                    ServiceUrl = GetKmUrl() + "user/exact/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.UpdateUser:
                    ServiceUrl = GetKmUrl() + "user/update" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.DeleteUser:
                    ServiceUrl = GetKmUrl() + "user/delete/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetAllUser:
                    ServiceUrl = GetKmUrl() + "user/all" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.DownloadFile2:
                    ServiceUrl = GetKmUrl() + "download2/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AcquireFolderDraft:
                    ServiceUrl = GetKmUrl() + "folder/newdraft/{0}" + "?shell=true&tid=0&pi=0&ps=10&api_key=" + API_Key + "&who=" + userid + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                    break;
                case ServiceType.AddFolder:
                    ServiceUrl = GetKmUrl() + "folder/new/{0}" + "?shell=true&tid=0&api_key=" + API_Key + "&who=" + userid + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                    break;
                default:
                    break;
            }
            return ServiceUrl;
        }

        private string GetKmUrl()
        {
            return KMUrl;
        }
    }
}
