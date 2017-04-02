using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace DfssVideoApp
{
    public class DFSSAPIClient
    {
        private HttpClientHandler mHandler;
        private HttpClient mHttpClient;
        public int SubjectID;

    
        public DFSSAPIClient()
        {
            InitHttpClient();
        }


        public LoginResponse Login(string uid,string pwd)
        {
            var request = new {
                username = uid,
                password = pwd,
                videoDomain = "www.dfsstv.cn"
            };
            var requestJson = JsonConvert.SerializeObject(request);
            string url = "http://api.dfsstv.cn/api/v1/User/Login";
            var responseJson = PostJson(url, requestJson);
            var response = JsonConvert.DeserializeObject<LoginResponse>(responseJson);
            if (response.status == 0) {
                SetHttpHeader("AuthToken", response.data.token);
                SubjectID = response.data.userProfile.learningSubjectId;
            }
            return response;
        }


        public SubjectResponse GetSubJect(int subjectId)
        {
            string url = "http://api.dfsstv.cn/api/v1/Subject/Get?subjectId=" + subjectId;
            var responseJson = GetString(url);
            var response = JsonConvert.DeserializeObject<SubjectResponse>(responseJson);
            return response;
        }

        public StartVideoResponse StartVideo(string videoId)
        {
            string url = "http://api.dfsstv.cn/api/v1/Stream/Start?videoId=" + videoId;
            var responseJson = PostString(url, "");
            var response = JsonConvert.DeserializeObject<StartVideoResponse>(responseJson);
            return response;
        }

        public BaseResponse EndVideo(string streamId)
        {
            string url = "http://api.dfsstv.cn/api/v1/Stream/End?streamId=" + streamId;
            var responseJson = PostString(url, "");
            var response = JsonConvert.DeserializeObject<BaseResponse>(responseJson);
            return response;
        }

        public BaseResponse FinishLesson(int lessonId)
        {
            string url = "http://api.dfsstv.cn/api/v1/Practice/Finish?lessonId=" + lessonId;
            var responseJson = PostString(url, "");
            var response = JsonConvert.DeserializeObject<BaseResponse>(responseJson);
            return response;
        }


        private void InitHttpClient()
        {
            mHandler = new HttpClientHandler();
            mHandler.UseCookies = true;
            mHandler.AutomaticDecompression = DecompressionMethods.GZip;
            mHandler.AllowAutoRedirect = true;
            mHttpClient = new HttpClient(mHandler);
            mHttpClient.DefaultRequestHeaders.ExpectContinue = false;
            SetHttpHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            SetHttpHeader("Accept-Language", "zh-CN,zh;q=0.8,en;q=0.6,zh-TW;q=0.4,ja;q=0.2");
            SetHttpHeader("Accept-Encoding", "gzip, deflate");

            //ApiKey: 59d71859d3dd491a8ad6accbe7262d94
            //AuthToken: 1b5b9599f5764cd1b897ab233b08937a
            SetHttpHeader("ApiKey", "59d71859d3dd491a8ad6accbe7262d94");
            SetHttpHeader("AuthToken", "1b5b9599f5764cd1b897ab233b08937a");
        }


        private void SetHttpHeader(string name, string value)
        {
            if (mHttpClient.DefaultRequestHeaders.Contains(name))
            {
                mHttpClient.DefaultRequestHeaders.Remove(name);
            }

            mHttpClient.DefaultRequestHeaders.Add(name, value);
        }

        private string PostString(string url, string content, MediaTypeHeaderValue contentType = null)
        {
            //try
            //{
            var scontent  = new StringContent(content);
            if (contentType != null)
            {
                scontent.Headers.ContentType =  contentType;
            }
            
            HttpResponseMessage response = mHttpClient.PostAsync(new Uri(url),scontent).Result;
            string ret = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            return ret;
            //}
            //catch
            // {
            //     InitHttpClient();
            //     return null;
            // }

        }

        private string GetString(string url)
        {


            HttpResponseMessage response = mHttpClient.GetAsync(new Uri(url)).Result;
            string ret = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            return ret;
            //}
            //catch
            // {
            //     InitHttpClient();
            //     return null;
            // }

        }


        private string PostJson(string url,string content)
        {
            return PostString(url, content, MediaTypeHeaderValue.Parse("application/json;charset=UTF-8"));
        }


       
    }
}
