using FirebaseAdmin.Auth;
using RPD_API.DTO.Admin;
using RPD_API.DTO.Trainer;

namespace RPD_API.Service.IService
{
    public interface ITrainersService : IBaseService
    {
        Task<TrainerLoginResponseDTO> LoginAsync(string firebaseIdToken);
        Task<TrainerLoginResponseDTO> GenerateAdminJwt(AdminLoginDTO dto);
        Task<TrainerLoginResponseDTO> RefreshAsync(string refreshToken);
    }
}
