namespace SaludGuru.Notifications.Models
{
    public enum enumNotificationStatus
    {
        Leida = 1301,
        No_Leida = 1302
    }

    public enum enumNotificationType
    {
        CreatedAppointment = 1501,
        CancelAppointment = 1502,
        NewPatient = 1503,
        ConfirmAppointment = 1504,
        ReminderAppointment = 1505,
        ReminderNextAppointment = 1506,
        Survey = 1507,
        ModifyAppointment = 1508
    }
}
