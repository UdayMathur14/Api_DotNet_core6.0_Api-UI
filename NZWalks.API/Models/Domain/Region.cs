namespace NZWalks.API.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; }    

        public string Code { get; set; }    

        public string Name { get; set; }    

        public string? RegionImageUrl { get; set; } //string? means nullable value means can be null .
        
        //Navigation property , one region can have multiple walks

       // public IEnumerable<Walk> Walks { get; set;}


    }
}
