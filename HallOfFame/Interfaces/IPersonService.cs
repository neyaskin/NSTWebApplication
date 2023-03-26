using HallOfFame.Models.Requests;
using HallOfFame.Models.Response;

namespace HallOfFame.Interfaces;

public interface IPersonService
{
    public List<PersonResponse> GetAll();
    public PersonResponse Get(int id);
    public PersonResponse Add(PersonRequest personRequest);
    public void Update(int id, PersonRequest personRequest);
    public void Delete(int id);
}