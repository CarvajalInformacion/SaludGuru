namespace BackOffice.Models.General
{
    public enum enumMenuPrincipal
    {
        Dashboard,
        Administrator,
        Profile,
        Appointment,
        Patient
    }

    public enum enumMenuSecundary
    {
        None,

        Ad_Insurance,
        Ad_Profile,
        Ad_Specialty,
        Ad_Treatment,

        Ap_Day,
        Ap_Week,
        Ap_Month,
        Ap_List,
        Ap_ShceduleAvailable
    }

    public enum enumMenuProfile
    {
        ProfileInfo,
        ProfileImages,
        Autorization,
        Office,
        Specialty,
        Insurance,
        Treatment,
        Comunicactions,
        RelatedProfiles
    }

    public enum enumMenuOffice
    {
        OfficeInfo,
        Treatments,
        ScheduleAvalilable
    }

    public enum enumMenuComunications
    {
        Messaging,
        Reminders
    }

    public enum enumMenuPatient
    {
        PatientInfo,
        Appointment,
        PatientNotes
    }

    public enum enumEditPermision
    {
        Read,
        Write,
    }

    public enum enumCatalog
    {
        Role = 1,
        Admin = 101,
        ProfileType = 2,
        ProfileStatus = 3,
        ProfileInfoType = 4,
        CategoryType = 5,
        CategoryInfoType = 6,
        OfficeInfoType = 7,
        OfficeCategoryInfoType = 8,
        IdentificationType = 9
    }

    public enum enumAppointmentType
    {
        Day,
        Week,
        List,
        Month,
        Detail,
    }
}
