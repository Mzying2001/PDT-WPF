using PDT_WPF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PDT_WPF.Services
{
    public static class Http
    {
        public const int DEFAULT_TIMEOUT = 15000;

        /// <summary>
        /// 常用HttpStatus
        /// </summary>
        public enum HttpStatus
        {
            OK = 200,
            BAD_REQUEST = 400,
            FORBIDDEN = 403,
            NOT_FOUND = 404,
            REQUEST_TIMEOUT = 408,
            SERVICE_UNAVAILABLE = 500
        }

        public static string BuildQuery(IDictionary<string, string> data)
        {
            if (data == null)
                return string.Empty;

            var sb = new StringBuilder();
            int i = 0;
            foreach (var item in data)
            {
                if (i++ > 0)
                    sb.Append("&");
                sb.AppendFormat("{0}={1}", WebUtility.UrlEncode(item.Key), WebUtility.UrlEncode(item.Value));
            }
            return sb.ToString();
        }

        public static string AppendQuery(string url, IDictionary<string, string> data)
        {
            return data != null && data.Count > 0 ? $"{url}?{BuildQuery(data)}" : url;
        }

        public static string Get(string url, string contentType = null, int timeout = DEFAULT_TIMEOUT)
        {
            return Get(url, null, null, contentType, timeout);
        }

        public static string Get(string url, IDictionary<string, string> data, string contentType = null, int timeout = DEFAULT_TIMEOUT)
        {
            return Get(url, data, null, contentType, timeout);
        }

        public static string Get(string url, IDictionary<string, string> data, IDictionary<string, string> headers, string contentType = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AppendQuery(url, data));

            request.Method = "GET";
            request.Timeout = timeout;
            request.ContentType = contentType;

            if (headers != null)
            {
                foreach (var item in headers)
                    request.Headers.Add(item.Key, item.Value);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            string result = null;

            try
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }

            return result;
        }

        public static string Post(string url, string contentType = null, int timeout = DEFAULT_TIMEOUT)
        {
            return Post(url, null, null, contentType, timeout);
        }

        public static string Post(string url, IDictionary<string, string> data, string contentType = null, int timeout = DEFAULT_TIMEOUT)
        {
            return Post(url, data, null, contentType, timeout);
        }

        public static string Post(string url, IDictionary<string, string> data, IDictionary<string, string> headers, string contentType = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.Timeout = timeout;
            request.ContentType = contentType;

            if (headers != null)
            {
                foreach (var item in headers)
                    request.Headers.Add(item.Key, item.Value);
            }

            if (data != null)
            {
                byte[] byteData = Encoding.UTF8.GetBytes(BuildQuery(data));
                request.ContentLength = byteData.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteData, 0, byteData.Length);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = null;
            Stream stream = response.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        public static string Delete(string url, string contentType = null, int timeout = DEFAULT_TIMEOUT)
        {
            return Delete(url, null, null, contentType, timeout);
        }

        public static string Delete(string url, IDictionary<string, string> data, string contentType = null, int timeout = DEFAULT_TIMEOUT)
        {
            return Delete(url, data, null, contentType, timeout);
        }

        public static string Delete(string url, IDictionary<string, string> data, IDictionary<string, string> headers, string contentType = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AppendQuery(url, data));

            request.Method = "DELETE";
            request.Timeout = timeout;
            request.ContentType = contentType;

            if (headers != null)
            {
                foreach (var item in headers)
                    request.Headers.Add(item.Key, item.Value);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            string result = null;

            try
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }

            return result;
        }

        public static string UploadFile(string url, string name, string filePath, IDictionary<string, string> data = null, IDictionary<string, string> headers = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.Timeout = timeout;

            if (headers != null)
            {
                foreach (var item in headers)
                    request.Headers.Add(item.Key, item.Value);
            }

            string boundary = DateTime.Now.Ticks.ToString("X");
            request.ContentType = $"multipart/form-data;boundary={boundary}";

            byte[] itemBoundary = Encoding.UTF8.GetBytes($"\r\n--{boundary}\r\n");
            byte[] endBoundary = Encoding.UTF8.GetBytes($"\r\n--{boundary}--\r\n");
            byte[] fileHead = Encoding.UTF8.GetBytes($"Content-Disposition:form-data;name=\"{name}\";filename=\"{FileUtility.GetFileName(filePath)}\"\r\nContent-Type:application/octet-stream\r\n\r\n");
            byte[] file = FileUtility.ReadBytes(filePath);

            using (Stream reqStream = request.GetRequestStream())
            {
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        byte[] head = Encoding.UTF8.GetBytes($"Content-Disposition:form-data;name=\"{item.Key}\"\r\n\r\n");
                        byte[] content = Encoding.UTF8.GetBytes(item.Value);
                        reqStream.Write(itemBoundary, 0, itemBoundary.Length);
                        reqStream.Write(head, 0, head.Length);
                        reqStream.Write(content, 0, content.Length);
                    }
                }
                reqStream.Write(itemBoundary, 0, itemBoundary.Length);
                reqStream.Write(fileHead, 0, fileHead.Length);
                reqStream.Write(file, 0, file.Length);
                reqStream.Write(endBoundary, 0, endBoundary.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = null;
            Stream stream = response.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
