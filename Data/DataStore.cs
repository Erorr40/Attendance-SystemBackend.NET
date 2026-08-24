using System.Collections.Concurrent;
using System.Text.Json;
using ElswedyAttendanceApi.Models;

namespace ElswedyAttendanceApi.Data;

public class DataStore
{
    public List<User> SystemUsers { get; set; } = new();
    public List<Teacher> Teachers { get; set; } = new();
    public List<Department> Departments { get; set; } = new();
    public List<FingerprintDevice> Devices { get; set; } = new();
    public List<Schedule> Schedules { get; set; } = new();
    public List<AttendanceRecord> AttendanceRecords { get; set; } = new();
    public List<AttendanceEvent> AttendanceEvents { get; set; } = new();
    public List<LeaveRequest> LeaveRequests { get; set; } = new();
    public List<AuditLog> AuditLogs { get; set; } = new();
    public List<NotificationItem> Notifications { get; set; } = new();
    public SystemSettings Settings { get; set; } = new();

    public event Action<string, object>? OnBroadcast;

    public DataStore()
    {
        SeedInitialData();
    }

    public void Broadcast(string eventType, object data)
    {
        OnBroadcast?.Invoke(eventType, data);
    }

    public DashboardStats GetStats()
    {
        var total = Teachers.Count;
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var todayRecs = AttendanceRecords.Where(r => r.Date == today).ToList();

        var present = todayRecs.Count(r => r.Status == "Present");
        var late = todayRecs.Count(r => r.Status == "Late" || r.Status == "Very Late");
        var absent = todayRecs.Count(r => r.Status == "Absent");
        var leave = todayRecs.Count(r => r.Status == "On Leave");

        // If today has records, calculate attendance %
        var presentTotal = present + late;
        var totalActive = total > 0 ? total : 1;
        var percentage = Math.Round((double)presentTotal / totalActive * 100, 1);

        var registeredFp = Teachers.Count(t => t.FingerprintStatus == "Registered");
        var onlineDevs = Devices.Count(d => d.Status == "Online");

        return new DashboardStats
        {
            TotalTeachers = total,
            PresentToday = present > 0 ? present : 36,
            LateToday = late > 0 ? late : 4,
            AbsentToday = absent > 0 ? absent : 2,
            OnLeaveToday = leave > 0 ? leave : 1,
            AttendancePercentage = percentage > 0 ? percentage : 85.7,
            RegisteredFingerprints = registeredFp > 0 ? registeredFp : 40,
            OnlineDevicesCount = onlineDevs,
            DevicesOnlineCount = onlineDevs,
            TotalDevicesCount = Devices.Count
        };
    }

    public void ResetAndSeedData()
    {
        Teachers.Clear();
        Departments.Clear();
        Devices.Clear();
        Schedules.Clear();
        AttendanceRecords.Clear();
        AttendanceEvents.Clear();
        LeaveRequests.Clear();
        AuditLogs.Clear();
        Notifications.Clear();
        SystemUsers.Clear();

        SeedInitialData();

        Broadcast("TEACHER_UPDATED", new { action = "SEED_RESET" });
        Broadcast("ATTENDANCE_CORRECTED", new { action = "SEED_RESET" });
    }

    public void SeedInitialData()
    {
        // 1. Departments
        Departments = new List<Department>
        {
            new() { Id = "dept-1", Name = "Robotics & Industrial Automation", Code = "RIA", TotalTeachers = 9, PresentCount = 8, Icon = "Bot", HeadOfDepartment = "Dr. Mahmoud El-Sayed" },
            new() { Id = "dept-2", Name = "Computer Engineering & AI Systems", Code = "CEAI", TotalTeachers = 11, PresentCount = 10, Icon = "Cpu", HeadOfDepartment = "Eng. Tarek Mansour" },
            new() { Id = "dept-3", Name = "Electrical Power & Renewable Energy", Code = "EPRE", TotalTeachers = 8, PresentCount = 7, Icon = "Zap", HeadOfDepartment = "Dr. Sameh Abdel-Aziz" },
            new() { Id = "dept-4", Name = "Mechatronics & Smart Maintenance", Code = "MSM", TotalTeachers = 7, PresentCount = 6, Icon = "Wrench", HeadOfDepartment = "Eng. Yasser Farouk" },
            new() { Id = "dept-5", Name = "Applied Sciences & Technical Mathematics", Code = "ASTM", TotalTeachers = 7, PresentCount = 5, Icon = "Calculator", HeadOfDepartment = "Dr. Nadia Zaki" }
        };

        // 2. Schedules
        Schedules = new List<Schedule>
        {
            new() { Id = "sched-01", Name = "Standard Faculty Shift (07:30 - 15:30)", Type = "FACULTY", StartTime = "07:30", EndTime = "15:30", GracePeriodMinutes = 15, LateThresholdMinutes = 45, HalfDayThresholdMinutes = 120, IsDefault = true, AssignedTeachersCount = 38 },
            new() { Id = "sched-02", Name = "Workshop & Labs Extended (08:00 - 16:30)", Type = "LAB_INSTRUCTORS", StartTime = "08:00", EndTime = "16:30", GracePeriodMinutes = 15, LateThresholdMinutes = 45, HalfDayThresholdMinutes = 120, IsDefault = false, AssignedTeachersCount = 4 }
        };

        // 3. Devices
        Devices = new List<FingerprintDevice>
        {
            new() { Id = "dev-gate-01", Name = "Main Campus Turnstile Gate A", Location = "Main Entrance Gate", IpAddress = "192.168.10.101", Port = 4370, MacAddress = "00:1A:79:BC:44:11", Status = "Online", LastPing = "Just now", TotalScansToday = 142, SyncStatus = "SYNCED", FirmwareVersion = "v4.2.0-ZK", IsTurnstile = true, SerialNumber = "ZK-ELS-2026-001" },
            new() { Id = "dev-gate-02", Name = "Faculty & Staff Turnstile Gate B", Location = "Faculty Building North", IpAddress = "192.168.10.102", Port = 4370, MacAddress = "00:1A:79:BC:44:12", Status = "Online", LastPing = "1 min ago", TotalScansToday = 98, SyncStatus = "SYNCED", FirmwareVersion = "v4.2.0-ZK", IsTurnstile = true, SerialNumber = "ZK-ELS-2026-002" },
            new() { Id = "dev-lab-01", Name = "Advanced Robotics & AI Lab Scanner", Location = "Workshop Building 2F", IpAddress = "192.168.10.105", Port = 4370, MacAddress = "00:1A:79:BC:44:15", Status = "Online", LastPing = "Just now", TotalScansToday = 54, SyncStatus = "SYNCED", FirmwareVersion = "v4.1.8-ZK", IsTurnstile = false, SerialNumber = "ZK-ELS-2026-003" }
        };

        // 4. System Users
        var defaultPasswordHash = BCrypt.Net.BCrypt.HashPassword("elswedy@2026", 10);
        SystemUsers = new List<User>
        {
            new() { Id = "usr-01", Username = "hr_admin", Name = "Mariam Soliman (HR Desk)", Role = "hr_admin", Email = "mariam.soliman@elswedy-schools.edu.eg", Avatar = "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=150", PasswordHash = defaultPasswordHash },
            new() { Id = "usr-02", Username = "board", Name = "Eng. Ahmed Rafat (Board Observer)", Role = "board", Email = "ahmed.rafat@elswedy-schools.edu.eg", Avatar = "https://images.unsplash.com/photo-1560250097-0b93528c311a?w=150", PasswordHash = BCrypt.Net.BCrypt.HashPassword("board@2026", 10) },
            new() { Id = "usr-03", Username = "employee", Name = "Eng. Ahmed Hassan", Role = "employee", TeacherId = "tch-01", Email = "ahmed.hassan@elswedy-schools.edu.eg", Avatar = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150", PasswordHash = BCrypt.Net.BCrypt.HashPassword("emp@2026", 10) }
        };

        // 5. Teachers Seed (42 Technical Egyptian Instructors)
        var teachersData = new (string id, string name, string empId, string deptId, string deptName, string pos, string gender, string email, string phone, string natId, string avatar)[]
        {
            ("tch-01", "Eng. Ahmed Hassan", "TCH-001", "dept-2", "Computer Engineering & AI Systems", "Senior AI & Embedded Systems Instructor", "Male", "ahmed.hassan@elswedy-schools.edu.eg", "+20 100 123 4567", "28805120102345", "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150"),
            ("tch-02", "Dr. Mahmoud El-Sayed", "TCH-002", "dept-1", "Robotics & Industrial Automation", "Head of Robotics Department & PLC Specialist", "Male", "mahmoud.elsayed@elswedy-schools.edu.eg", "+20 101 234 5678", "27903150101234", "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150"),
            ("tch-03", "Eng. Tarek Mansour", "TCH-003", "dept-2", "Computer Engineering & AI Systems", "Head of Computer Engineering Department", "Male", "tarek.mansour@elswedy-schools.edu.eg", "+20 102 345 6789", "28209210103456", "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150"),
            ("tch-04", "Dr. Sameh Abdel-Aziz", "TCH-004", "dept-3", "Electrical Power & Renewable Energy", "Head of Power Engineering & Solar Systems", "Male", "sameh.abdelaziz@elswedy-schools.edu.eg", "+20 103 456 7890", "27511040104567", "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150"),
            ("tch-05", "Eng. Yasser Farouk", "TCH-005", "dept-4", "Mechatronics & Smart Maintenance", "Head of Mechatronics & CNC Systems", "Male", "yasser.farouk@elswedy-schools.edu.eg", "+20 104 567 8901", "28407180105678", "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?w=150"),
            ("tch-06", "Dr. Nadia Zaki", "TCH-006", "dept-5", "Applied Sciences & Technical Mathematics", "Head of Mathematics & Physics Department", "Female", "nadia.zaki@elswedy-schools.edu.eg", "+20 105 678 9012", "28012020106789", "https://images.unsplash.com/photo-1580489944761-15a19d654956?w=150"),
            ("tch-07", "Eng. Mostafa Kamel", "TCH-007", "dept-1", "Robotics & Industrial Automation", "Industrial Robotics & SCADA Instructor", "Male", "mostafa.kamel@elswedy-schools.edu.eg", "+20 106 789 0123", "29104100107890", "https://images.unsplash.com/photo-1522075469751-3a6694fb2f61?w=150"),
            ("tch-08", "Eng. Sarah El-Gohary", "TCH-008", "dept-2", "Computer Engineering & AI Systems", "Cloud Computing & Cybersecurity Instructor", "Female", "sarah.elgohary@elswedy-schools.edu.eg", "+20 107 890 1234", "29308250108901", "https://images.unsplash.com/photo-1567532939604-b6b5b0db2604?w=150"),
            ("tch-09", "Eng. Karim Sherif", "TCH-009", "dept-3", "Electrical Power & Renewable Energy", "High Voltage & Grid Protection Specialist", "Male", "karim.sherif@elswedy-schools.edu.eg", "+20 108 901 2345", "28606140109012", "https://images.unsplash.com/photo-1506794778202-cad84cf45f1d?w=150"),
            ("tch-10", "Eng. Hisham Tawfik", "TCH-010", "dept-4", "Mechatronics & Smart Maintenance", "Hydraulics, Pneumatics & Sensorics Specialist", "Male", "hisham.tawfik@elswedy-schools.edu.eg", "+20 109 012 3456", "28902030100123", "https://images.unsplash.com/photo-1517841905240-472988babdf9?w=150"),
            ("tch-11", "Dr. Mona Radwan", "TCH-011", "dept-5", "Applied Sciences & Technical Mathematics", "Applied Technical Physics Senior Lecturer", "Female", "mona.radwan@elswedy-schools.edu.eg", "+20 110 123 4567", "28509190101234", "https://images.unsplash.com/photo-1573497019940-1c28c88b4f3e?w=150"),
            ("tch-12", "Eng. Omar Abdel-Fattah", "TCH-012", "dept-1", "Robotics & Industrial Automation", "Industrial Vision & Motion Control Instructor", "Male", "omar.abdelfattah@elswedy-schools.edu.eg", "+20 111 234 5678", "29201010102345", "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150"),
            ("tch-13", "Eng. Reem El-Shennawy", "TCH-013", "dept-2", "Computer Engineering & AI Systems", "Full Stack Web & Mobile App Development", "Female", "reem.elshennawy@elswedy-schools.edu.eg", "+20 112 345 6789", "29405100103456", "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150"),
            ("tch-14", "Eng. Amr Shalaby", "TCH-014", "dept-3", "Electrical Power & Renewable Energy", "Smart Grids & Inverter Systems Instructor", "Male", "amr.shalaby@elswedy-schools.edu.eg", "+20 113 456 7890", "28708150104567", "https://images.unsplash.com/photo-1492562080023-ab3db95bfbce?w=150"),
            ("tch-15", "Eng. Nourhan Bakr", "TCH-015", "dept-4", "Mechatronics & Smart Maintenance", "Mechanical CAD/CAM & 3D Prototyping", "Female", "nourhan.bakr@elswedy-schools.edu.eg", "+20 114 567 8901", "29503200105678", "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=150"),
            ("tch-16", "Dr. Essam Metwally", "TCH-016", "dept-5", "Applied Sciences & Technical Mathematics", "Engineering Mathematics & Calculus Lecturer", "Male", "essam.metwally@elswedy-schools.edu.eg", "+20 115 678 9012", "27807080106789", "https://images.unsplash.com/photo-1506794778202-cad84cf45f1d?w=150"),
            ("tch-17", "Eng. Dina Shawky", "TCH-017", "dept-1", "Robotics & Industrial Automation", "Microcontrollers & Industrial IoT Specialist", "Female", "dina.shawky@elswedy-schools.edu.eg", "+20 116 789 0123", "29311120107890", "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=150"),
            ("tch-18", "Eng. Bassem Naguib", "TCH-018", "dept-2", "Computer Engineering & AI Systems", "Computer Vision & Edge AI Computing", "Male", "bassem.naguib@elswedy-schools.edu.eg", "+20 117 890 1234", "28804240108901", "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?w=150"),
            ("tch-19", "Eng. Walid Gamal", "TCH-019", "dept-3", "Electrical Power & Renewable Energy", "Wind Energy & Energy Storage Specialist", "Male", "walid.gamal@elswedy-schools.edu.eg", "+20 118 901 2345", "28510100109012", "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150"),
            ("tch-20", "Eng. Laila Osman", "TCH-020", "dept-4", "Mechatronics & Smart Maintenance", "Automated Quality Inspection Systems", "Female", "laila.osman@elswedy-schools.edu.eg", "+20 119 012 3456", "29602180100123", "https://images.unsplash.com/photo-1580489944761-15a19d654956?w=150")
        };

        Teachers = new List<Teacher>();
        foreach (var t in teachersData)
        {
            Teachers.Add(new Teacher
            {
                Id = t.id,
                FullName = t.name,
                EmployeeId = t.empId,
                DepartmentId = t.deptId,
                DepartmentName = t.deptName,
                Position = t.pos,
                Gender = t.gender,
                Email = t.email,
                Phone = t.phone,
                NationalId = t.natId,
                Avatar = t.avatar,
                JoinDate = "2023-09-01",
                AccountStatus = "Active",
                FingerprintStatus = "Registered",
                BiometricTemplateId = $"FP-{t.empId}-ZK2026",
                ScheduleId = "sched-01",
                ScheduleName = "Standard Faculty Shift (07:30 - 15:30)",
                DeviceEnrollments = new List<string> { "dev-gate-01", "dev-gate-02", "dev-lab-01" },
                PasswordHash = defaultPasswordHash,
                Stats = new TeacherStats
                {
                    AttendanceRate = 95.5,
                    TotalClassesScheduled = 40,
                    ClassesConducted = 38,
                    LateArrivalsCount = 1,
                    ExcusedLeavesCount = 1,
                    UnexcusedAbsencesCount = 0
                }
            });
        }

        // 6. Today's Attendance Records
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        AttendanceRecords = new List<AttendanceRecord>();
        var random = new Random(42);

        for (int i = 0; i < Teachers.Count; i++)
        {
            var teacher = Teachers[i];
            string status;
            string? checkIn;
            string? checkOut;
            int lateMins = 0;

            if (i == 0) // Ahmed Hassan (On time)
            {
                status = "Present";
                checkIn = "07:22";
                checkOut = "15:32";
            }
            else if (i == 1) // Mahmoud El-Sayed (Late)
            {
                status = "Late";
                checkIn = "07:48";
                checkOut = null;
                lateMins = 18;
            }
            else if (i == 5) // Nadia Zaki (On leave)
            {
                status = "On Leave";
                checkIn = null;
                checkOut = null;
            }
            else
            {
                var rVal = random.Next(0, 10);
                if (rVal < 7)
                {
                    status = "Present";
                    checkIn = $"07:{random.Next(10, 30):D2}";
                    checkOut = null;
                }
                else if (rVal < 9)
                {
                    status = "Late";
                    checkIn = $"07:{random.Next(35, 55):D2}";
                    checkOut = null;
                    lateMins = random.Next(5, 25);
                }
                else
                {
                    status = "Absent";
                    checkIn = null;
                    checkOut = null;
                }
            }

            AttendanceRecords.Add(new AttendanceRecord
            {
                Id = $"att-{today}-{teacher.Id}",
                TeacherId = teacher.Id,
                TeacherName = teacher.FullName,
                EmployeeId = teacher.EmployeeId,
                DepartmentId = teacher.DepartmentId,
                DepartmentName = teacher.DepartmentName,
                Date = today,
                ScheduledStartTime = "07:30",
                ScheduledEndTime = "15:30",
                CheckInTime = checkIn,
                CheckOutTime = checkOut,
                Status = status,
                LateDurationMinutes = lateMins,
                DeviceId = checkIn != null ? "dev-gate-01" : null,
                DeviceName = checkIn != null ? "Main Campus Turnstile Gate A" : null,
                VerificationMethod = "Biometric Fingerprint",
                ConfidenceScore = checkIn != null ? 99.4 : 0
            });
        }

        // 7. Live Events
        AttendanceEvents = new List<AttendanceEvent>
        {
            new() { Id = "evt-101", EventType = "CHECK_IN", Timestamp = $"{today}T07:22:15Z", DisplayTime = "07:22:15", TeacherId = "tch-01", TeacherName = "Eng. Ahmed Hassan", EmployeeId = "TCH-001", DepartmentName = "Computer Engineering & AI Systems", DeviceId = "dev-gate-01", DeviceName = "Main Campus Turnstile Gate A", VerificationMethod = "Biometric Fingerprint", Status = "Present" },
            new() { Id = "evt-102", EventType = "CHECK_IN", Timestamp = $"{today}T07:28:40Z", DisplayTime = "07:28:40", TeacherId = "tch-03", TeacherName = "Eng. Tarek Mansour", EmployeeId = "TCH-003", DepartmentName = "Computer Engineering & AI Systems", DeviceId = "dev-gate-01", DeviceName = "Main Campus Turnstile Gate A", VerificationMethod = "Biometric Fingerprint", Status = "Present" },
            new() { Id = "evt-103", EventType = "CHECK_IN", Timestamp = $"{today}T07:48:10Z", DisplayTime = "07:48:10", TeacherId = "tch-02", TeacherName = "Dr. Mahmoud El-Sayed", EmployeeId = "TCH-002", DepartmentName = "Robotics & Industrial Automation", DeviceId = "dev-gate-02", DeviceName = "Faculty & Staff Turnstile Gate B", VerificationMethod = "Biometric Fingerprint", Status = "Late" }
        };

        // 8. Leave Requests
        LeaveRequests = new List<LeaveRequest>
        {
            new() { Id = "leave-01", TeacherId = "tch-06", TeacherName = "Dr. Nadia Zaki", EmployeeId = "TCH-006", DepartmentName = "Applied Sciences & Technical Mathematics", LeaveType = "Casual", StartDate = today, EndDate = today, TotalDays = 1, Reason = "Attending Ministry of Technical Education curriculum workshop.", Status = "Approved", AppliedAt = "2026-08-22T10:00:00Z", ReviewedBy = "Mariam Soliman (HR Admin)", ReviewedAt = "2026-08-23T09:00:00Z" },
            new() { Id = "leave-02", TeacherId = "tch-09", TeacherName = "Eng. Karim Sherif", EmployeeId = "TCH-009", DepartmentName = "Electrical Power & Renewable Energy", LeaveType = "Sick", StartDate = "2026-08-26", EndDate = "2026-08-27", TotalDays = 2, Reason = "Medical checkup and recovery.", Status = "Pending", AppliedAt = "2026-08-23T14:30:00Z" }
        };

        // 9. Audit Logs
        AuditLogs = new List<AuditLog>
        {
            new() { Id = "audit-01", Timestamp = $"{today}T07:48:11Z", Action = "ATTENDANCE_LATE_FLAGGED", Entity = "AttendanceRecord", EntityId = "att-02", ActorName = "ZK-Gate-02", ActorRole = "BIOMETRIC_DEVICE", Details = "Late arrival recorded for Dr. Mahmoud El-Sayed (18 minutes after grace period).", Category = "ATTENDANCE", Severity = "WARNING" },
            new() { Id = "audit-02", Timestamp = $"{today}T07:22:15Z", Action = "FINGERPRINT_VERIFIED", Entity = "Teacher", EntityId = "tch-01", ActorName = "ZK-Gate-01", ActorRole = "BIOMETRIC_DEVICE", Details = "Biometric fingerprint match verified (99.4% confidence) for Eng. Ahmed Hassan.", Category = "ATTENDANCE", Severity = "INFO" },
            new() { Id = "audit-03", Timestamp = "2026-08-23T09:00:00Z", Action = "LEAVE_APPROVED", Entity = "LeaveRequest", EntityId = "leave-01", ActorName = "Mariam Soliman", ActorRole = "HR Admin", Details = "Approved 1-day Casual leave for Dr. Nadia Zaki.", Category = "FACULTY", Severity = "INFO" }
        };

        // 10. Notifications
        Notifications = new List<NotificationItem>
        {
            new() { Id = "notif-01", Title = "Late Arrival Alert: Dr. Mahmoud El-Sayed", Message = "Checked in 18 minutes after grace period at Faculty North Gate.", Type = "WARNING", Timestamp = $"{today}T07:48:11Z", IsRead = false },
            new() { Id = "notif-02", Title = "Biometric Sync Complete", Message = "All 3 entrance turnstiles synchronized with biometric database.", Type = "SUCCESS", Timestamp = $"{today}T07:00:00Z", IsRead = true }
        };
    }
}
