using System.Text.Json.Serialization;

namespace ElswedyAttendanceApi.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = "hr_admin"; // hr_admin, board, teacher, employee
    public string? TeacherId { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
}

public class TeacherStats
{
    public double AttendanceRate { get; set; } = 95.0;
    public int TotalClassesScheduled { get; set; } = 40;
    public int ClassesConducted { get; set; } = 38;
    public int LateArrivalsCount { get; set; } = 1;
    public int ExcusedLeavesCount { get; set; } = 1;
    public int UnexcusedAbsencesCount { get; set; } = 0;
}

public class Teacher
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string DepartmentId { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public string JoinDate { get; set; } = string.Empty;
    public string AccountStatus { get; set; } = "Active"; // Active, Suspended, Inactive
    public string FingerprintStatus { get; set; } = "Registered"; // Registered, Pending, Failed
    public string? BiometricTemplateId { get; set; }
    public string ScheduleId { get; set; } = "sched-01";
    public string ScheduleName { get; set; } = "Standard Faculty (07:30 - 15:30)";
    public List<string> DeviceEnrollments { get; set; } = new();
    public string? PlainPassword { get; set; }
    public string Password { get; set; } = "••••••••••••";
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
    public TeacherStats Stats { get; set; } = new();
}

public class Department
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int TotalTeachers { get; set; }
    public int PresentCount { get; set; }
    public string Icon { get; set; } = "Cpu";
    public string HeadOfDepartment { get; set; } = string.Empty;
}

public class Schedule
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "FACULTY";
    public string StartTime { get; set; } = "07:30";
    public string EndTime { get; set; } = "15:30";
    public int GracePeriodMinutes { get; set; } = 15;
    public int LateThresholdMinutes { get; set; } = 45;
    public int HalfDayThresholdMinutes { get; set; } = 120;
    public List<string> WorkingDays { get; set; } = new() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday" };
    public List<string> WorkDays { get => WorkingDays; set => WorkingDays = value; }
    public bool IsDefault { get; set; } = true;
    public int AssignedTeachersCount { get; set; }
}

public class AttendanceRecord
{
    public string Id { get; set; } = string.Empty;
    public string TeacherId { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string DepartmentId { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string ScheduledStartTime { get; set; } = "07:30";
    public string ScheduledEndTime { get; set; } = "15:30";
    public string? CheckInTime { get; set; }
    public string? CheckOutTime { get; set; }
    public string Status { get; set; } = "Present"; // Present, Late, Very Late, Absent, On Leave, Half Day
    public int LateDurationMinutes { get; set; }
    public bool IsManualCorrection { get; set; }
    public string? CorrectionReason { get; set; }
    public string? CorrectedBy { get; set; }
    public string? CorrectedAt { get; set; }
    public string? DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public string VerificationMethod { get; set; } = "Biometric Fingerprint";
    public double ConfidenceScore { get; set; } = 99.4;
}

public class AttendanceEvent
{
    public string Id { get; set; } = string.Empty;
    public string EventType { get; set; } = "CHECK_IN"; // CHECK_IN, CHECK_OUT, ACCESS_DENIED, ANOMALY
    public string Timestamp { get; set; } = string.Empty;
    public string DisplayTime { get; set; } = string.Empty;
    public string TeacherId { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string VerificationMethod { get; set; } = "Biometric Fingerprint";
    public string Status { get; set; } = "Present";
    public bool IsAnomalous { get; set; }
    public string? AnomalyReason { get; set; }
}

public class FingerprintDevice
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; } = 4370;
    public string MacAddress { get; set; } = string.Empty;
    public string Status { get; set; } = "Online"; // Online, Offline, Warning, Maintenance
    public string LastPing { get; set; } = string.Empty;
    public int TotalScansToday { get; set; }
    public string SyncStatus { get; set; } = "SYNCED";
    public string FirmwareVersion { get; set; } = "v4.1.2-ZK";
    public bool IsTurnstile { get; set; } = true;
    public string SerialNumber { get; set; } = string.Empty;
}

public class LeaveRequest
{
    public string Id { get; set; } = string.Empty;
    public string TeacherId { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string LeaveType { get; set; } = "Casual"; // Casual, Sick, Annual, Emergency, Official Duty
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public int TotalDays { get; set; } = 1;
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    public string AppliedAt { get; set; } = string.Empty;
    public string? ReviewedBy { get; set; }
    public string? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }
}

public class AuditLog
{
    public string Id { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string ActorName { get; set; } = string.Empty;
    public string ActorRole { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string IpAddress { get; set; } = "127.0.0.1";
    public string Category { get; set; } = "SYSTEM"; // AUTH, FACULTY, ATTENDANCE, DEVICE, SECURITY, SYSTEM
    public string Severity { get; set; } = "INFO"; // INFO, WARNING, ALERT
    public object? Metadata { get; set; }
}

public class NotificationItem
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "INFO"; // INFO, SUCCESS, WARNING, ERROR
    public string Timestamp { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public string? Link { get; set; }
}

public class SystemSettings
{
    public string SchoolName { get; set; } = "Elswedy International Applied Technology School";
    public string SchoolCode { get; set; } = "ELSWEDY-IATS-10TH";
    public string AcademicYear { get; set; } = "2025/2026";
    public string TimeZone { get; set; } = "Africa/Cairo (UTC+2)";
    public bool AllowBiometricAttendance { get; set; } = true;
    public bool AutoCalculateLateDeductions { get; set; } = true;
    public int GracePeriodMinutes { get; set; } = 15;
    public bool EnableRealtimeBroadcasting { get; set; } = true;
    public int SsePingIntervalSeconds { get; set; } = 25;
    public bool StrictTurnstileMode { get; set; } = true;
    public string AuditLoggingLevel { get; set; } = "VERBOSE";
    public bool RequireAdminApprovalForLeaves { get; set; } = true;
    public string DatabaseDriver { get; set; } = "MongoDB Atlas / In-Memory Active Engine";
    public bool MultiTenantEnabled { get; set; } = false;
}

public class DashboardStats
{
    public int TotalTeachers { get; set; }
    public int PresentToday { get; set; }
    public int LateToday { get; set; }
    public int AbsentToday { get; set; }
    public int OnLeaveToday { get; set; }
    public double AttendancePercentage { get; set; }
    public int RegisteredFingerprints { get; set; }
    public int OnlineDevicesCount { get; set; }
    public int DevicesOnlineCount { get; set; }
    public int TotalDevicesCount { get; set; }
}

public class LoginRequest
{
    public string UsernameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ScanRequest
{
    public string TeacherId { get; set; } = string.Empty;
    public string? DeviceId { get; set; }
    public string? CustomTimestamp { get; set; }
    public bool? IsOfflineSync { get; set; }
}

public class CorrectionRequest
{
    public string RecordId { get; set; } = string.Empty;
    public string NewStatus { get; set; } = "Present";
    public string? NewCheckIn { get; set; }
    public string? NewCheckOut { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? AdminName { get; set; }
    public string? AdminRole { get; set; }
}

public class RevealPasswordRequest
{
    public string TeacherId { get; set; } = string.Empty;
    public string? RequesterRole { get; set; }
    public string? RequesterName { get; set; }
}

public class ResetPasswordRequest
{
    public string TeacherId { get; set; } = string.Empty;
    public string? NewPassword { get; set; }
    public string? RequesterRole { get; set; }
    public string? RequesterName { get; set; }
}

