using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Abp.Authorization.Users;
using Abp.Extensions;

namespace FengWo.Authorization.Users
{
    public class User : AbpUser<User>
    {
        /// <summary>
        /// 默认密码
        /// </summary>
        public const string DefaultPassword = "00000";

        /// <summary>
        /// 工贸技师学院教师工号
        /// </summary>
        public string GMGH { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [EmailAttribute]
        public override string EmailAddress { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var text = value as string;
            if (string.IsNullOrEmpty(text))
                return true;
            else
            {
                var EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
                var regex = new Regex(EmailRegex);
                return regex.IsMatch(text);
            }
        }
    }
}
