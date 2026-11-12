# C# learning projects

Two small .NET 8 projects written while learning C#.

## 🃏 BlackJack — console game

A console Blackjack game against the dealer, refactored into an object-oriented structure:

- `Card`, `Deck` — a 52-card deck, cards are drawn at random and the deck refills when empty
- `Participant` — the player and the dealer (hand, score, Ace counted as 1 or 11)
- `BlackjackEngine` — game rounds: the player hits or stands, the dealer draws until 17, the result is shown and win/loss/tie statistics are kept between rounds

LINQ is used for score calculation and deck handling.

```bash
cd BlackJack
dotnet run
```

## ✅ TaskTracker.Api — REST API

A simple to-do list API built with ASP.NET Core Web API, Entity Framework Core and SQLite.

| Method | Path | Description |
| --- | --- | --- |
| GET | `/api/tasks` | list all tasks |
| POST | `/api/tasks` | create a task |
| PUT | `/api/tasks/{id}` | update the title, description and completion status |
| DELETE | `/api/tasks/{id}` | delete a task |

A task (`Models/TodoTask.cs`) has `Id`, `Title`, `Description`, `IsCompleted` and `CreatedAt`.

```bash
cd TaskTracker.Api
dotnet tool install --global dotnet-ef   # once
dotnet ef database update                # creates tasktracker.db from the migrations
dotnet run
```

In the Development environment Swagger UI is available at `http://localhost:5164/swagger`.
