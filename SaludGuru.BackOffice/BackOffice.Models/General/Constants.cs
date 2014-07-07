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

        #endregion

        #region Appointment Settings

        public const string C_Settings_AppointmentImageEmpty = "AppointmentImageEmpty";
        public const string C_Settings_AppointmentImageGroup = "AppointmentImageGroup";

        public const string C_Settings_AppointmentTitleTemplate = "AppointmentTitleTemplate_{{StatusId}}";

        #endregion

        #endregion
    }
}

