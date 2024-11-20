using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrontEnd.Helpers
{
    public static class InitDataHelper
    {
        public static async Task InitSession(HttpContext context, PersonalAccountService personalAccountService, AuthService authService, ViewDataDictionary viewData, string userName)
        {
            try
            {
                if (!String.IsNullOrEmpty(userName) && String.IsNullOrEmpty(context.Session.GetString(userName + Constants.FullNamePrefix)))
                {
                    if (context?.Request?.Cookies[Constants.UserIdCookieKey] != null)
                    {
                        var builder = new UserDataBuilderFromPA(personalAccountService, 
                            context.Request.Cookies[Constants.UserIdCookieKey].ToString(), 
                            userName);
                        var ownerBuilder = new BuilderOwner(builder);
                        await ownerBuilder.Construct();
                        var result = builder.GetResult();

                        viewData[Constants.PersonalAccountDataKey] = result.UserModel;
                        SetSession(context, result);
                    }

                    if (String.IsNullOrEmpty(context.Session.GetString(userName + Constants.FullNamePrefix)))
                    {
                        var builder = new UserDataBuilderFromAuth(authService, userName);
                        var ownerBuilder = new BuilderOwner(builder);
                        await ownerBuilder.Construct();
                        var result = builder.GetResult();
                        SetSession(context, result);
                    }
                }
            }
            catch (Exception ex) { }
        }

        public static void InitViewData(ViewDataDictionary viewData, HttpContext context, string userName)
        {
            var lang = context.Session.GetString(userName + Constants.LanguagePrefix);
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(lang))
            {
                viewData[Constants.CaptionsKey] = Constants.Dictionaries[lang];
                viewData[Constants.LanguageKey] = Constants.Langs[lang];
            }

            if (viewData[Constants.CaptionsKey] == null)
            {
                viewData[Constants.CaptionsKey] = Constants.Dictionaries[Constants.LanguageBase];
                viewData[Constants.LanguageKey] = Constants.Langs[Constants.LanguageBase];
            }

            viewData[Constants.UserFullNameKey] = context.Session.GetString(userName + Constants.FullNamePrefix);
        }

        private static void SetSession(HttpContext context, UserDataModel result)
        {
            if (!String.IsNullOrEmpty(result.Id))
            {
                context.Session.SetString(result.UserLogin, result.Id);
                context.Session.SetString(result.UserLogin + Constants.IsAdminPrefix, result.IsAdmin.ToString());
            }
            if (!String.IsNullOrEmpty(result.FullName))
            {
                context.Session.SetString(result.UserLogin + Constants.FullNamePrefix, result.FullName);
            }

            context.Session.SetString(result.UserLogin + Constants.LanguagePrefix, result.Language);
        }
    }
}
