// This source code is a part of NAVER Open API Wrapper.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace NaverOpenAPI
{
    /// <summary>
    /// 작업량이 매우 적은 경우 이 클래스를 사용하세요
    /// </summary>
    internal class SmallRequest
    {
        /// <summary>
        /// 네이버 개발자 공식 홈페이지에서 가져온 메서드입니다.
        /// </summary>
        /// <param name="sess"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Request(Session sess, string url, string data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", sess.ClientId);
            request.Headers.Add("X-Naver-Client-Secret", sess.ClientSecret);
            request.ContentType = "application/json";
            request.Method = "POST";
            string body = data;
            byte[] byteDataParams = Encoding.UTF8.GetBytes(body);
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            return text;
        }
    }
}
