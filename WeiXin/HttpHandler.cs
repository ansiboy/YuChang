using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WeiXin
{
    public class HttpHandler
    {
        string token;
        MessageProcesser processer;

        public HttpHandler(string token, MessageProcesser processer)
        {
            if (token == null)
                throw Error.ArugmentNull("token");

            if (processer == null)
                throw Error.ArugmentNull("processer");

            this.token = token;
            this.processer = processer;
        }

        public void Process(HttpRequest request, HttpResponse response)
        {
            if (request == null)
                throw Error.ArugmentNull("request");

            if (response == null)
                throw Error.ArugmentNull("response");

            try
            {
                Trace.WriteLine(DateTime.Now);
                if (!CheckSignature(request))
                {
                    response.Write("Invalid request." + Environment.NewLine);
                    Trace.WriteLine("CheckSignature fail");

                    return;
                }

                var httpMethod = request.HttpMethod.ToLower();
                if (httpMethod == "get")
                {
                    if (request.QueryString["echostr"] != null)
                        response.Write(request.QueryString["echostr"]);

                    return;
                }

                //处理 post
                Stream s = request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                var postStr = Encoding.UTF8.GetString(b);

                if (string.IsNullOrEmpty(postStr))
                {
                    Trace.WriteLine("Post string is null or empty.");
                    return;
                }

                Trace.WriteLine("Post Message:");   
                Trace.WriteLine(postStr);

                var reply = processer.Process(postStr);
                var xml = reply.ToXml();

                response.ContentEncoding = Encoding.UTF8;
                response.Write(xml);

                Trace.WriteLine("Reply Message:");
                Trace.WriteLine(xml);

            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Source);
                Trace.WriteLine(exc.StackTrace);
                throw exc;
            }
            finally
            {
                Trace.Flush();
                Trace.Close();
            }

        }

        private bool CheckSignature(HttpRequest request)
        {
            var signature = request.QueryString["signature"];
            var timestamp = request.QueryString["timestamp"];
            var nonce = request.QueryString["nonce"];

            var expectSignature = CreateSignature(timestamp, nonce, token);
            return expectSignature == signature;
        }

        public static string CreateSignature(string timestamp, string nonce, string token)
        {
            //token = token ?? Token;
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }




    }
}
