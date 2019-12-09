using FengWo.App.Framework;
using FengWo.App.Framework.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FengWo.App.ClientLib
{
    /// <summary>
    /// 客户端方法
    /// </summary>
    public static class Clientlib
    {
        /// <summary>
        /// 基础服务
        /// </summary>
        private static ServiceHelper Service;

        #region 方法

        #region Get method
        public static ResponseContent Get(string routeApi)
        {
            return Service.Get(routeApi);
        }

        public static ResponseContent Get(string routeApi, object keyValuePairs)
        {
            return Service.Get(routeApi, keyValuePairs);
        }
        #endregion

        #region Post method
        public static ResponseContent Post(string routeApi, object keyValuePairs)
        {
            return Service.Post(routeApi, keyValuePairs);
        }
        #endregion

        #endregion

        #region 账户
        #region 身份验证
        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="userNameOrEmail"></param>
        /// <param name="password"></param>
        /// <param name="isRemember"></param>
        /// <returns></returns>
        public static bool Authenticate(string userNameOrEmail, string password, bool isRememberClient)
        {
            var json = new { userNameOrEmailAddress = userNameOrEmail, password = password, rememberClient = isRememberClient };
            var result = Service.Post("api/TokenAuth/Authenticate", json);
            if (result?.Success==true)
            {
                Service.AuthorizationToken = result.Result["accessToken"].ToString();

                var informations = Service.Get("/api/services/app/Session/GetCurrentLoginInformations");
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(informations.Result["user"].ToString());
                userId = Convert.ToInt32(data["id"].ToString());
                emailAddress = data["emailAddress"].ToString();
                userName = data["userName"].ToString();
            }
            return result?.Success??false;
        }
        #endregion

        #region 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="SurName"></param>
        /// <param name="UserName"></param>
        /// <param name="EmailAddress"></param>
        /// <param name="Password"></param>
        /// <param name="CaptchaResponse"></param>
        /// <returns></returns>
        public static ResponseContent Register(string Name,string SurName,string UserName,string EmailAddress,string Password,string CaptchaResponse)
        {
            var json = new { name = Name, surname = SurName, userName = UserName, emailAddress = EmailAddress, password = Password, captchaResponse = CaptchaResponse };
            return Service.Post("/api/services/app/Account/Register", json);
        }
        #endregion
        #endregion

        #region 属性/Properties

        /// <summary>
        /// 用户Id
        /// </summary>
        private static int userId;

        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public static int UserId
        {
            get { return userId; }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        private static string userName;

        /// <summary>
        /// 当前登录用户名
        /// </summary>
        public static string UserName
        {
            get { return userName; }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        private static string emailAddress;

        /// <summary>
        /// 当前登录用户邮箱
        /// </summary>
        public static string EmailAddress
        {
            get { return emailAddress; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static string serverUrl;

        /// <summary>
        /// 服务端URL,包含端口号
        /// </summary>
        public static string ServerUrl
        {
            get { return serverUrl; }
            set
            {
                serverUrl = value;
                Service = new ServiceHelper(serverUrl);
            }
        }
        #endregion
    }
}
