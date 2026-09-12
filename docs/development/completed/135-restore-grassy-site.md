# Task 135 - Restore the grassy site and clear sky

- Why: the user requests removal of clouds, trees, river and decorative ground rocks, and restoration of the continuous grassy land patch.
- Integrated: whole-surface turf/grass, seeded height variation, existing natural wind, geometric distance detail and compact instancing. MainGame and its builder omit clouds/scenery/stone dressing; independent sun sky retains the accepted cyan gradient.
- Cleanup: 33 owned runtime/import/meta files removed without remaining GUID references. Editable sources/licenses stay inactive outside Unity; original collectible rocks and user recovery scene/materials remain.
- Evidence: fast compile, MainGame scene validation and grass cut/reset/restore/reenable tests pass. Official CLI player-height, overhead, sky/sun and cut inspection confirms 7,996 supported clumps across all 144 patches (minimum 40 per patch, zero empty patches); the reviewed cut clears 43 clumps. Review does not start a player save.
- Delivery: [Windows executable](../../../builds/windows/SomethingDownThere.exe), **2026-09-12 19:46 UTC**, zero errors / one existing Pipeline runtime warning; clean seven-second native startup. [Reports](../../../unity/Logs/Task135/). Temporary captures cleaned; no commit.
- Limit: preserved rendering optimizations are verified; no new native frame-time benchmark or zero-hitch guarantee for the fuller lawn. Next eligible work is 126; reservoir art remains staged pending its existing specific approval.
