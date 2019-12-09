using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Localization;

namespace FengWo.EntityFrameworkCore.Seed.Host
{
    public class DefaultLanguagesCreator
    {
        public static List<ApplicationLanguage> InitialLanguages => GetInitialLanguages();

        public static List<ApplicationLanguageText> InitalLanguageTexts => GetInitialLanguageTexts();

        private readonly FengWoDbContext _context;


        /// <summary>
        /// 初始化语言
        /// </summary>
        /// <returns></returns>
        private static List<ApplicationLanguage> GetInitialLanguages()
        {
            return new List<ApplicationLanguage>
            {
                new ApplicationLanguage(null, "zh-CN", "简体中文", "famfamfam-flags cn"),
                new ApplicationLanguage(null, "en", "English", "famfamfam-flags gb"),
                //new ApplicationLanguage(null, "ar", "العربية", "famfamfam-flags sa"),
                //new ApplicationLanguage(null, "de", "German", "famfamfam-flags de"),
                //new ApplicationLanguage(null, "it", "Italiano", "famfamfam-flags it"),
                //new ApplicationLanguage(null, "fr", "Français", "famfamfam-flags fr"),
                //new ApplicationLanguage(null, "pt-BR", "Português", "famfamfam-flags br"),
                //new ApplicationLanguage(null, "tr", "Türkçe", "famfamfam-flags tr"),
                //new ApplicationLanguage(null, "ru", "Русский", "famfamfam-flags ru"),
                //new ApplicationLanguage(null, "es-MX", "Español México", "famfamfam-flags mx"),
                //new ApplicationLanguage(null, "nl", "Nederlands", "famfamfam-flags nl"),
                //new ApplicationLanguage(null, "ja", "日本語", "famfamfam-flags jp")
            };
        }

        private static List<ApplicationLanguageText> GetInitialLanguageTexts()
        {
            return new List<ApplicationLanguageText>
            {
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="AreYouSure",  Value = "您确定吗?", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="Cancel",  Value = "取消!", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="DefaultError",  Value = "发生了错误!", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="DefaultError401",  Value = "您未通过身份验证！!", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="DefaultError403",  Value = "您并未拥有权限!", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="DefaultError404",  Value = "找不到该资源!", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="DefaultErrorDetail",  Value = "服务器未发送错误详细信息.", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="DefaultErrorDetail401",  Value = "您应该经过身份验证（登录）才能执行此操作.", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="DefaultErrorDetail403",  Value = "您无权执行此操作.", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="DefaultErrorDetail404",  Value = "在服务器上找不到请求的资源.", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="EntityNotFound",  Value = "没有ID为{1}的实体对象{0}!", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="InternalServerError",  Value = "您的请求期间发生内部错误!", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="ValidationError",  Value = "您的请求无效!", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="ValidationNarrativeTitle",  Value = "验证期间检测到以下错误.", },
                new ApplicationLanguageText(){ LanguageName="zh-CN", Source="AbpWeb", Key="Yes",  Value = "是", },

            };
        }

        public DefaultLanguagesCreator(FengWoDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateLanguages();

            CreateLanguageTexts();
        }

        private void CreateLanguages()
        {
            foreach (var language in InitialLanguages)
            {
                AddLanguageIfNotExists(language);
            }
        }

        private void CreateLanguageTexts()
        {
            foreach (var language in InitalLanguageTexts)
            {
                AddLanguageTextIfNotExists(language);
            }
        }

        private void AddLanguageIfNotExists(ApplicationLanguage language)
        {
            if (_context.Languages.IgnoreQueryFilters().Any(l => l.TenantId == language.TenantId && l.Name == language.Name))
            {
                return;
            }

            _context.Languages.Add(language);
            _context.SaveChanges();
        }

        private void AddLanguageTextIfNotExists(ApplicationLanguageText language)
        {
            if (_context.LanguageTexts.IgnoreQueryFilters().Any(l => l.TenantId == language.TenantId && l.LanguageName == language.LanguageName && l.Source == language.Source && l.Key == language.Key))
            {
                return;
            }

            _context.LanguageTexts.Add(language);
            _context.SaveChanges();
        }
    }
}
