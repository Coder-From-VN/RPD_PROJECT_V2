using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class TrainerRepo : BaseRepository<Trainer>, ITrainerRepo
    {
        public TrainerRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Trainer trainer)
        {
            await _context.Trainers.AddAsync(trainer);
        }

        public async Task<Trainer?> GetByFirebaseUidAsync(string firebaseUid)
        {
            return await _context.Trainers
                .FirstOrDefaultAsync(t => t.FirebaseUid == firebaseUid);
        }

        public async Task<Trainer?> GetByIdAsync(Guid trainerID)
        {
            return await _context.Trainers
                .FirstOrDefaultAsync(t => t.TrainerId == trainerID);
        }
    }
}
