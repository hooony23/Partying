using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

// JSON 정보를 서버로 보내는 클래스 입니다
public class MServer
{
    public static string json = "";
    private static string basicURL = "https://localhost:5001/";

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

        // GET
        if (method.Equals("GET"))
        {
            // 요청과 보낼 주소 세팅
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.ContentType = "application/json";

            // StreamReader 로 역질렬화, 응답 데이터를 받음
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            json = reader.ReadToEnd();
        }

        // POST, PUT, DELETE
        else
        {
            // 데이터 직렬화
            string str = JsonConvert.SerializeObject(Info);
            var bytes = System.Text.Encoding.UTF8.GetBytes(str);

            // 요청과 보낼 주소 세팅
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;

            // Stream 형식으로 데이터를 보냄
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
            }

            // StreamReader 로 역질렬화, 응답 데이터를 받음
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            json = reader.ReadToEnd();

        }

        return json;
    }
}
