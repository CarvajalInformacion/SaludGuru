namespace BackOffice.Models.General
{
    public enum enumPrincipalMenu
    {
        Dashboard,
        Administrator,
        Profile,
        Appointment,
        Patient
    }
    public enum enumEditPermision
    {
        Read,
        Write,
    }

    public enum enumCatalog
    {
        Role = 1,
        ProfileType = 2,
        ProfileStatus = 3,
        ProfileInfoType = 4,
        CategoryType = 5,
        CategoryInfoType = 6,
        OfficeInfoType = 7,
        OfficeCategoryInfoType = 8,
        IdentificationType = 9
    }

    public enum enumOfficeMenu
    {
        OfficeInfo,
        Treatments,
        ScheduleAvalilable
    }
}
