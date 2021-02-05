using API_Sistema_Central.DTOs;

namespace API_Sistema_Central.Services
{
    public interface ITokenService
    {
        public TokenUtilizadorDTO BuildToken(InfoUtilizadorDTO InfoUtilizadorDTO);
    }
}
