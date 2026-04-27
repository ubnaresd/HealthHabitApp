# 🌿 Health Habit App

> A cross-platform mobile app that helps you build healthy habits, stay consistent, and track your progress - all in one place.

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation & Running](#installation--running)
- [Contributors](#contributors)

---

## About the Project

### The Problem

Many people struggle to maintain consistent healthy habits in their daily lives. Busy schedules, forgetfulness, and low motivation make it easy to fall off track — and that inconsistency adds up over time, negatively impacting personal health and wellness goals.

### The Solution

**Health Habit App** is a cross-platform mobile application built with **.NET MAUI** and the **MVVM** architecture pattern. It empowers users to:

- 🥤 **Set up health habits** — hydration, supplements, exercise, and more
- 🔔 **Receive timely reminders** with motivational messages to keep you on track
- 📈 **Track streaks and progress** over time to stay motivated

All habit data and history is stored **locally using SQLite**, so your data persists between sessions — no account or internet connection required.

---

## Features

| Feature | Description |
|--------|-------------|
| **Habit Creation** | Create custom habits and assign them to a recurring schedule |
| **Reminders & Notifications** | Get notified at the right time with motivational messages |
| **Streak Tracking** | Visualize your progress and maintain streaks to build consistency |
| **Local Data Persistence** | All data stored on-device with SQLite — no cloud dependency |
| **Cross-Platform** | Runs on Windows, Android, iOS, and macOS via .NET MAUI |

---

## Tech Stack

- **Framework:** [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) (Multi-platform App UI)
- **Language:** C# / .NET 8
- **Architecture:** MVVM (Model-View-ViewModel)
- **Database:** SQLite (local on-device storage)
- **IDE:** Visual Studio 2022

---

## Getting Started

### Prerequisites

Make sure you have the following installed before building the project:

- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** with the **.NET Multi-platform App UI development** workload enabled
- **[.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)**

> **Tip:** When installing Visual Studio, you can enable the .NET MAUI workload under *Workloads → Mobile & Gaming → .NET Multi-platform App UI development*.

---

### Installation & Running

1. **Download the source code**

   - Click the green **`<> Code`** button at the top of this repository and select **Download ZIP**, or clone the repo:
     ```bash
     git clone https://github.com/your-username/health-habit-app.git
     ```
   - If you downloaded the ZIP, extract it to a folder of your choice.

2. **Open the project in Visual Studio 2022**

   - Navigate to the extracted folder and open the **`.sln`** file (the Visual Studio solution file) by double-clicking it, or by launching Visual Studio and using *File → Open → Project/Solution*.

3. **Select your target platform**

   - At the top of the Visual Studio window, locate the green **▶ Play** button.
   - If it reads **`Windows Machine`**, you're ready to go — just click it to build and run.
   - If it reads **`Start`** or shows a different platform, click the small **dropdown arrow** (▾) next to the button and select your desired target platform (e.g., *Windows Machine*, *Android Emulator*, etc.).

4. **Build & Run**

   - Click the green **▶** button to build and launch the application. Visual Studio will compile the project and deploy it to your selected platform.

---

## Contributors

| Name | Role |
|------|------|
| **Shrutika Ubnare** | Core app development |
| **Max Hicks** | UI/UX & feature development |
| **Jacob Dice** | Backend & data layer |
| **Olamilekan Esan** | Integration & testing |
