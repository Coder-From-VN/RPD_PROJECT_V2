using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.DTO.Move;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class MoveRepo : IMoveRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public MoveRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Guid> AddMove(PostMoveDTO model)
        {
            var newMove = _mapper.Map<Move>(model);

            _context.Move.Add(newMove);
            await _context.SaveChangesAsync();

            var saved = await _context.Move
                .Include(m => m.Types)
                .FirstAsync(m => m.moveID == newMove.moveID);

            return saved.moveID;
        }

        public async Task DeleteMove(Guid moveID)
        {
            var move = _context.Move!.SingleOrDefault(b => b.moveID == moveID);
            if (move != null)
            {
                _context.Move!.Remove(move);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MoveDTO>> GetAllMove()
        {
            var move = await _context.Move.ToListAsync();
            return _mapper.Map<List<MoveDTO>>(move);
        }

        public async Task<MoveDTO> GetMoveById(Guid moveID)
        {
            var move = await _context.Move!.FindAsync(moveID);
            return _mapper.Map<MoveDTO>(move);
        }

        public async Task UpdateMove(Guid moveID, MoveDTO model)
        {
            if (moveID == model.moveID)
            {
                var updateMove = _mapper.Map<Move>(model);
                _context.Move!.Update(updateMove);
                await _context.SaveChangesAsync();
            }
        }
    }
}
