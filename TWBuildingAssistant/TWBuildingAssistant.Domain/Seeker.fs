module TWBuildingAssistant.Domain.Seeker

type internal CalculationSlot =
    { Descriptor:Provinces.SlotDescriptor
      Branch:Buildings.BuildingBranch option
      Level:Buildings.BuildingLevel option
      RegionId:int
      SlotIndex:int }

type internal CombinationTaskRegion = 
    { Slots:CalculationSlot[] }

type internal CombinationTask = 
    { Regions:CombinationTaskRegion[] }

type internal SeekerSettingsSlot = 
    { Branch:Buildings.BuildingBranch option
      Level:Buildings.BuildingLevel option
      Descriptor:Provinces.SlotDescriptor
      RegionId:int
      SlotIndex:int }

type internal SeekerSettingsRegion = 
    { Slots:SeekerSettingsSlot[] }

type internal SeekerResult = 
    { Branch:Buildings.BuildingBranch
      Level:Buildings.BuildingLevel
      RegionId:int
      SlotIndex:int }

type internal SeekerResultWithIncome =
    { Income:float
      CapitalTierMin:int
      CapitalTierSum:int
      Result:SeekerResult[] }

let internal getRegionCombinationsToSeek (buildingLibrary:Buildings.BuildingLibraryEntry[]) regionSeekerSettings =
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
                let branchFilter (branch: Buildings.BuildingBranch) =
                    match (branch.Id, branch.Interesting) with
                    | _, false -> false
                    | "", _ -> true
                    | _, _ ->
                        simulationSlots 
                        |> Array.filter (fun y -> y.Level <> None) 
                        |> Array.append(combination) 
                        |> Array.forall (fun y -> y.Branch <> Some branch)
                options[slotIndex]
                    |> Array.filter branchFilter
                    |> Array.map (fun x -> recursiveSeek (slotIndex + 1) (combination |> Array.append [|{ slot with Branch = Some x; Level = None }|] ))
                    |> Array.collect id

    recursiveSeek 0 [||]

let internal getCombinationsToSeek buildingLibrary (seekerSettings:SeekerSettingsRegion[]) =
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
                |> Array.collect id

    recursiveSeek 0 [||]

let internal combinationComparerAscending left right =
    let capitalTiersMinCompare = compare left.CapitalTierMin right.CapitalTierMin
    if capitalTiersMinCompare = 0 then

        let capitalTiersSumCompare = compare left.CapitalTierSum right.CapitalTierSum
        if capitalTiersSumCompare = 0 then
            compare left.Income right.Income
        else
            capitalTiersSumCompare
    else
        capitalTiersMinCompare

let internal combinationComparerDescending left right =
    -1 * (combinationComparerAscending left right)

let internal seek settings predefinedEffect buildingLibrary seekerSettings minimalCondition resetProgress incrementProgress =
    resetProgress 0
    let combinations = getCombinationsToSeek buildingLibrary seekerSettings
    resetProgress combinations.Length

    let loop combination =
        let rec recursiveSeek regionIndex slotIndex combinationResult (combinationRegionResult:SeekerResult list) :SeekerResultWithIncome option =
            if (regionIndex < combination.Regions.Length) then
                if (slotIndex < combination.Regions.[regionIndex].Slots.Length) then
                    let slot = combination.Regions.[regionIndex].Slots.[slotIndex]
                    match (slot.Branch, slot.Level) with
                    | None, _ ->
                        failwith "This shouldn't happen"
                    | Some slotBranch, None ->
                        slotBranch.Levels
                        |> Array.map (fun levelOption -> recursiveSeek regionIndex (slotIndex + 1) combinationResult ({ Branch = slotBranch; Level = levelOption; RegionId = slot.RegionId; SlotIndex = slot.SlotIndex }::combinationRegionResult))
                        |> Array.choose id
                        |> Array.sortByDescending (fun x -> x.Income)
                        |> Array.tryHead
                    | Some slotBranch, Some slotLevel ->
                        recursiveSeek regionIndex (slotIndex + 1) combinationResult ({ Branch = slotBranch; Level = slotLevel; RegionId = slot.RegionId; SlotIndex = slot.SlotIndex }::combinationRegionResult)
                else
                    recursiveSeek (regionIndex + 1) 0 (combinationRegionResult::combinationResult) []
            else
                let state = State.getState (combinationResult |> List.map (fun x -> x |> List.map (fun y -> y.Level))) settings predefinedEffect
                if (minimalCondition state) then
                    let capitalTiers = state.Regions |> Array.map (fun x -> x.CapitalTier)
                    let capitalTierMin = capitalTiers |> Array.min
                    let capitalTierSum = capitalTiers |> Array.sum
                    Some { Income = state.TotalIncome; CapitalTierMin = capitalTierMin; CapitalTierSum = capitalTierSum; Result = (combinationResult |> List.collect id) |> List.toArray }
                else
                    None

        let loopResult = recursiveSeek 0 0 [] []

        incrementProgress ()

        loopResult

    let bestCombination = 
        combinations
        |> Array.map loop
        |> Array.choose id
        |> Array.sortWith combinationComparerDescending
        |> Array.tryHead

    match bestCombination with
    | Some bestCombination ->
        bestCombination.Result
    | None ->
        [||]