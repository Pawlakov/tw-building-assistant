module TWBuildingAssistant.Data.FSharp.Seeker

open Models

let getRegionCombinationsToSeek (buildingLibrary:BuildingLibraryEntry[]) regionSeekerSettings =
    let simulationSlots = 
        regionSeekerSettings.Slots
        |> Array.map (fun y -> { Descriptor = Some y.Descriptor; Branch = y.Branch; Level = y.Level; RegionId = y.RegionId; SlotIndex = y.SlotIndex }:CalculationSlot)
    let options = 
        simulationSlots
        |> Array.map (fun x -> (buildingLibrary |> Array.find (fun y -> Some y.Descriptor = x.Descriptor)).BuildingBranches)

    let simulationSlotsLength =
        simulationSlots |> Array.length

    let rec recursiveSeek slotIndex combination :CombinationTaskRegion[] =
        match slotIndex with
        | slotIndex when slotIndex = simulationSlotsLength ->
            [| { Slots = combination } |]
        | slotIndex ->
            let slot = simulationSlots.[slotIndex];
            match slot.Level with
            | None ->
                recursiveSeek (slotIndex + 1) (combination |> Array.append [|slot|])
            | Some level ->
                let slotOptions = 
                    options[slotIndex]
                        |> Array.filter (fun x -> x.Interesting)
                        |> Array.filter (fun x -> x.Id = 0 || simulationSlots |> Array.filter (fun y -> y.Level <> None) |> Array.append(combination) |> Array.forall (fun y -> y.Branch <> Some x))
                slotOptions
                    |> Array.map (fun x -> recursiveSeek (slotIndex + 1) (combination |> Array.append [|{ slot with Branch = Some x; Level = None }|] ))
                    |> Array.collect (fun x -> x)

    recursiveSeek 0 [||]

let getCombinationsToSeek buildingLibrary (seekerSettings:SeekerSettingsRegion[]) =
    let regionCombinations =
        seekerSettings
        |> Array.map (getRegionCombinationsToSeek buildingLibrary)

    let rec recursiveSeek regionIndex (combination:CombinationTaskRegion[]) =
        if (regionIndex = regionCombinations.Length) then
            [| { Regions = combination |> Array.map (fun x -> { Slots = x.Slots |> Array.map (fun x -> { x with Level = x.Level } ) } ) } |]
        else
            let regionOptions = 
                regionCombinations[regionIndex]
            regionOptions
                |> Array.map (fun x -> recursiveSeek (regionIndex + 1) (combination |> Array.append [|x|] ))
                |> Array.collect (fun x -> x)

            
    recursiveSeek 0 [||]

//let seek settings predefinedEffect buildingLibrary seekerSettings minimalCondition updateProgressMax updateProgressValue =
//    updateProgressMax 0
//    updateProgressValue 0

//    let combinations = getCombinationsToSeek buildingLibrary seekerSettings
//    updateProgressMax combinations.Length

//    let completedCounter = 0;
//    let bestCombination = []
//    let bestWealth = 0.0;

//    let singleCombination combination =
//        let seekingSlots = combination.Regions.SelectMany(x => x.Slots).Where(x => x.Level == null).ToArray();

//        let rec recursiveSeek slotIndex =
//            if (slotIndex < seekingSlots.Length) then
//                var seekResult = Enumerable.Empty<object>();
//                var slot = seekingSlots[slotIndex];
//                foreach (var levelOption in slot.Branch.Levels)
//                {
//                    slot.Level = levelOption;
//                    RecursiveSeek(slotIndex + 1);
//                    slot.Level = null;
//                }
//            else
//                var state = Data.FSharp.State.getState(combination.Regions.Select(x => x.Slots.Select(y => y.Level)), settings, predefinedEffect);
//                if (minimalCondition(state) && state.Wealth > bestWealth)
//                {
//                    lock (bestCombination)
//                    {
//                        bestWealth = state.Wealth;
//                        bestCombination.Clear();
//                        bestCombination.AddRange(seekingSlots.Select(x => new SeekerResult(x.Branch, x.Level, x.RegionId, x.SlotIndex)));
//                    }
//                }

//        recursiveSeek 0 

//        lock updateProgressValue (fun () -> updateProgressValue(++completedCounter))

//    combinations |> Array.Parallel.iter singleCombination

//    bestCombination