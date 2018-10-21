namespace TWBuildingAssistant.Model.Map
{
    using System;

    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Resources;

    public class Region : IRegion
    {
        private const int CityDefaultSlotsCount = 6;

        private const int TownDefaultSlotsCount = 4;

        private IResource resource;

        private Parser<IResource> resourceParser;

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
        
        public bool IsCity { get; set; }
        
        public int? ResourceId { get; set; }
        
        public bool IsCoastal { get; set; }
        
        public int SlotsCountOffset { get; set; }

        public int GetSlotsCount()
        {
            if (this.IsCity)
            {
                return CityDefaultSlotsCount + this.SlotsCountOffset;
            }

            return TownDefaultSlotsCount + this.SlotsCountOffset;
        }

        public IResource GetResource()
        {
            if (this.resourceParser == null)
            {
                throw new MapException("Resource has not been parsed.");
            }

            if (this.resource != null || !this.ResourceId.HasValue)
            {
                return this.resource;
            }

            this.resource = this.resourceParser.Find(this.ResourceId);
            if (this.resource == null && this.ResourceId.HasValue)
            {
                throw new MapException($"No resource with id = {this.ResourceId.Value}.");
            }

            return this.resource;
        }

        public void SetResourceParser(Parser<IResource> parser)
        {
            this.resourceParser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.resource = null;
        }

        public bool Validate(out string message)
        {
            if (this.ResourceId.HasValue && this.ResourceId.Value < 1)
            {
                message = "Resource id is out od range.";
                return false;
            }

            if (this.resourceParser == null)
            {
                message = "Resource parser is missing.";
                return false;
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                message = "Name is empty.";
                return false;
            }

            if (this.SlotsCountOffset > 0 || this.SlotsCountOffset < -1)
            {
                message = "Slots count offset is out of range.";
                return false;
            }

            message = "Values are correct.";
            return true;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}