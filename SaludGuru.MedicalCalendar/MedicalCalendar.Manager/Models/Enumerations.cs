namespace MedicalCalendar.Manager.Models
{
    public enum enumPatientInfoType
    {
        Email = 1001,
        Birthday = 1002,
        Gender = 1003,
        Telephone = 1004,
        IdentificationType = 1005,
        IdentificationNumber = 1006,
        Mobile = 1007,
        ProfileImage = 1008,
        Insurance = 1009,
        MedicalPlan = 1010,
        DoctorNotes = 1011,
        Responsable = 1012,
        SendSMS = 1013,
        SendEmail = 1014,
        IsMarketPlaceUser = 1015,
    }

    public enum enumAppointmentInfoType
    {
        Category = 1101,
        AfterCare = 1103,
        BeforeCare = 1104,
        Note = 1105,
        AppointmentNote = 1106
    }

    public enum enumAppointmentStatus
    {
        New = 1201,
        Confirmed = 1202,
        Canceled = 1203,
        PendientAsistance = 1204,
        Attendance = 1205,
        NotAttendance = 1206,
        BlockCalendar = 1207,
    }

    public enum enumSpecialDayType
    {
        Holiday = 1,
        NotAvailable = 2,
    }
}
