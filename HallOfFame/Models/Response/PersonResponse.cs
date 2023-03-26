namespace HallOfFame.Models.Response;

public class PersonResponse
{
    public long? Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public List<SkillsResponse> Skills { get; set; }
    

}