using System.Runtime.CompilerServices;
using System;
using System.IO;
using  System.Security.Cryptography.X509Certificates;
using  System.Net.Security;
using System.Net;
using Newtonsoft.Json;
using Util;
// JSON 정보를 서버로 보내는 클래스 입니다
namespace Communication.MainServer
{
    
public class MServer
{
    public static string json = "";
    // private static string basicURL = Config.mainServerDNS;
    private static string basicURL = "https://localhost:1215/";

    /// <summary>
    /// POST, PUT, DELETE 메서드를 사용할 경우 이용합니다
    /// </summary>
    /// <param name="uri">MServer 클래스의 basicURL을 확인, 해당부분을 제외한 나머지 주소 입력</param>
    /// <param name="method">"POST", "PUT", "DELETE"</param>
    /// <param name="Info">JSON으로 변환될 클래스</param>
    /// <returns>JSON -> String 값</returns>
    public static String Communicate(string uri, string method, object Info=null)
    {
        uri = basicURL + uri;
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

        // 요청과 보낼 주소 세팅
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.Method = method;
        request.ContentType = "application/json";
        // GET이 아닐 경우.
        if (!method.Equals("GET"))
        {
            // 데이터(body) 직렬화
            string str = JsonConvert.SerializeObject(Info);
            var bytes = System.Text.Encoding.UTF8.GetBytes(str);

            request.ContentLength = bytes.Length;

            // Stream 형식으로 데이터를 보냄
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
            }
        }
        
        // StreamReader 로 역질렬화, 응답 데이터를 받음
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        json = reader.ReadToEnd();
        return json;
    }
       
static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
    return true;
    }
}

public class WebRequestCert : UnityEngine.Networking.CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        //return base.ValidateCertificate(certificateData);
        return true;
    }
 
}

}