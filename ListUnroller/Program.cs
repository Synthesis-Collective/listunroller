using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;

namespace ListUnroller
{
    public static class MyExtensions
    {
        public static bool ContainsInsensitive(this string str, string rhs)
        {
            return str.Contains(rhs, StringComparison.OrdinalIgnoreCase);
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? e)
        {
            if (e == null) return Enumerable.Empty<T>();
            return e;
        }
    }

    public class Program
    {
        public static int Main(string[] args)
        {
            return SynthesisPipeline.Instance.Patch<ISkyrimMod, ISkyrimModGetter>(
                args: args,
                patcher: RunPatch,
                new UserPreferences()
                {
                    ActionsForEmptyArgs = new RunDefaultPatcher()
                    {
                        IdentifyingModKey = "ListUnroller.esp",
                        TargetRelease = GameRelease.SkyrimSE
                    }
                }
            );
        }

        // TODO: De-duplicate this once the mutagen update comes out that includes a more generic way to access these.
        public static void RunPatch(SynthesisState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (var leveledList in state.LoadOrder.PriorityOrder.WinningOverrides<ILeveledItemGetter>())
            {
                if (leveledList.EditorID?.ContainsInsensitive("unroll") == false) continue;

                var modifiedList = state.PatchMod.LeveledItems.GetOrAddAsOverride(leveledList);

                foreach (var entry in modifiedList.Entries.EmptyIfNull().ToList())
                {
                    if (entry.Data == null) continue;

                    // Add an entry per count - 1
                    var count = entry.Data.Count;
                    for (int j = 1; j < count; j++)
                    {
                        modifiedList.Entries?.Add(new LeveledItemEntry()
                        {
                            Data = new LeveledItemEntryData()
                            {
                                Count = 1,
                                Level = entry.Data.Level,
                                Reference = entry.Data.Reference.FormKey
                            }
                        });
                    }

                    // Modify the original entry
                    entry.Data.Count = 1;
                }
            }

            foreach (var leveledList in state.LoadOrder.PriorityOrder.WinningOverrides<ILeveledNpcGetter>())
            {
                if (leveledList.EditorID?.ContainsInsensitive("unroll") == false) continue;

                var modifiedList = state.PatchMod.LeveledNpcs.GetOrAddAsOverride(leveledList);

                foreach (var entry in modifiedList.Entries.EmptyIfNull().ToList())
                {
                    if (entry.Data == null) continue;

                    // Add an entry per count - 1
                    var count = entry.Data.Count;
                    for (int j = 1; j < count; j++)
                    {
                        modifiedList.Entries?.Add(new LeveledNpcEntry()
                        {
                            Data = new LeveledNpcEntryData()
                            {
                                Count = 1,
                                Level = entry.Data.Level,
                                Reference = entry.Data.Reference.FormKey
                            }
                        });
                    }

                    // Modify the original entry
                    entry.Data.Count = 1;
                }
            }

            foreach (var leveledList in state.LoadOrder.PriorityOrder.WinningOverrides<ILeveledSpellGetter>())
            {
                if (leveledList.EditorID?.ContainsInsensitive("unroll") == false) continue;

                var modifiedList = state.PatchMod.LeveledSpells.GetOrAddAsOverride(leveledList);

                foreach (var entry in modifiedList.Entries.EmptyIfNull().ToList())
                {
                    if (entry.Data == null) continue;

                    // Add an entry per count - 1
                    var count = entry.Data.Count;
                    for (int j = 1; j < count; j++)
                    {
                        modifiedList.Entries?.Add(new LeveledSpellEntry()
                        {
                            Data = new LeveledSpellEntryData()
                            {
                                Count = 1,
                                Level = entry.Data.Level,
                                Reference = entry.Data.Reference.FormKey
                            }
                        });
                    }

                    // Modify the original entry
                    entry.Data.Count = 1;
                }
            }
        }
    }
}
