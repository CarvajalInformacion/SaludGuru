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

        #region LoginModule

        public const string C_Settings_Login_FBUrl = "{{AreaName}}_Login_FBUrl";
        public const string C_Settings_Login_GoogleUrl = "{{AreaName}}_Login_GoogleUrl";

        #endregion

        #region Menu

        public const string C_Settings_PrincipalMenu = "PrincipalMenu_{{RoleId}}";
        public const string C_Settings_SecundaryMenu = "SecundaryMenu_{{MenuPrincipal}}";

        #endregion

        #region Profile Genera

        public const string C_Settings_RemindersType = "Profile_RemindersType";

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
        #endregion

        #endregion
    }
}

