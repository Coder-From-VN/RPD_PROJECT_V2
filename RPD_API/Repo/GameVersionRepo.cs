using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class GameVersionRepo : IGameVersionRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public GameVersionRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> AddGameVersion(GameVersionDTO model)
        {
            var newGameVersion = _mapper.Map<GameVersion>(model);
            _context.GameVersion!.Add(newGameVersion);
            await _context.SaveChangesAsync();

            return newGameVersion.gvID;
        }

        public async Task DeleteGameVersion(Guid gvID)
        {
            var gameVersion = _context.GameVersion!.SingleOrDefault(b => b.gvID == gvID);
            if (gameVersion != null)
            {
                _context.GameVersion!.Remove(gameVersion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<GameVersionDTO>> GetAllGameVersion()
        {
            var gameVersion = await _context.GameVersion.ToListAsync();
            return _mapper.Map<List<GameVersionDTO>>(gameVersion);
        }

        public async Task<GameVersionDTO> GetGameVersionById(Guid gvID)
        {
            var gameVersion = await _context.GameVersion!.FindAsync(gvID);
            return _mapper.Map<GameVersionDTO>(gameVersion);
        }

        public async Task UpdateGameVersion(Guid gvID, GameVersionDTO model)
        {
            if (gvID == model.gvID)
            {
                var updateGameVersion = _mapper.Map<GameVersion>(model);
                _context.GameVersion!.Update(updateGameVersion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
