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

        public async Task<GameVersionDTO?> AddGameVersion(PostGameVersionDTO model)
        {
            var existing = await _context.GameVersion!.SingleOrDefaultAsync(gv => gv.gvName == model.gvName);

            if (existing != null)
                return null;

            var newGameVersion = _mapper.Map<GameVersion>(model);
            _context.GameVersion!.Add(newGameVersion);
            var saved = await _context.SaveChangesAsync();

            return saved > 0 ? _mapper.Map<GameVersionDTO>(newGameVersion) : null;
        }

        public async Task<bool> DeleteGameVersion(Guid gvID)
        {
            var gameVersion = _context.GameVersion!.SingleOrDefault(b => b.gvID == gvID);
            if (gameVersion != null)
            {
                _context.GameVersion!.Remove(gameVersion);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
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

        public async Task<bool> UpdateGameVersion(Guid gvID, PutGameVersionDTO model)
        {
            var gameVersion = await _context.GameVersion!.FindAsync(gvID);
            if (gameVersion != null)
            {
                if (model.gvName != "")
                    gameVersion.gvName = model.gvName;
                if (model.gvGen != 0)
                    gameVersion.gvGen = model.gvGen;

                _context.GameVersion!.Update(gameVersion);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }
    }
}
