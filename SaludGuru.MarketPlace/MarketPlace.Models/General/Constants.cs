namespace MarketPlace.Models.General
{
    public static class Constants
    {
        #region Area Manager

        public const string C_AppSetting_AreaName = "AreaName";
        public const string C_WebAreaName = "Web";
        public const string C_MobileAreaName = "Mobile";

        #endregion

        #region RouteNames

        public const string C_RouteValue_IsNoFollow = "IsNoFollow";
        public const string C_RouteValue_IsNoIndex = "IsNoIndex";
        public const string C_RouteValue_IsRedirect = "IsRedirect";
        public const string C_RouteValue_IsCanonical = "IsCanonical";

        public const string C_Route_Default = "Default";

        public const string C_Route_SearchQuery_Default = "SearchQuery_Default";

        public const string C_Route_SearchCategory_City = "SearchCategory_City";
        public const string C_Route_SearchCategory_InsuranceCity = "SearchCategory_InsuranceCity";
        public const string C_Route_SearchCategory_SpecialtyCity = "SearchCategory_SpecialtyCity";
        public const string C_Route_SearchCategory_SpecialtyInsuranceCity = "SearchCategory_SpecialtyInsuranceCity";
        public const string C_Route_SearchCategory_TreatmentCity = "SearchCategory_TreatmentCity";
        public const string C_Route_SearchCategory_TreatmentInsuranceCity = "SearchCategory_TreatmentInsuranceCity";

        public const string C_Route_Error_NotFound = "Error_NotFound";
        public const string C_Route_Profile_Default = "Profile_Default";

        #endregion

        #region Internal Settings

        public const string C_SettingsModuleName = "MarketPlace";

        #region LoginModule

        public const string C_Settings_Login_FBUrl = "{{AreaName}}_Login_FBUrl";
        public const string C_Settings_Login_GoogleUrl = "{{AreaName}}_Login_GoogleUrl";

        #endregion

        #region General Settings

        public const string C_Cookie_CookieKey = "SaludGuruCookie";

        #region Google maps

        public const string C_Settings_GoogleMaps_ApiKey = "GoogleMaps_ApiKey";

        public const string C_Settings_GoogleMaps_Sensor = "GoogleMaps_Sensor";

        #endregion

        public const string C_Settings_Cities = "Cities";

        public const string C_Settings_SaludGuru_FacebookProfile = "SaludGuru_FacebookProfile";

        public const string C_Settings_Profile_CertifiedImage = "Profile_CertifiedImage";

        public const string C_Settings_Profile_WebsiteIcon = "Profile_WebsiteIcon";

        public const string C_Settings_Profile_FacebookIcon = "Profile_FacebookIcon";

        #endregion

        #region Url

        public const string C_Settings_Url_Invalid_Char = "Url_Invalid_Char";

        #endregion

        #endregion
    }
}
