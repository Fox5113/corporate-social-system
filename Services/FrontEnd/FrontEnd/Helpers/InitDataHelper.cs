using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrontEnd.Helpers
{
    public static class InitDataHelper
    {
        public static async Task InitSession(HttpContext context, PersonalAccountService personalAccountService, AuthService authService, ViewDataDictionary viewData, string userName)
        {
            try
            {
                if (!String.IsNullOrEmpty(userName) && String.IsNullOrEmpty(context.Session.GetString(userName)))
                {
                    if (context?.Request?.Cookies[Constants.UserIdCookieKey] != null)
                    {
                        try
                        {
                            var userModel = await personalAccountService.GetPersonalAccountData(context.Request.Cookies[Constants.UserIdCookieKey].ToString());
                            viewData[Constants.PersonalAccountDataKey] = userModel;
                            context.Session.SetString(userName, userModel.Id.ToString());
                            context.Session.SetString(userName + Constants.FullNamePrefix, userModel.Firstname + " " + userModel.Surname);
                            context.Session.SetString(userName + Constants.LanguagePrefix, !String.IsNullOrEmpty(userModel.Language) ? userModel.Language : Constants.LanguageBase);
                            context.Session.SetString(userName + Constants.IsAdminPrefix, userModel.IsAdmin.ToString());
                        }
                        catch (Exception ex) { }
                    }

                    if (!String.IsNullOrEmpty(userName) && String.IsNullOrEmpty(context.Session.GetString(userName)))
                    {
                        var userModel = await authService.GetUserByLogin(userName);
                        if (userModel != null)
                        {
                            context.Session.SetString(userName, userModel.Id.ToString());
                            context.Session.SetString(userName + Constants.FullNamePrefix, userModel.Name);
                            context.Session.SetString(userName + Constants.LanguagePrefix, Constants.LanguageBase);
                        }
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
    }
}
