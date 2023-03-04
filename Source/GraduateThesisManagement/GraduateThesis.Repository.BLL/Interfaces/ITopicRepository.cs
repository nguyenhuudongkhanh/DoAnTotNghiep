using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface ITopicRepository : IAsyncSubRepository<TopicInput, TopicOutput, string>
{

}
