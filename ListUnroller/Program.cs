using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;
using Noggog;

namespace ListUnroller
{
    public class Program
    {
        public static Task<int> Main(string[] args)
        {
            return SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "ListUnroller.esp")
                .Run(args);
        }

        // TODO: De-duplicate this once the mutagen update comes out that includes a more generic way to access these.
        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (var leveledList in state.LoadOrder.PriorityOrder.LeveledItem().WinningOverrides())
            {
                if (!leveledList.EditorID?.ContainsInsensitive("unroll") ?? true) continue;

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
                                Reference = entry.Data.Reference
                            }
                        });
                    }

                    // Modify the original entry
                    entry.Data.Count = 1;
                }
            }

            foreach (var leveledList in state.LoadOrder.PriorityOrder.LeveledNpc().WinningOverrides())
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
                                Reference = entry.Data.Reference
                            }
                        });
                    }

                    // Modify the original entry
                    entry.Data.Count = 1;
                }
            }

            foreach (var leveledList in state.LoadOrder.PriorityOrder.LeveledSpell().WinningOverrides())
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
                                Reference = entry.Data.Reference
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
