# CyberSafe Awareness Bot

## Overview

CyberSafe Awareness Bot is a C# desktop application built with Windows Presentation Foundation (WPF) to promote cybersecurity awareness through interactive chatbot conversations and practical learning activities.

The application combines an intelligent rule-based chatbot with task management, an interactive cybersecurity quiz, natural language intent recognition and activity tracking.

## Features

### 1. Chatbot
- Cybersecurity advice on passwords, phishing, scams and online privacy.
- Randomised responses and conversational follow-ups.
- User memory and recall.
- Sentiment detection for worried, curious and frustrated users.
- Natural language intent recognition for navigating application features.
- Input validation and error handling.
- Voice greeting on application startup.

### 2. Cybersecurity Task Assistant
- Create cybersecurity-related tasks with titles and descriptions.
- Set optional task reminders.
- View saved tasks.
- Mark tasks as completed.
- Delete tasks.
- MySQL database integration for persistent task storage.

### 3. Cybersecurity Quiz
- 12 cybersecurity knowledge questions.
- Multiple-choice and true/false questions.
- One question displayed at a time.
- Immediate feedback after submitting an answer.
- Explanations of correct answers.
- Automatic score tracking and final percentage.
- Option to restart the quiz.

### 4. Activity Log
- Records chatbot interactions.
- Tracks task creation, completion and deletion.
- Records quiz activities and results.
- Displays the 10 most recent activities.

Activity history is maintained in memory during the application session.

## Technologies

- C#
- .NET 9
- Windows Presentation Foundation (WPF)
- XAML
- MySQL
- MySql.Data
- Git and GitHub
- GitHub Actions for Continuous Integration (CI)

## Project Assets

- `Audio/greeting.wav` — voice greeting played when the application starts.
- `CyberSafeASCII.png` — cybersecurity application branding.
- `Database/database_setup.sql` — MySQL database and table creation script.

## Getting Started

### Prerequisites

1. Visual Studio 2022.
2. .NET 9 SDK.
3. Visual Studio .NET desktop development workload.
4. MySQL Server 8.0 or later.
5. MySQL Workbench (optional, for database management).

### Installation

1. Clone or download the repository.
2. Open `CyberSafeAwarenessBot2.sln` in Visual Studio 2022.
3. Restore the NuGet dependencies.
4. Configure the MySQL database using the script in `Database/database_setup.sql`.
5. Configure the database password using a Windows environment variable.
6. Build and run the application.

### Database Configuration

The application connects to a local MySQL database named `CyberSecurityBotDB`.

The database contains a `cyber_tasks` table for storing cybersecurity tasks, reminders and completion status.

Configure the following Windows environment variable:

`CYBERSAFE_DB_PASSWORD`

Set its value to your local MySQL password. For example, using PowerShell:

```powershell
[Environment]::SetEnvironmentVariable(
    "CYBERSAFE_DB_PASSWORD",
    "YOUR_MYSQL_PASSWORD",
    "User"
)
```

Restart Visual Studio after configuring the variable.

**Security note:** Database credentials are stored outside the source code and should never be committed to the Git repository.

## Continuous Integration

GitHub Actions builds the solution automatically to help verify that changes do not introduce build errors.

View workflow results under the repository's **Actions** tab.

**Successful CI screenshot:** `CI-success.png` (located in the repository root).

## Presentation

**YouTube video:** https://youtu.be/OHb37F3pt2Y

## Academic Purpose

Developed for the Programming 2A Portfolio of Evidence (PROG6221).

The project demonstrates:

- Object-oriented programming using C#.
- Windows desktop GUI development with WPF and XAML.
- Generic collections and data management.
- Delegates and event-driven programming.
- Dynamic chatbot responses.
- User memory and sentiment detection.
- Natural language intent recognition.
- Database integration and CRUD operations.
- Interactive quiz development and score tracking.
- Activity logging.
- Git version control and continuous integration.
