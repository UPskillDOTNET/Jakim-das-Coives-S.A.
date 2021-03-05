using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public interface IUtilizadorService
    {
        public Task<IdentityResult> RegistarUtilizador(RegistarUtilizadorDTO registarUtilizadorDTO);
        public Task<Microsoft.AspNetCore.Identity.SignInResult> Login(InfoUtilizadorDTO infoUtilizadorDTO);
        public Task<double> GetSaldoAsync(string nif);
        public Task DepositarSaldoAsync(string nif, double valor);
        public Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        public Task AlterarPasswordAsync(AlterarPasswordDTO alterarPasswordDTO);
    }
}
