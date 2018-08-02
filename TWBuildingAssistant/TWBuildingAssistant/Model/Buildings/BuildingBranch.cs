namespace TWBuildingAssistant.Model.Buildings
{
    using System.Linq;
    using System.Xml.Linq;

    // Gałąź drzewa budynków.
    public class BuildingBranch
    {
        // Nazwa
        public string Name { get; }
        // Jeśli budynek daje bonus jakieś religii to to nie jest null.
        public Religions.IReligion Religion { get; }
        // True - jeśli religia budynku i państwowa są różne to może być użyty. False - może
        public bool IsReligiouslyExclusive { get; }
        // Poziomy budynków wchodzące w skład gałęzi.
        private BuildingLevel[] _levels;
        //
        public BuildingLevel[] Levels
        {
            get { return _levels.ToArray(); }
        }
        //
        public BuildingBranch(XElement element, ITechnologyLevelAssigner technologyLevelAssigner, Map.IReligionParser religionParser)
        {
            Name = (string)element.Attribute("n");
            _levels = (from XElement subelement in element.Elements() select new BuildingLevel(this, subelement, technologyLevelAssigner)).ToArray();
            IsReligiouslyExclusive = false;
            if (element.Attribute("r") != null)
            {
                Religion = religionParser.Parse((string)element.Attribute("r"));
                IsReligiouslyExclusive = (bool)element.Attribute("ire");
            }
        }
        //
        public BuildingLevel this[int whichLevel]
        {
            get { return _levels[whichLevel]; }
        }
        // Czy przy obecnej sytuacji ta gałąź jest dostępna.
        public bool IsAvailable
        {
            get
            {
                // Czy choć jeden poziom jest technologicznie dostępny.
                if (_levels.All((BuildingLevel level) => !level.IsAvailable))
                    return false;
                // Czy spełniony jest warunek religii.
                if (!IsReligiouslyExclusive)
                    return true;
                if (this.Religion == null || this.Religion.IsState)
                    return true;
                return false;
            }
        }
    }
}