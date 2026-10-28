---
type: Reference
title: Graph queries
description: How to query the live codebase knowledge graph for this repository.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/.codebase-memory
tags: [reference, graph, tooling]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Project

The graph is served by the `codebase-memory-mcp` server under project **`realworld-dotnet`**,
backed by `.codebase-memory/graph.db.zst`. If the name does not resolve, call `list_projects` and
pick the entry whose root is this repository.

# Shape

| Node label | Count | Edge type | Count |
| --- | --- | --- | --- |
| Method | 313 | USAGE | 1777 |
| Field | 290 | DEFINES | 1274 |
| Variable | 242 | CALLS | 481 |
| File / Module | 197 each | DEFINES_METHOD | 313 |
| Class | 177 | IMPORTS | 296 |
| Folder | 125 | DECORATES | 162 |
| Section | 43 | CONTAINS_FILE | 197 |
| Decorator | 16 | IMPLEMENTS | 24 |
| Interface | 11 | OVERRIDE | 22 |
| Enum / Branch / Project | 1 each | INHERITS | 8 |

Totals: 1614 nodes, 5089 edges, across 182 C# files and 5 YAML files.

# Useful Calls

| Goal | Call |
| --- | --- |
| Orientation | `get_architecture(project="realworld-dotnet", aspects=["overview"])` |
| Find a symbol | `search_graph(project=…, query="SetReminderCommandHandler")` |
| Semantic search | `search_graph(project=…, semantic_query=["daily reminder limit"])` |
| Read source | `get_code_snippet(project=…, qualified_name="TutorialsController")` |
| File contents | `get_file_outline(project=…, file_path="src/CleanArchitecture.Domain/Users/User.cs")` |
| Who calls X | `trace_path` between two qualified names |

# Cypher Examples

All handlers:

```cypher
MATCH (c:Class) WHERE c.name ENDS WITH 'Handler'
RETURN c.file_path AS f, c.name AS name, c.start_line AS line ORDER BY f
```

Everything an interface touches:

```cypher
MATCH (i:Interface)-[:DEFINES_METHOD]->(m)
WHERE i.name = 'ITutorialsRepository'
RETURN m.name, m.start_line
```

Highest fan-in methods:

```cypher
MATCH (m:Method)<-[r:CALLS]-() RETURN m.qualified_name AS qn, count(r) AS fan_in
ORDER BY fan_in DESC
```

# Caveats

* `query_graph` rejects list indexing such as `labels(n)[0]`; project the properties you need
  instead, or run one query per label.
* `get_architecture` returns `routes` as empty for this repository — ASP.NET attribute routing is
  not extracted, so use [Api](/modules/api.md) for the endpoint table.
* `graph=missed` lists flagged coverage gaps; its absence is not proof of completeness.
