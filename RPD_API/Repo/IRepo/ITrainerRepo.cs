using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface ITrainerRepo : IBaseRepository
    {
        Task AddAsync(Trainer trainer);
        Task<Trainer?> GetByFirebaseUidAsync(string firebaseUid);
        Task<Trainer?> GetByIdAsync(Guid trainerID);
    }
}
