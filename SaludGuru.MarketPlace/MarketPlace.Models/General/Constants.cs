﻿namespace MarketPlace.Models.General
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
        public const string C_RouteValue_IsQuery = "IsQuery";

        public const string C_Route_Default = "Default";

        public const string C_Route_Home = "Home";

        public const string C_Route_LegalTerms = "LegalTerms";
        public const string C_Route_ConditionsAndRestrictions = "ConditionsAndRestrictions";
        public const string C_Route_FAQ = "FAQ";
        public const string C_Route_Contact = "Contact";


        public const string C_Route_SearchQuery_Default = "SearchQuery_Default";
        public const string C_Route_SearchQuery_CityAll = "SearchQuery_CityAll";

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
        public const string C_Settings_Login_WindowsLiveUrl = "{{AreaName}}_Login_WindowsLiveUrl";

        #endregion

        #region General Settings

        public const string C_Cookie_CookieKey = "SaludGuruCookie";

        #region Google maps

        public const string C_Settings_GoogleMaps_ApiKey = "GoogleMaps_ApiKey";

        public const string C_Settings_GoogleMaps_Sensor = "GoogleMaps_Sensor";

        #endregion

        public const string C_Settings_City_Cities = "City_Cities";

        public const string C_Settings_City_Default = "City_Default";

        public const string C_Settings_City_Indicative = "City_Indicative";

        public const string C_Settings_SaludGuru_FacebookProfile = "SaludGuru_FacebookProfile";

        public const string C_Settings_Profile_CertifiedImage = "Profile_CertifiedImage";

        public const string C_Settings_Profile_WebsiteIcon = "Profile_WebsiteIcon";

        public const string C_Settings_Profile_FacebookIcon = "Profile_FacebookIcon";

        public const string C_Settings_SearchPage_RowCount = "SearchPage_RowCount_{AreaName}";

        public const string C_Settings_Profile_SmallImageDefault = "Profile_SmallImageDefault";

        public const string C_Settings_HomeImage_Contact = "HomeImage_Contact";

        #endregion

        #region Url

        public const string C_Settings_Url_Invalid_Char = "Url_Invalid_Char";

        public const string C_Settings_UrlHttpExceptions = "UrlHttpExceptions";

        public const string C_Settings_UrlBackOffice = "UrlBackOffice";

        public const string C_Settings_Url_MP_Desktop = "Url_MP_Desktop";

        public const string C_Settings_Url_MP_Mobile = "Url_MP_Mobile";

        #endregion

        #region Home Image

        public const string C_Settings_HomeImage_Baseurl = "HomeImage_Baseurl";

        public const string C_Settings_HomeImage_CountMin = "HomeImage_CountMin";

        public const string C_Settings_HomeImage_CountMax = "HomeImage_CountMax";

        #endregion

        #region SEO

        public const string C_Settings_SEO_Home_Title = "SEO_Home_Title_{AreaName}";

        public const string C_Settings_SEO_Home_Description = "SEO_Home_Description_{AreaName}";

        public const string C_Settings_SEO_Home_Keywords = "SEO_Home_Keywords_{AreaName}";

        public const string C_Settings_SEO_Search_Title = "SEO_Search_Title_{AreaName}";

        public const string C_Settings_SEO_Search_Description = "SEO_Search_Description_{AreaName}";

        public const string C_Settings_SEO_Search_Keywords = "SEO_Search_Keywords_{AreaName}";

        public const string C_Settings_SEO_Profile_Title = "SEO_Profile_Title_{AreaName}";

        public const string C_Settings_SEO_Profile_Description = "SEO_Profile_Description_{AreaName}";

        public const string C_Settings_SEO_Profile_Keywords = "SEO_Profile_Keywords_{AreaName}";


        #endregion

        #region Mobile

        public const string C_Settings_Mobile_Devices = "Mobile_Devices";

        #endregion

        #endregion
    }
}
