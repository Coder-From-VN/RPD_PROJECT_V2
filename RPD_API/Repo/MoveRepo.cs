using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
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

        public async Task<bool> UpdateMove(Guid moveID, PutMoveDTO model)
        {
            var move = await _context.Move.Include(m => m.Types).FirstOrDefaultAsync(m => m.moveID == moveID);

            if (move != null)
            {
                if (model.moveDamageClass != "")
                    move.moveDamageClass = model.moveDamageClass;
                if (model.moveName != "")
                    move.moveName = model.moveName;
                if (model.movePower != 0)
                    move.movePower = model.movePower;
                if (model.moveAccuracy != 0)
                    move.moveAccuracy = model.moveAccuracy;
                if (model.movePP != 0)
                    move.movePP = model.movePP;
                if (model.movePriority != 0)
                    move.movePriority = model.movePriority;
                if (model.moveDescription != "")
                    move.moveDescription = model.moveDescription;
                if (model.typesID != Guid.Empty)
                    move.typesID = model.typesID;

                _context.Move!.Update(move);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }
    }
}
