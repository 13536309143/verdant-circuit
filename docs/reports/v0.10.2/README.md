# v0.10.2 validation

These reports run against the actual Unity model and VM, not the historical Python fleet prototype.

- `fleet.json`: 45 assertions, including eight mixed-size drones over 1,000 scheduling ticks, legacy migration, maintenance recovery, specialist speed, single-step pause and the actual two-drone template.
- `rules.json`: 50 core checks; compiles all eleven templates and exercises the six trial examples.
- `campaign.json`: all 30 contracts completed using production, normal research spending and quantity gates, with no fleet purchase requirement.
- `localization.json`: 1,216 nonempty entries across English and Simplified Chinese tables, plus lookup checks in both locales.
- `playmode.json`: actual advancing frames, both UI locales, the Fleet button's raycast reachability and action, and a completed queued irrigation pass. This runs with an isolated test save.
- Platform build summaries: both report zero errors and warnings. Mac standalone startup and F8 language switching were also visually checked; screenshots in `docs/images/` use a seeded mixed-fleet fixture.

The campaign replay is deterministic and does not measure human difficulty. Input behavior under remote automation and native Windows runtime compatibility require broader manual testing. A blocked queue job may expire; collision safety is not a universal traffic solver. No floating-island integration is claimed.
