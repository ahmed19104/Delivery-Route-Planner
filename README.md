# Delivery Route Planner

A **.NET 9 console application** that groups delivery requests into vehicle trips. It respects a weight limit per trip, handles urgent deliveries first, and reports how well each trip uses the vehicle's capacity.

Built for a technical challenge (2026).

## Table of contents

- [Features](#features)
- [How it works](#how-it-works)
- [Example](#example)
- [Project structure](#project-structure)
- [Getting started](#getting-started)
- [Input format](#input-format)
- [Design decisions and trade-offs](#design-decisions-and-trade-offs)
- [Scaling to 1,000,000+ requests](#scaling-to-1000000-requests)
- [Future improvements](#future-improvements)

## Features

- **Capacity limit:** each trip carries at most **10.0 kg**.
- **Priority first:** lower priority number means more urgent, so urgent deliveries go out first.
- **Area grouping:** inside the same priority, deliveries are ordered by area so nearby drops sit together.
- **Edge-case handling:** deliveries over 10 kg or with zero or negative weight are rejected and listed separately. A missing file and an empty file print a clear message instead of crashing.
- **Efficiency metric (extension):** `Efficiency % = total trip weight / 10.0 * 100` for every trip.

## How it works

1. Read `deliveries.json` and deserialize it into `Delivery` records (case-insensitive property names).
2. **Validate:** split deliveries into valid and rejected (`weight > 10` or `weight <= 0`).
3. **Sort** valid deliveries by `Priority`, then by `Area`.
4. **Pack greedily:** add deliveries to the current trip. When the next one would push the trip over 10 kg, close the trip and start a new one.
5. Print every trip (id, total weight, efficiency, deliveries) and the list of rejected deliveries.

## Example

Input (`deliveries.json`):

| Id | Area | Priority | Weight (kg) |
| --- | --- | --- | --- |
| 1 | Nasr City | 2 | 4.5 |
| 2 | Maadi | 1 | 2.0 |
| 3 | Nasr City | 3 | 1.2 |
| 4 | Zamalek | 1 | 7.0 |
| 5 | Maadi | 2 | 3.5 |
| 6 | Giza | 1 | 12.0 |

Result with the planner's rules:

| Trip | Deliveries | Total weight | Efficiency |
| --- | --- | --- | --- |
| 1 | #2 (Maadi), #4 (Zamalek) | 9.0 kg | 90.0% |
| 2 | #5 (Maadi), #1 (Nasr City), #3 (Nasr City) | 9.2 kg | 92.0% |
| Rejected | #6 (Giza, 12 kg, over capacity) | - | - |

## Project structure

```
Delivery Route Planner/
├── Delivery Route Planner.sln
└── Delivery Route Planner/
    ├── Program.cs                  # Entry point: load input, run planner, print results
    ├── Models/
    │   ├── Delivery.cs             # record Delivery(Id, Area, Priority, Weight)
    │   └── Trip.cs                 # Trip with TotalWeight and CapacityEfficiency
    ├── Services/
    │   └── DeliveryPlanner.cs      # Validation, sorting and trip packing
    └── deliveries.json/
        └── deliveries.json         # Sample input
```

## Getting started

**Prerequisite:** [.NET 9 SDK](https://dotnet.microsoft.com/download)

```bash
git clone https://github.com/ahmed19104/Delivery-Route-Planner.git
cd "Delivery-Route-Planner/Delivery Route Planner"
dotnet run
```

> **Note:** `Program.cs` currently reads the input from a hard-coded local path (`filePath`). Change it to the location of `deliveries.json/deliveries.json` in your clone before running, or point it at a command-line argument.

## Input format

A JSON array of deliveries:

```json
[
  { "Id": 1, "Area": "Nasr City", "Priority": 2, "Weight": 4.5 }
]
```

| Field | Type | Meaning |
| --- | --- | --- |
| `Id` | int | Delivery identifier |
| `Area` | string | Destination area |
| `Priority` | int | Lower number is more urgent |
| `Weight` | double | Package weight in kg |

## Design decisions and trade-offs

The planner is a **greedy algorithm with multi-level sorting**. It puts urgency before packing efficiency.

- Pros: simple, predictable, `O(n log n)`, urgent deliveries are never delayed to fill space.
- Cons: it does not solve bin packing optimally. Example: a priority 1 package of 2 kg (area A) and a priority 1 package of 7 kg (area B) share a trip to honor priority, even if a later package from area A could fill the space better.

## Scaling to 1,000,000+ requests

- Loading everything into memory and sorting it becomes the bottleneck.
- Store requests in a database indexed on `(Priority, Area)`.
- Stream data with `IAsyncEnumerable` or paged batches and build trips incrementally.

## Future improvements

- Read capacity and rules from `appsettings.json`.
- Use a distance matrix or routing API instead of grouping by area name.
- Add xUnit tests for empty input, boundary weights (exactly 10 kg) and priority ties.
- Accept the input file path as a command-line argument.

## Author

**Ahmed Yaser** - [@ahmed19104](https://github.com/ahmed19104)
