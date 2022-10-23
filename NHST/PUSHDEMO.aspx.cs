using Supremes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class PUSHDEMO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            //WebClient client = new WebClient();
            //client.Encoding = ASCIIEncoding.UTF8;
            //string downloadString = client.DownloadString(txturl.Text);
            //string s = downloadString;
            ////ltrcontent.Text = downloadString;

            //loadProduct1(txturl.Text);
        }
       
        public class FCMResponse
        {
            public long multicast_id { get; set; }
            public int success { get; set; }
            public int failure { get; set; }
            public int canonical_ids { get; set; }
            public List<FCMResult> results { get; set; }
        }
        public class FCMResult
        {
            public string message_id { get; set; }
        }

        protected void Push(string DeviceToken)
        {
            string android_firebase_key = "AAAAT24SKGg:APA91bE_sajLWA32ch8KERTcPSyQCVjreL3RA-O8ljyTSV37OPB6MCYcLl5_4jgBtiA6CRCr1gAFqkcdN30Ydt587Vuq3VG6Ac6hsmdIY95dtm_qqr9GCO96KLd_YU2sDH8KnDYy-ruw";
            string SenderIdAndroid = "341149100136";

            string ios_firebase_key = "AAAAX6qzOM0:APA91bFSLSm-aXsYQBJha0-SOZvS94Umh1UaDmKiINhEc5Zv-cf_FvGvkV40ROzF8-Ev-c93L3bpvZggLBG9mv_dT5KpBhHWm66uTVradh-taD6ia13hkx0rrLKiOoo4v3iIfXNmoPC-";
            string SenderIdiOS = "410885765325";

            string cre = DateTime.UtcNow.AddHours(7).ToString("HH:mm dd/MM");

            var SENDER_ID = "";
            var FirebaseKey = "";

            if (ddlType.SelectedValue == "android")
            {
                SENDER_ID = SenderIdAndroid;
                FirebaseKey = android_firebase_key;
            }
            else
            {
                SENDER_ID = SenderIdiOS;
                FirebaseKey = ios_firebase_key;
            }

            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var objNotificationiOS = new
            {
                to = DeviceToken,
                collapse_key = "type_a",
                notification = new
                {
                    body = "ABC",
                    title = "THANH TOÁN HỘ",
                    sound = "default",
                    vibration = "default"
                },
                data = new
                {
                    title = "123",
                    message = "456",
                    image = "https://kyguinhanh.vn/App_Themes/ptite/images/logo.png",
                    link = "https://kyguinhanh.vn/thanh-toan-ho-app.aspx?UID=18680"
                }
            };
            var objNotificationAndroid = new
            {
                to = DeviceToken,
                data = new
                {
                    title = "THANH TOÁN HỘ",
                    message = "ABC",
                    image = "https://kyguinhanh.vn/App_Themes/ptite/images/logo.png",
                    link = "https://kyguinhanh.vn/thanh-toan-ho-app.aspx?UID=18680"
                }
            };


            string jsonNotificationFormat = "";
            if (ddlType.SelectedValue == "android")
                jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotificationAndroid);
            else
                jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotificationiOS);

            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", FirebaseKey));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String responseFromFirebaseServer = tReader.ReadToEnd();

                            FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            if (response.success == 1)
                            {
                                //PJUtils.ShowMessageBoxSwAlert("Thành công", "s", true, Page);
                            }
                            else if (response.failure == 1)
                            {
                                //PJUtils.ShowMessageBoxSwAlert("Thất bại", "e", true, Page);
                            }
                        }
                    }

                }
            }
        }

        protected void btnpush_Click1(object sender, EventArgs e)
        {
            Push(txturl.Text);
        }
    }
}