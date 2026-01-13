# Data Management .NET Application

This repository contains a **full-stack .NET 8 application** with automated CI/CD using **GitHub Actions** and a **self-hosted runner on a Windows Azure VM**.

---

## 🧱 Architecture Overview

```
GitHub Repository
        |
        | (push / manual trigger)
        v
GitHub Actions Workflow
        |
        | (self-hosted runner)
        v
Azure Windows VM
 ├── Backend API (ASP.NET Core, port 5000)
 └── Frontend Web (ASP.NET Core MVC, port 5001)
```

---

## 📁 Repository Structure

```
.
├── Backend.Api/
│   ├── Backend.Api.csproj
│   └── Program.cs
│
├── Frontend.Web/
│   ├── Frontend.Web.csproj
│   ├── Program.cs
│   └── Controllers/
│
└── .github/
    └── workflows/
        └── build-data-management.yml
```

---

## ✅ Prerequisites

### Azure VM Requirements
- Windows Server 2019 / 2022 (or Windows 10/11)
- Minimum specifications:
  - 2 vCPU
  - 4 GB RAM
  - 20+ GB disk space

### Software Required on VM
Install the following **manually** on the VM:

1. **.NET 8 SDK**  
   https://dotnet.microsoft.com/download/dotnet/8.0

2. **Git**  
   https://git-scm.com/download/win

3. **PowerShell** (preinstalled on Windows)

Verify installation:
```powershell
dotnet --version
git --version
```

---

## 🤖 Self-Hosted GitHub Runner Setup (Windows VM)

> **Important Notes**
> - Replace `username` with your **GitHub username**
> - This is **ONLY** for **runner registration**
> - It is **NOT** used for cloning the repository

---

### 1️⃣ Open Runner Setup Page

Navigate to:

```
https://github.com/username/data-management-dotnet-app/settings/actions/runners/new
```

Select:
- Operating System: **Windows**
- Architecture: **x64**

---

### 2️⃣ Create Runner Directory on VM

```powershell
mkdir C:\actions-runner
cd C:\actions-runner
```

---

### 3️⃣ Download GitHub Actions Runner

```powershell
Invoke-WebRequest -Uri https://github.com/actions/runner/releases/download/v2.330.0/actions-runner-win-x64-2.330.0.zip -OutFile actions-runner.zip
Expand-Archive actions-runner.zip .
```

---

### 4️⃣ Configure the Runner

```powershell
.\config.cmd --url https://github.com/username/data-management-dotnet-app --token <RUNNER_TOKEN>
```

During setup:
- Runner name: `azure-vm-runner`
- Work folder: `_work`
- Run as service: **Yes**
- Service account: **NT AUTHORITY\SYSTEM**

---

### 5️⃣ Start Runner Service

```powershell
Start-Service actions.runner.*
```

Verify runner shows **Idle** in:
```
Repository → Settings → Actions → Runners
```

---

## 🚀 CI/CD Workflow

Workflow file:
```
.github/workflows/build-data-management.yml
```

### What the Workflow Does
- Restore backend & frontend
- Build both in Release mode
- Stop running apps safely (port-based)
- Copy new DLLs
- Restart backend and frontend

---

## 📦 Deployment Paths (on VM)

```
C:\apps\data-management\backend
C:\apps\data-management\frontend
```

---

## 🌐 Application URLs

| Component | URL |
|---------|----|
| Backend API | http://localhost:5000 |
| Swagger UI | http://localhost:5000/swagger |
| Frontend Web UI | http://localhost:5001 |

---

## ▶️ Manual Run (Optional)

```powershell
dotnet C:\apps\data-management\backend\Backend.Api.dll --urls=http://localhost:5000
dotnet C:\apps\data-management\frontend\Frontend.Web.dll --urls=http://localhost:5001
```

---

## 🔧 Common Issues & Fixes

### Port Already in Use
```powershell
netstat -ano | findstr :5000
taskkill /PID <PID> /F
```

### DLL Locked During Deployment
✔ Automatically handled by the workflow.

---

## 🔐 Security Notes

- No Azure credentials required
- No secrets stored in GitHub
- Runner runs fully inside your VM

---

## 📌 Summary

- ✔ Full CI/CD on Windows Azure VM
- ✔ Self-hosted GitHub Actions runner
- ✔ Backend & Frontend automated deployment
- ✔ Reusable for any GitHub username
