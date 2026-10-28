# Knowledge Update Log

## 2026-10-28

* **Creation**: `modules/domain-categories.md` and `modules/application-categories.md` for the new
  `Category` entity, its create/list use cases and `ICategoriesRepository` (project
  `realworld-dotnet` @ `d56e8561a0fe55a0a123dab492ae019f009f7375`), trace `62e4b98ec9d4`.
* **Update**: the persistence provider moved from SQLite to PostgreSQL in the same trace.
  `overview.md`, `architecture.md`, `index.md`, `modules/index.md`,
  `modules/infrastructure-persistence.md`, `modules/tests-api-integrationtests.md` and
  `modules/tests-subcutaneous.md` now describe `UseNpgsql` with a configured
  `ConnectionStrings:DefaultConnection`, the new `RequestPipeline.ApplyMigrations`, the collapsed
  `20261028114456_InitialCreate` baseline, and the integration suite's Testcontainers
  `PostgresTestDatabase` (replacing the deleted `SqliteTestDatabase`) while subcutaneous tests stay
  on SQLite. Recorded that the checked-in `docker-compose.yml` still mounts the removed
  `CleanArchitecture.sqlite` and so does not match `.env.example`.
* **Update**: `modules/api.md`, `modules/contracts.md`, `modules/application-common.md`,
  `modules/application-tutorials.md`, `modules/domain-tutorials.md`,
  `modules/tests-testcommon.md`, `conventions.md` and `playbooks/common-tasks.md` for the
  `api/categories` endpoints, the optional `Tutorial.CategoryId` foreign key and its `SetNull`
  relationship, the widened `Tutorial` constructor and `Update` signatures, and the new TestCommon
  category factories. Corrected the previous claim that both pipeline behaviors are constrained to
  `IAuthorizeableRequest` -- only `AuthorizationBehavior` is, which is how validation still runs
  for the anonymous category commands.
* **Update**: `overview.md` -- recorded `.github/workflows/api-correction.yml` in the running and
  building table, the only file changed since the last run (project `realworld-dotnet`
  @ `2650bfd28ce9976b5db0a140e2928725519d0af3`), trace `5301463f0930`. No source or test project
  changed, so all module, architecture and convention concepts were left untouched.

## 2026-10-27

* **Creation**: Full bundle generated from the codebase knowledge graph (project `realworld-dotnet`
  @ `40766801559612ce58c0226d1768da27f4b2e358`), trace `1cf887f4d14f`. Added `index.md`,
  `overview.md`, `architecture.md`, `conventions.md`, `playbooks/common-tasks.md`,
  `reference/graph-queries.md`, and 20 module concepts covering the five source projects and five
  test projects.
