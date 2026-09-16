# Delivery Route Planner — Technical Challenge 2026

A **.NET 8** Console Application designed to organize delivery requests into optimized trips based on weight constraints, urgent priorities, and area grouping.

---

##  How to Run

1. Clone or download the repository.
2. Ensure **.NET 8 SDK** is installed.
3. Place your sample input file `deliveries.json` in the project root directory.
4. Run the application using the CLI:
   ```bash
   dotnet run

    Features & Extension
Multi-Criteria Grouping: Prioritizes urgent items while grouping deliveries heading to the same destination.

Strict Capacity Enforcement: Enforces a maximum limit of 10.0 kg per vehicle trip.

Trip Capacity Efficiency Metric (My Extension):
Calculates and displays the load utilization percentage for each vehicle trip:
Trip Efficiency (%) = (Total Trip Weight / 10.0 kg) * 100
Helps logistics managers evaluate how efficiently vehicle capacities are utilized across all routes.

 Technical Reflections & Q&A
1. Reasoning & Decision Making
The solution utilizes a Greedy Strategy combined with multi-level sorting:

Validation & Filtering: Invalid inputs (packages exceeding 10 kg or with zero/negative weights) are filtered out immediately and logged separately as edge cases.

Multi-Criteria Sorting: Valid deliveries are sorted primarily by Priority (ascending, so urgent items are handled first) and secondarily by Area (alphabetically) to group same-destination deliveries naturally.

Trip Packing: Iterates through sorted deliveries, filling the current vehicle until adding the next item would breach the 10.0 kg limit, at which point a new trip is instantiated.

2. Challenges & Edge Cases
Trade-off Balancing: Balancing strict priority constraints with optimal area grouping without breaching the 10.0 kg threshold.

Edge-Case Validation: Handling single heavy items exceeding 10.0 kg gracefully without failing the entire pipeline execution.

3. Algorithmic Trade-offs
Because this is a Greedy Approach prioritizing urgency over strict bin-packing (similar to the NP-Hard Bin Packing Problem), it may not always yield the absolute best space grouping:

Example: If a Priority 1 package weighs 2 kg (Area A) and another Priority 1 package weighs 7 kg (Area B), they will be placed in the same trip to honor priority first—rather than waiting to find a Priority 2 package from Area A that might fill the remaining space better.

4. Scalability (1,000,000+ Requests)
Memory Constraints: Loading 1,000,000 JSON records into RAM simultaneously creates heavy memory consumption.

Sorting Overhead: In-memory sorting has an O(N log N) complexity, creating a processing bottleneck at scale.

Solution for Scale: Transition to a database with proper indexing on (Priority, Area), stream data using IAsyncEnumerable or paginated batches, and process trips incrementally.

5. Future Improvements
Dynamic Capacity: Move capacity limits and rule definitions to appsettings.json.

Distance Matrix Integration: Integrate a graph-based route optimization library or distance matrix API to calculate exact road distances instead of string matching.

Unit Testing: Implement comprehensive tests using xUnit covering edge cases like empty files, boundary limits, and priority ties.
