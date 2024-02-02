using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Text;

namespace _3rdEyE.Controllers
{
    public class SMSController : Controller
    {
        public string CallAPI()
        {
            var responseFromServer = "";
            try
            {
                WebRequest request = WebRequest.Create("http://172.17.2.112:90/api/SendTopUp");
                request.Method = "POST";
                request.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjIxNjQxN2U3MjZkNzJiYjNjYjFiMGY4ZTYzOTg4YTdkY2U0NTZhNzRmYmRmYjc2YjUwNjlmYTg5YTExYzhjY2RkY2I1YzJmMmUyZGQ3ZWFmIn0.eyJhdWQiOiIxIiwianRpIjoiMjE2NDE3ZTcyNmQ3MmJiM2NiMWIwZjhlNjM5ODhhN2RjZTQ1NmE3NGZiZGZiNzZiNTA2OWZhODlhMTFjOGNjZGRjYjVjMmYyZTJkZDdlYWYiLCJpYXQiOjE1NjE3Nzg5NDgsIm5iZiI6MTU2MTc3ODk0OCwiZXhwIjoxNTkzNDAxMzQ4LCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.XEkmw2nm6jFooShoWC-khlZycG8HJEOeg2HtYdki777nMLZ-CO9emGefCmRHh4VTbuv8sdhLADwtKdyR9LNhXNKgxjUh8EPfUmJWHoGqeNfAi5K8I8qX4xHjCSzrYqQhoPUYeap8at5bS4FhcBtv3jlUHpOnZoLNs-JwGZmn9kg1-BxitZG3LV3Wn7FfliWF_CUnIT2CKGWEgQyGcZzTpQjZywSyU2iRMG_gRCRnNyQmG1qU7EjvoL9kA2yTbu8pl-TchI0CZRH9wjtBqCdDuotLqoIh8xjt9FBbdbEz1byDWOknH795150Xg7Acqg06V0RkqSEh7FfFBaXr5YWpsp7M0Hpk-DE2nzP5f_KtyMb9I6_qp3vMNISlHR3_jx2_R9UAW1egaX21wcb-o3rGBjO_GPpnj-ai05y_FK7k6FHNFqIuMfyqQ6h0Zrec9udrpp_Nfw4TvzT2gafdc1eM-OE1I8DTYXZzoNqma-QcuL98O9caOueNiqEdBwkr68SMw0SD0RRJA3KhCHxysHvztKBimGwPPEGekQUj-33n6bX8r6Eb9Ky2r-a0NXN0bh7o4CdWsUQdOoBa3gt9PJU1elp1_l6TERaAR89APjwEzybYC_o-ZgV9ezvxq72IMmQWSYiWjyIStiqG0soSmJo5S0IgX7Eqo_CT9Z6dkeWbhj0");
                // Code here for your msisdn, amount, conType, operator
                string postData = "username=ReL&password=w3lc0m3@r3l&msisdn=01835053902&amount=2&conType=prepaid&operator=3";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }
                response.Close();
                return responseFromServer;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
