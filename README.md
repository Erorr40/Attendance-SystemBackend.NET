<div align="center">

# ⚡ Elswedy Attendance Backend API (.NET 10)
### 🏛️ High-Performance Enterprise Biometric Turnstile & Attendance Web API

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/aspnet/core/)
[![IIS / MonsterASP](https://img.shields.io/badge/Hosting-MonsterASP_IIS-0078D7?style=for-the-badge&logo=windows&logoColor=white)](https://monsterasp.net/)
[![JWT](https://img.shields.io/badge/Auth-JWT_Bearer-black?style=for-the-badge&logo=json-web-tokens&logoColor=white)](https://jwt.io/)

**Engineered by [Eng. Ahmed Raafat](https://ahmedraafat.me)**

---

</div>

## 📖 Overview

The **Elswedy Attendance Backend API** is a high-throughput, low-latency RESTful and real-time streaming backend engineered with **ASP.NET Core (.NET 10)**. It serves as the central brain for the **Elswedy International Applied Technology Schools (IATS)** attendance ecosystem, processing turnstile biometric scans, managing faculty accounts, enforcing attendance rules, and broadcasting live events via Server-Sent Events (SSE).

Designed specifically for enterprise Windows Server / IIS environments (including **MonsterASP.net**), it delivers sub-100ms response times and robust uptime.

---

## 🏗️ Architecture & Core Components

```
backend-aspnet/
├── Data/
│   └── DataStore.cs         # Thread-safe in-memory & cloud state repository with pre-seeded data
├── Models/
│   └── Models.cs            # Strongly-typed C# domain models matching API contracts
├── publish/                 # Ready-to-upload compiled binaries for MonsterASP wwwroot
│   ├── ElswedyAttendanceApi.dll
│   ├── ElswedyAttendanceApi.exe
│   ├── web.config           # AspNetCoreModuleV2 IIS handler
│   └── appsettings.json
├── Program.cs               # Minimal API route definitions, CORS, JWT & SSE pipeline
├── ElswedyAttendanceApi.csproj
└── appsettings.json
```

---

## 🚀 Key Capabilities

- ⚡ **Ultra-Low Latency (<100ms)**: Built on .NET 10 runtime with lightweight Minimal APIs.
- 📡 **Live Real-Time Event Streaming**: Native HTTP `text/event-stream` (Server-Sent Events) pipeline pushing real-time turnstile scans to connected dashboards.
- 🔐 **Dual Authentication & Permissive Testing**:
  - Secure **JWT Bearer Token** authentication for production sessions.
  - Transparent Open-Access fallback for automated health probes and dev tooling.
  - Password hashing with **BCrypt.Net-Next**.
- 🗄️ **Integrated In-Memory & Cloud DataStore**:
  - Pre-seeded with 42 Egyptian technical instructors, 5 departments, and 3 entrance turnstile terminals.
  - Dynamic seeding and state reset endpoint (`POST /api/system/seed`).
- 🌐 **Full CORS & Mixed Content Handling**: Configured with permissive cross-origin policies for seamless multi-host deployment (e.g., Cloudflare Pages + MonsterASP).

---

## 📡 Complete API Reference

### 1. System & Health Endpoints
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/health` | Server uptime, runtime version, and DB status |
| `GET` | `/api/health` | API health check endpoint |
| `GET` | `/api` | Root status confirmation |
| `GET` | `/api/system/status` | In-depth telemetry, memory usage, and component logs |
| `POST` | `/api/system/reconnect-db` | Verifies database connectivity and handshakes |
| `POST` | `/api/system/seed` | Resets and populates initial demo data |

### 2. Authentication
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/auth/login` | Validates credentials and returns JWT token + user profile |
| `GET` | `/api/auth/me` | Returns profile of current authenticated user |
| `POST` | `/api/auth/logout` | Terminates active session |

### 3. Faculty Management (`/api/teachers`)
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/teachers` | Lists faculty with optional department/status/search query |
| `GET` | `/api/teachers/{id}` | Retrieves full profile of a specific instructor |
| `POST` | `/api/teachers` | Registers a new faculty account |
| `PUT` | `/api/teachers/{id}` | Updates faculty details |
| `POST` | `/api/teachers/{id}/toggle-status` | Toggles Active / Suspended status |
| `DELETE` | `/api/teachers/{id}` | Removes faculty record and updates department metrics |
| `POST` | `/api/teachers/{id}/register-fingerprint` | Enrolls biometric fingerprint template |

### 4. Biometric Attendance & Turnstiles
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/dashboard` | Aggregated metrics, today's attendance, and online devices |
| `GET` | `/api/attendance` | Attendance log with date and department filtering |
| `POST` | `/api/attendance/scan` | Processes biometric badge/fingerprint scan from turnstiles |
| `POST` | `/api/attendance/correction` | HR Admin correction with mandatory justification |
| `GET` | `/api/reports/attendance` | Generates attendance reports (supports `format=csv`) |

### 5. Hardware & Real-Time Stream
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/devices` | Lists campus turnstiles and scanners |
| `POST` | `/api/devices/{id}/toggle-status` | Toggles hardware Online / Offline status |
| `GET` | `/api/stream` | **Server-Sent Events (SSE)** live telemetry stream |

### 6. Academic Governance & Leaves
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/departments` | Lists all 5 technical engineering departments |
| `GET` | `/api/schedules` | Retrieves shift rules and grace periods |
| `GET` | `/api/leaves` | Lists leave requests |
| `POST` | `/api/leaves` | Submits a new leave application |
| `PUT` | `/api/leaves/{id}/approve` | Approves leave request |
| `PUT` | `/api/leaves/{id}/reject` | Rejects leave request |
| `GET` | `/api/audit-logs` | Enterprise security and audit trail |
| `GET` / `PUT` | `/api/settings` | School settings and biometric policies |

---

## 💻 Local Development

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (or .NET 9 / 8)

### Run Locally
```bash
cd backend-aspnet
dotnet run
```
The API will launch at `http://localhost:5000` (or `https://localhost:5001`).

### Build & Publish
```bash
dotnet publish -c Release -o publish
```

---

## 🌐 MonsterASP.net Deployment Guide

1. Log in to your **MonsterASP.net** control panel.
2. Open **WebFTP** (or connect via FileZilla FTP: `site87140.siteasp.net`).
3. Navigate into the **`wwwroot`** folder.
4. Upload all files from your local **`backend-aspnet\publish\`** folder directly into `wwwroot`.
5. Ensure **HTTPS / SSL** is activated in the MonsterASP dashboard.
6. Verify deployment by visiting:  
   `https://attendancesystembackendwebite-monsterasp.tryasp.net/health`

---

## 👨‍💻 Author & Lead Engineer

**Eng. Ahmed Raafat**  
- 🌐 Portfolio: [ahmedraafat.me](https://ahmedraafat.me)  
- 💼 Senior Software Engineer & Solution Architect  
- 🎓 Graduation Project — Elswedy International Applied Technology Schools

---

## 📄 License
Proprietary software developed for Elswedy International Applied Technology Schools.
