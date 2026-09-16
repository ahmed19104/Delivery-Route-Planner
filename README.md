
# Delivery Route Planner - Technical Challenge 2026

A .NET 8 Console Application designed to organize delivery requests into optimized trips based on weight constraints, urgent priorities, and area grouping.

## How to Run
1. Clone or download the repository.
2. Ensure `.NET 8 SDK` is installed.
3. Place your sample input file `deliveries.json` in the project root directory.
4. Run the application using the CLI:
   ```bash
   dotnet run

 Additional Feature (My Extension)
Trip Capacity Efficiency Metric:
Calculates and displays the load utilization percentage for each vehicle trip ((Total Trip Weight / 10.0 kg) * 100). This helps logistics managers evaluate how efficiently vehicle capacities are utilized across all routes.

1=> Reasoning & Decision Making
1. Explain your solution approach in your own words.
The solution utilizes a Greedy Strategy combined with multi-level sorting:

Validation & Filtering: First, invalid inputs (e.g., packages exceeding 10 kg or having zero/negative weights) are filtered out and logged separately as edge cases.

Multi-Criteria Sorting: Valid deliveries are sorted primarily by Priority (ascending, so urgent items are handled first) and secondarily by Area (alphabetically) to naturally group deliveries heading to the same destination together.

Trip Packing: The algorithm iterates through sorted deliveries and fills the current vehicle until adding the next item would breach the 10 kg capacity limit, at which point a new trip is instantiated.


2=> What was the most difficult part of the assignment?
Balancing the trade-off between strict priority order and optimal area grouping without exceeding the 10 kg capacity limit. Deciding how to handle single heavy items exceeding 10 kg gracefully without breaking the pipeline execution required careful edge-case validation.

3=> Are there situations where your algorithm may not produce the best possible grouping? Explain.
Yes. Because this is a Greedy Approach prioritizing urgency over optimal space bin-packing (similar to the Bin Packing Problem, which is NP-Hard):

If a Priority 1 package weighs 2 kg (Area A) and another Priority 1 package weighs 7 kg (Area B), they will be placed in the same trip to honor priority first, rather than waiting to find another Priority 2 package from Area A that might fill the remaining space better.


4. If the input contained 1,000,000 delivery requests, what part of your solution might become slow or memory-intensive?Memory Constraints: Loading 1,000,000 JSON records into memory simultaneously as objects would cause high RAM consumption.Sorting Overhead: Sorting 1,000,000 records in-memory has an $O(N \log N)$ complexity, creating a processing bottleneck.
 Solution for Scale: Transition to a database with proper indexing (Priority, Area), read data using streaming/chunking (IAsyncEnumerable or batches), and process trips incrementally.

5=> What would you improve if you had another day to work on the solution?
Implement dynamic capacity options via a configuration file (appsettings.json).

Integrate a graph-based route optimization library or distance matrix API to calculate exact road distances between areas instead of simple string grouping.

Write comprehensive Unit Tests (using xUnit) covering edge cases like empty files, maximum boundaries, and priority ties.
