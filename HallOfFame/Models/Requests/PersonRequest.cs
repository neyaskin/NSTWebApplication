using HallOfFame.Models.Response;

namespace HallOfFame.Models.Requests;

public class PersonRequest
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public List<SkillsRequest> Skills { get; set; }
}