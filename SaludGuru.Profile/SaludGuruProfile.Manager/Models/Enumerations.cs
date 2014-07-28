namespace SaludGuruProfile.Manager.Models
{
    public enum enumCategoryType
    {
        Insurance = 501,
        Specialty = 502,
        Treatment = 503
    }

    public enum enumCategoryInfoType
    {
        Keyword = 601,
        DurationTime = 602,
    }

    public enum enumProfileType
    {
        Consultorio = 201,
        Doctor = 202,
    }

    public enum enumProfileStatus
    {
        Free = 301,
        Basic = 302,
        Pay = 303,
        NotShow = 304
    }

    public enum enumProfileInfoType
    {
        IsCertified = 401,
        ProfileText = 402,
        Education = 403,
        Certification = 404,
        Email = 405,
        Website = 406,
        Gender = 407,
        FacebookProfile = 408,
        ImageProfileSmall = 409,
        ImageGeneral = 410,
        IdentificationType = 411,
        IdentificationNumber = 412,
        AsignacionCita = 413,
        CancelacionCita = 414,
        EncuestaSatisfaccion = 415,
        ModificacionCita = 416,
        RecordatorioCita = 417,
        RecordatorioProxCita = 418,

        ImageProfileLarge = 420,
        ImageProfileSmallOriginal = 421,
        ImageProfileLargeOriginal = 422,
        ImageGeneralOriginal = 423,
    }

    public enum enumOfficeInfoType
    {
        Address = 701,
        Telephone = 702,
        Geolocation = 703,
        SlotMinutes = 704,
    }

    public enum enumOfficeCategoryInfoType
    {
        DurationTime = 801,
        BeforeCare = 802,
        AfterCare = 803,
        IsDefault = 804,
    }

    public enum enumIdentificationType
    {
        Cedula = 901,
        Nit = 902
    }

    public enum enumMessageType
    {
        Email = 101,
        Sms = 102,
        GuruNotification = 103
    }

    public enum enumImageType
    {
        ProfileSmall,
        ProfileLarge,
        ProfileGeneral
    }
}
