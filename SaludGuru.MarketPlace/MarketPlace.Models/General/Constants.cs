namespace MarketPlace.Models.General
{
    public static class Constants
    {
        #region Area Manager

        public const string C_AppSetting_AreaName = "AreaName";
        public const string C_WebAreaName = "Web";
        public const string C_MobileAreaName = "Mobile";

        #endregion

        #region Internal Settings

        public const string C_SettingsModuleName = "MarketPlace";

        #region LoginModule

        public const string C_Settings_Login_FBUrl = "{{AreaName}}_Login_FBUrl";
        public const string C_Settings_Login_GoogleUrl = "{{AreaName}}_Login_GoogleUrl";

        #endregion

        #region General Settings

        public const string C_Cookie_CookieKey = "SaludGuruCookie";

        public const string C_Settings_Cities = "Cities";

        public const string C_Settings_SaludGuru_FacebookProfile = "SaludGuru_FacebookProfile";

        public const string C_Settings_Profile_CertifiedImage = "Profile_CertifiedImage";

        public const string C_Settings_Profile_WebsiteIcon = "Profile_WebsiteIcon";

        public const string C_Settings_Profile_FacebookIcon = "Profile_FacebookIcon";

        #endregion

        #endregion
    }
}
