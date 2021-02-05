using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public class UtilizadorService : IUtilizadorService
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;

        public UtilizadorService (UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegistarUtilizador(RegistarUtilizadorDTO registarUtilizadorDTO)
        {
            var user = new Utilizador { Id = registarUtilizadorDTO.Nif, UserName = registarUtilizadorDTO.Email, Nome = registarUtilizadorDTO.Nome, Email = registarUtilizadorDTO.Email, CredencialId = registarUtilizadorDTO.CredencialId };
            var result = await _userManager.CreateAsync(user, registarUtilizadorDTO.Password);
            return result;
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> Login(InfoUtilizadorDTO infoUtilizadorDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(infoUtilizadorDTO.Email, infoUtilizadorDTO.Password, isPersistent: false, lockoutOnFailure: false);
            return result;
        }
    }
}
