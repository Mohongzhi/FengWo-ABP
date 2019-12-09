using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.App.Framework.Network
{
    /// <summary>
    /// 服务
    /// </summary>
    public class ServiceHelper
    {
        #region 公共变量
        RestClient client;
        #endregion

        #region 构造
        public ServiceHelper(string baseUrl)
        {
            baseServerUrl = baseUrl;
            client = new RestClient(baseServerUrl);
        }
        #endregion

        /// <summary>
        /// Get数据
        /// </summary>
        /// <param name="routeApi"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public ResponseContent Get(string routeApi)
        {
            var request = new RestRequest(routeApi, Method.GET);
            if (AuthorizationToken?.Length > 0)
            {
                request.AddHeader("Authorization", $"Bearer {AuthorizationToken}");
            }
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ResponseContent>(response.Content); // 返回解析结果
        }

        /// <summary>
        /// Get数据
        /// </summary>
        /// <param name="routeApi"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public ResponseContent Get(string routeApi, Dictionary<string, object> keyValuePairs)
        {
            var request = new RestRequest(routeApi, Method.GET);
            var json = JsonConvert.SerializeObject(keyValuePairs);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            if (AuthorizationToken?.Length > 0)
            {
                request.AddHeader("Authorization", $"Bearer {AuthorizationToken}");
            }
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ResponseContent>(response.Content); // 返回解析结果
        }

        /// <summary>
        /// Get数据
        /// </summary>
        /// <param name="routeApi"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public ResponseContent Get(string routeApi, object keyValuePairs)
        {
            var request = new RestRequest(routeApi, Method.GET);
            var json = JsonConvert.SerializeObject(keyValuePairs);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            if (AuthorizationToken?.Length > 0)
            {
                request.AddHeader("Authorization", $"Bearer {AuthorizationToken}");
            }
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ResponseContent>(response.Content); // 返回解析结果
        }

        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="routeApi"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public ResponseContent Post(string routeApi, Dictionary<string, object> keyValuePairs)
        {
            var request = new RestRequest(routeApi, Method.POST);
            var json = JsonConvert.SerializeObject(keyValuePairs);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            if (AuthorizationToken?.Length > 0)
            {
                request.AddHeader("Authorization", $"Bearer {AuthorizationToken}");
            }
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ResponseContent>(response.Content); // 返回解析结果
        }

        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="routeApi"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public ResponseContent Post(string routeApi, object keyValuePairs)
        {
            var request = new RestRequest(routeApi, Method.POST);
            var json = JsonConvert.SerializeObject(keyValuePairs);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            if (AuthorizationToken?.Length > 0)
            {
                request.AddHeader("Authorization", $"Bearer {AuthorizationToken}");
            }
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ResponseContent>(response.Content); // 返回解析结果
        }

        #region 属性/Properties
        /// <summary>
        /// 基础URL
        /// </summary>
        private string baseServerUrl;

        /// <summary>
        /// 服务URL
        /// </summary>
        public string BaseServerUrl
        {
            get { return baseServerUrl; }
            set
            {
                baseServerUrl = value;
                client = new RestClient(baseServerUrl);
            }
        }

        /// <summary>
        /// 身份令牌
        /// </summary>
        public string AuthorizationToken { get; set; }
        #endregion
    }
}
