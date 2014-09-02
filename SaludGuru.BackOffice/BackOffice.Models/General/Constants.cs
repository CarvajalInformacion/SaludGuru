namespace BackOffice.Models.General
{
    public static class Constants
    {
        #region Area Manager
        public const string C_AppSetting_AreaName = "AreaName";
        public const string C_WebAreaName = "Web";
        public const string C_MobileAreaName = "Mobile";
        #endregion

        #region Internal Settings

        public const string C_SettingsModuleName = "BackOffice";

        #region ViewData

        public const string C_ViewData_UserNotAutorizedText = "UserNotAutorizedText";

        #endregion

        #region LoginModule

        public const string C_Settings_Login_FBUrl = "{{AreaName}}_Login_FBUrl";
        public const string C_Settings_Login_GoogleUrl = "{{AreaName}}_Login_GoogleUrl";
        public const string C_Settings_Login_WindowsLiveUrl = "{{AreaName}}_Login_WindowsLiveUrl";

        public const string C_Settings_Login_UserNotAutorized = "Login_UserNotAutorized";

        #endregion

        #region Menu

        public const string C_Settings_PrincipalMenu = "PrincipalMenu_{{RoleId}}";
        public const string C_Settings_SecundaryMenu = "SecundaryMenu_{{MenuPrincipal}}";

        #endregion

        #region Profile Genera

        public const string C_Settings_RemindersType = "Profile_RemindersType";

        public const string C_Settings_ComunicationType = "Profile_ComuicationType";

        public const string C_Settings_Georef_LatitudeMin = "Georef_LatitudeMin";

        public const string C_Settings_Georef_LatitudeMax = "Georef_LatitudeMax";

        public const string C_Settings_Georef_LongitudeMin = "Georef_LongitudeMin";

        public const string C_Settings_Georef_LongitudeMax = "Georef_LongitudeMax";

        public const string C_Settings_Profile_MarketPlaceUrl = "Profile_MarketPlaceUrl";

        public const string C_Settings_Url_Invalid_Char = "Url_Invalid_Char";

        #endregion

        #region Profile Images

        public const string C_Settings_ProfileImage_TempDirectory = "ProfileImage_TempDirectory";

        public const string C_Settings_ProfileImage_Man = "ProfileImage_Man";
        public const string C_Settings_ProfileImage_Woman = "ProfileImage_Woman";
        public const string C_Settings_ProfileImage_Admin = "ProfileImage_Admin";

        #endregion

        #region Patient Images

        public const string C_Settings_PatientImage_Man = "PatientImage_Man";
        public const string C_Settings_PatientImage_Woman = "PatientImage_Woman";

        #endregion

        #region Appointment Settings

        public const string C_Settings_Appointment_ImageEmpty = "Appointment_ImageEmpty";
        public const string C_Settings_Appointment_ImageGroup = "Appointment_ImageGroup";
        public const string C_Settings_Appointment_TitleTemplate = "Appointment_TitleTemplate_{{StatusId}}";
        public const string C_Settings_Appointment_TitleTemplate_Month = "Appointment_TitleTemplate_Month";
        public const string C_Settings_Appointment_StatusName_New = "Appointment_StatusName_New";
        public const string C_Settings_Appointment_StatusName = "Appointment_StatusName_{{StatusId}}";

        #endregion

        #endregion
    }
}

