using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.models
{  

    public static class Settings
    {
        private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

        private const string UserNameKey = "username_key";
        private static readonly string UserNameDefault = string.Empty;

        public static string UserName
        {
            get { return AppSettings.GetValueOrDefault<string>(UserNameKey, UserNameDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserNameKey, value); }
        }


        private const string UserPasswordKey = "userpassword_key";
        private static readonly string UserPasswordDefault = string.Empty;

        public static string UserPassword
        {
            get { return AppSettings.GetValueOrDefault<string>(UserPasswordKey, UserPasswordDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserPasswordKey, value); }
        }


        private const string UserIdKey = "userid_key";
        private static readonly int UserIdDefault = 0;

        public static int UserId
        {
            get { return AppSettings.GetValueOrDefault<int>(UserIdKey, UserIdDefault); }
            set { AppSettings.AddOrUpdateValue<int>(UserIdKey, value); }
        }

        private const string UrlKey = "url_key";
        private static readonly string UrlDefault = string.Empty;

        public static string UserUrlName
        {
            get { return AppSettings.GetValueOrDefault<string>(UrlKey, UrlDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UrlKey, value); }
        }

        private const string UserDbKey = "db_key";
        private static readonly string DbDefault = string.Empty;

        public static string UserDbName
        {
            get { return AppSettings.GetValueOrDefault<string>(UserDbKey, DbDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserDbKey, value); }
        }

        private const string ColourCodes = "stage_colour_code";
        private static readonly string UserColourCodeDefault = string.Empty;

        public static string StageColourCode
        {
            get { return AppSettings.GetValueOrDefault<string>(ColourCodes, UserColourCodeDefault); }
            set { AppSettings.AddOrUpdateValue<string>(ColourCodes, value); }

        }

        private const string PrefKeyIsLockedkey = "False";
        private static readonly string PrefKeyIsLockedDefault = "False";

        public static string PrefKeyIsLocked
        {
            get { return AppSettings.GetValueOrDefault<string>(PrefKeyIsLockedkey, PrefKeyIsLockedDefault); }
            set { AppSettings.AddOrUpdateValue<string>(PrefKeyIsLockedkey, value); }
        }

        private const string PrefKeyUserDetailsKey = "UserDetails";
        private static readonly string PrefKeyUserDetailsDefault = "UserDefaultDetails";

        public static string PrefKeyUserDetails
        {
            get { return AppSettings.GetValueOrDefault<string>(PrefKeyUserDetailsKey, PrefKeyUserDetailsDefault); }
            set { AppSettings.AddOrUpdateValue<string>(PrefKeyUserDetailsKey, value); }
        }


    }
}
