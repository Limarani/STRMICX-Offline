using Newtonsoft.Json;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for CallAPI
/// </summary>
public class CallAPI
{
    public CallAPI()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string Authenticate()
    {

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var client = new HttpClient();

        var url = new UriBuilder("https://ssobeta.amrock.com/as/token.oauth2").ToString();

        var values = new Dictionary<string, string>
            {
               { "grant_type", "client_credentials" },

                { "client_id", "String" },
                { "client_secret", "VDAMxAk6TWXGzXe0IailVvy89pepVfoui2IHp2xfmUCsqpIoqHCIDPWwEPm1pQsg" }
            };

        var content = new FormUrlEncodedContent(values);
        var response = client.PostAsync(url, content).Result;
        if (!response.IsSuccessStatusCode)
            return string.Empty;
        return ExtractToken(response);
    }

    private static string ExtractToken(HttpResponseMessage res)
    {
        var response = JsonConvert.DeserializeObject<AccessToken>(res.Content.ReadAsStringAsync().Result);
        return response.access_token;
    }
    /// POST("https://taxintegrationapiuat.amrock.com/TaxData/Response", jsonString, acctokn);


    public string POST(string jsonContent,string orderno)
    {

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (
            object s,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors
        )
        {
            return true;
        };
        string ac1 = Authenticate();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://taxintegrationapiuat.amrock.com");

            client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authToken);
            //client.DefaultRequestHeaders.Add("IDENTITY_KEY", authToken);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ac1);
            var content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync("https://taxintegrationapiuat.amrock.com/TaxData/Response", content).Result;
            WriteLog(orderno + ": " + result);
            string statusCode = result.StatusCode.ToString();
            string isSuccessCode = result.IsSuccessStatusCode.ToString();
            string reasonPhrase = result.ReasonPhrase.ToString();
            return isSuccessCode;

        }
    }

    private void WriteLog(string content)
    {
        FileStream fs = new FileStream(@"d:\amrocklog.txt", FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);
        sw.BaseStream.Seek(0, SeekOrigin.End);
        sw.WriteLine(content);
        sw.Flush();
        sw.Close();

    }
}
public class AccessToken
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public long expires_in { get; set; }

    public string userName { get; set; }

    public string issued { get; set; }
    public string expires { get; set; }

}