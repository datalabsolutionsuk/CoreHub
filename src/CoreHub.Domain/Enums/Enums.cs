namespace CoreHub.Domain.Enums;

public enum UserRole
{
    Admin = 1,
    Manager = 2,
    Practitioner = 3,
    ReadOnly = 4
}

public enum ClientStatus
{
    Open = 1,
    Closed = 2,
    WaitingList = 3,
    Discharged = 4
}

public enum Gender
{
    Male = 1,
    Female = 2,
    NonBinary = 3,
    PreferNotToSay = 4,
    Other = 5
}

public enum SessionType
{
    Assessment = 1,
    Treatment = 2,
    Review = 3,
    Discharge = 4,
    DNA = 5
}

public enum FlagType
{
    Risk = 1,
    OffTrack = 2,
    NeedClosing = 3,
    Custom = 4,
    HighRisk = 5,
    DataQuality = 6
}

public enum AppointmentStatus
{
    Booked = 1,
    Confirmed = 2,
    Cancelled = 3,
    DNA = 4,
    Completed = 5
}

public enum NoteCategory
{
    Episode = 1,
    Admin = 2,
    Risk = 3,
    Clinical = 4,
    Supervision = 5
}

public enum FormMode
{
    ClientFacing = 1,
    PractitionerAssisted = 2
}

public enum MeasureScaleType
{
    Likert0To4 = 1,
    Likert1To5 = 2,
    YesNo = 3,
    Numeric = 4
}

public enum DataQualityScore
{
    None = 0,
    Poor = 1,
    Fair = 2,
    Good = 3,
    VeryGood = 4,
    Excellent = 5
}

public enum DischargeReason
{
    Completed = 1,
    DroppedOut = 2,
    Referred = 3,
    MovedAway = 4,
    NotSuitable = 5,
    Other = 6
}

public enum ReminderType
{
    Appointment = 1,
    FormDue = 2,
    ReviewDue = 3,
    RiskReview = 4
}

public enum LetterStatus
{
    Draft = 1,
    Generated = 2,
    Sent = 3
}

public enum AuditAction
{
    Create = 1,
    Update = 2,
    Delete = 3,
    View = 4,
    Export = 5
}
