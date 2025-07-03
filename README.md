**Clickly**

[![.NET](https://img.shields.io/badge/.NET-9.0-blue)](https://dotnet.microsoft.com/) [![EntityFramework](https://img.shields.io/badge/EF%20Core-9.0-lightgrey)](https://docs.microsoft.com/ef/core/) [![TailwindCSS](https://img.shields.io/badge/TailwindCSS-3.0-teal)](https://tailwindcss.com/)

A modern, extensible social media web application built on C# and ASP.NET MVC, following an N-Tier architecture and leveraging Entity Framework Core for data access. The frontend is styled with Tailwind CSS and enhanced with JavaScript.

---

## 🚀 Features

- **Stories & Posts**: Create and view ephemeral stories and persistent posts.
- **Likes, Shares & Favorites**: Engage with content by liking, sharing, or favoriting posts.
- **Friend Requests Management**: Send, cancel, ignore, and approve friend requests.
- **Trending Section**: Discover the most popular hashtags in real time.
- **Real-Time Updates**: Powered by SignalR for live notifications and chat.
- **Admins/Modders dashboard**: Checking reported users post and reject or approve them.

---

## 🛠 Technology Stack

| Layer            | Technology                              |
|------------------|-----------------------------------------|
| Presentation     | ASP.NET MVC, Razor Views, Tailwind CSS  |
| Business Logic   | C#, .NET 9.0, Dependency Injection     |
| Data Access      | Entity Framework Core, SQL Server/MySQL |
| Client Scripting | JavaScript (ES6+) |
| Real-time Communication | SignalR |



## 📐 Architecture

This project follows an **N-Tier** architecture:

1. **Presentation Layer**: Handles HTTP requests, renders views, and interacts with client-side scripts.
2. **Business Layer**: Contains application logic, services, and validation.
3. **Data Layer**: Manages database context and migrations with Entity Framework Core.

Each layer is organized in its own project namespace:
```
- Clickly.Presentation (ASP.NET MVC)
- Clickly.Services    (Services)
- Clickly.ServiceContracts    (Interfaces)
- Clickly.Data        (DbContext, Migrations)
```

---

## 💾 Prerequisites

- [.NET 9.0](https://dotnet.microsoft.com/download)
- [Node.js 14+](https://nodejs.org/) (for Tailwind CLI)
- SQL Server, MySQL, or an alternative supported by EF Core

---

## ⚙️ Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/sina-ashtari/Clickly.git
   cd Clickly
   ```

2. **Configure database connection**
   - Open `appsettings.json` in the Presentation project.
   - Set your connection string under `ConnectionStrings:DefaultConnection`.

3. **Apply migrations & seed data**
   ```bash
   cd Clickly.Data
   dotnet ef database update

4. **Configure Google and Github oauth clientId and clientSecret **
   - Open `appsettings.json` in the Presentation project.
   - Set your clientId under `Auth:Google/Github:ClientId`.
   - Set your clientSecret under `Auth:Google/Github:clientSecret`.

5. **Install Tailwind CSS**
   ```bash
   npm install -D tailwindcss postcss autoprefixer
   npx tailwindcss init
   ```
   Ensure your `tailwind.config.js` points to Razor views and JS files.

6 **Build & run**
   ```bash
   cd Clickly
   dotnet run
   ```
   Navigate to `https://localhost:5001` in your browser.


## 🖋️ Design 

![authentication](https://github.com/user-attachments/assets/ba4a60e6-b84c-49dc-b041-3b5d275b1765)
![feed or home page](https://github.com/user-attachments/assets/0ba59d9b-ba39-4399-a6bb-6e0c8364a6da)
![friends screen](https://github.com/user-attachments/assets/03da168b-83a7-4f34-8bb4-337bf34a8710)


## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repo.
2. Create a feature branch: `git checkout -b feature/AwesomeFeature`.
3. Commit your changes: `git commit -m 'Add some feature'`.
4. Push to the branch: `git push origin feature/AwesomeFeature`.
5. Open a Pull Request.

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for more details on code standards and branch naming.

## 📬 Contact

Sina – [sinaashtari004@gmail.com](mailto:sinaashtari004@gmail.com)

Project Link: [https://github.com/sina-ashtari/Clickly](https://github.com/sina-ashtari/Clickly)

