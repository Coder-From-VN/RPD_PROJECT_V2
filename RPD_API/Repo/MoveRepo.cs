using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.DTO.Move;
using RPD_API.DTO.Types;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using System.Drawing.Drawing2D;

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


        public async Task<MoveDTO> AddMove(PostMoveDTO model)
        {
            var existing = await _context.Move!.SingleOrDefaultAsync(m => m.moveName == model.moveName);

            if (existing != null)
                return null;

            var newMove = _mapper.Map<Move>(model);
            _context.Move.Add(newMove);
            var saved = await _context.SaveChangesAsync();

            if (saved > 0)
            {
                var moveWithType = await _context.Move
                    .Include(m => m.Types)
                    .FirstOrDefaultAsync(m => m.moveID == newMove.moveID);
                return _mapper.Map<MoveDTO>(moveWithType);
            }
            return null;
        }

        public async Task<bool> DeleteMove(Guid moveID)
        {
            var move = _context.Move!.SingleOrDefault(b => b.moveID == moveID);
            if (move != null)
            {
                _context.Move!.Remove(move);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }

        public async Task<List<MoveDTO>> GetAllMove()
        {
            var move = await _context.Move.Include(m => m.Types).ToListAsync();
            return _mapper.Map<List<MoveDTO>>(move);
        }

        public async Task<MoveDTO> GetMoveById(Guid moveID)
        {
            var move = await _context.Move.Include(m => m.Types).FirstOrDefaultAsync(m => m.moveID == moveID);
            return _mapper.Map<MoveDTO>(move);
        }

        public async Task<bool> UpdateMove(Guid moveID, MoveDTO model)
        {
            if (moveID == model.moveID)
            {
                var updateMove = _mapper.Map<Move>(model);
                _context.Move!.Update(updateMove);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }
    }
}
