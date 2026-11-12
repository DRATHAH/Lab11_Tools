BFS: Calculates the shortest path regardless of movement cost in a grid-like pattern. Calculates based on the number of steps from the starting position. (How many steps away)
Dijkstra: Similar to BFS, searches for the cheapest available option to move to. Uses priority queues to sort valid moves. (How much it costs to get to where I am)
A*: Uses estimates between current position and destination, then chooses the cheapest tile both to reach and cheapest from the goal. (How much it costs to get to where I am and how far am I from the goal)

When dynamically updating obstacles, paths need to be recalculated based on the current location (if the object is in the middle of navigating the original path) and ensure that the destination is still valid.

A*, as it is more efficient (doesn't check as many valid move locations) and accounts for things like terrain roughness.

I would use a priority queue to sort the valid move locations. That way the pathfinding algorithm would be able to account for the terrain differences. I don't think A* pathfinding would be necessary in this example as the map is a relatively small grid.