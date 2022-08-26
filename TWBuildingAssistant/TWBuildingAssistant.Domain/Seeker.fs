module TWBuildingAssistant.Domain.Seeker

open Buildings
open Province
open State

type CalculationSlot =
    { Descriptor:SlotDescriptor
      Branch:BuildingBranch option
      Level:BuildingLevel option
      RegionId:int
      SlotIndex:int }

type CombinationTaskRegion = 
    { Slots:CalculationSlot[] }

type CombinationTask = 
    { Regions:CombinationTaskRegion[] }

type SeekerSettingsSlot = 
    { Branch:BuildingBranch option
      Level:BuildingLevel option
      Descriptor:SlotDescriptor
      RegionId:int
      SlotIndex:int }

type SeekerSettingsRegion = 
    { Slots:SeekerSettingsSlot[] }

type SeekerResult = 
    { Branch:BuildingBranch
      Level:BuildingLevel
      RegionId:int
      SlotIndex:int }

type SeekerResultWithWealth =
    { Wealth:double
      Result:SeekerResult[] }

type MinimalConditionDelegate = delegate of ProvinceState -> bool
type ResetProgressDelegate = delegate of int -> unit
type IncrementProgressDelegate = delegate of unit -> unit

let getRegionCombinationsToSeek (buildingLibrary:BuildingLibraryEntry[]) regionSeekerSettings =
    let simulationSlots = 
        regionSeekerSettings.Slots
        |> Array.map (fun y -> { Descriptor = y.Descriptor; Branch = y.Branch; Level = y.Level; RegionId = y.RegionId; SlotIndex = y.SlotIndex }:CalculationSlot)
    let options = 
        simulationSlots
        |> Array.map (fun x -> (buildingLibrary |> Array.find (fun y -> y.Descriptor = x.Descriptor)).BuildingBranches)

    let simulationSlotsLength =
        simulationSlots |> Array.length

    let rec recursiveSeek slotIndex combination :CombinationTaskRegion[] =
        match slotIndex with
        | slotIndex when slotIndex = simulationSlotsLength ->
            [| { Slots = combination } |]
        | slotIndex ->
            let slot = simulationSlots.[slotIndex];
            match slot.Level with
            | Some level ->
                recursiveSeek (slotIndex + 1) (combination |> Array.append [|slot|])
            | None ->
                let branchFilter (branch:BuildingBranch) =
                    match (branch.Id, branch.Interesting) with
                    | _, false -> false
                    | 0, _ -> true
                    | _, _ ->
                        simulationSlots 
                        |> Array.filter (fun y -> y.Level <> None) 
                        |> Array.append(combination) 
                        |> Array.forall (fun y -> y.Branch <> Some branch)
                options[slotIndex]
                    |> Array.filter branchFilter
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

let seek settings predefinedEffect buildingLibrary seekerSettings (minimalCondition:MinimalConditionDelegate) (resetProgress:ResetProgressDelegate) (incrementProgress:IncrementProgressDelegate) =
    resetProgress.Invoke 0
    let combinations = getCombinationsToSeek buildingLibrary seekerSettings
    resetProgress.Invoke combinations.Length

    let loop combination =
        let rec recursiveSeek regionIndex slotIndex combinationResult (combinationRegionResult:SeekerResult list) =
            if (regionIndex < combination.Regions.Length) then
                if (slotIndex < combination.Regions.[regionIndex].Slots.Length) then
                    let slot = combination.Regions.[regionIndex].Slots.[slotIndex]
                    match (slot.Branch, slot.Level) with
                    | None, _ ->
                        failwith "This shouldn't happen"
                    | Some slotBranch, None ->
                        slotBranch.Levels
                        |> Array.map (fun levelOption -> recursiveSeek regionIndex (slotIndex + 1) combinationResult ({ Branch = slotBranch; Level = levelOption; RegionId = slot.RegionId; SlotIndex = slot.SlotIndex }::combinationRegionResult))
                        |> Array.choose (fun x -> x)
                        |> Array.sortByDescending (fun x -> x.Wealth)
                        |> Array.tryHead
                    | Some slotBranch, Some slotLevel ->
                        recursiveSeek regionIndex (slotIndex + 1) combinationResult ({ Branch = slotBranch; Level = slotLevel; RegionId = slot.RegionId; SlotIndex = slot.SlotIndex }::combinationRegionResult)
                else
                    recursiveSeek (regionIndex + 1) 0 (combinationRegionResult::combinationResult) []
            else
                let state = getState (combinationResult |> List.map (fun x -> x |> List.map (fun y -> y.Level))) settings predefinedEffect
                if (minimalCondition.Invoke state) then
                    Some { Wealth = state.Regions |> Array.sumBy (fun x -> x.Wealth); Result = (combinationResult |> List.collect (fun x -> x)) |> List.toArray }
                else
                    None

        let loopResult = recursiveSeek 0 0 [] []

        incrementProgress.Invoke ()

        loopResult

    let bestCombination = 
        combinations
        |> Array.map loop
        |> Array.choose (fun x -> x)
        |> Array.sortByDescending (fun x -> x.Wealth)
        |> Array.tryHead

    match bestCombination with
    | Some bestCombination ->
        bestCombination.Result
    | None ->
        [||]