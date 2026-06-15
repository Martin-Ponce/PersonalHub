using PersonalHub.Application.Dtos.Security;
using PersonalHub.Application.Helpers;
using System;

namespace PersonalHub.Application.Services
{
    public class SecurityService
    {
        public async Task<GetUserDto> GetUser(string Username, string password)
        {
            var cryptedPassword = CryptUtil.HashPassword(password);
            var usuario = await _usuarioRepo.ObtenerUsuario(u => u.Nombre == nombreUsuario && u.Password == cryptedPassword) ??
                throw TUnauthorizedAccessException.IncorrectCredentials("El nombre de usuario o la contraseña son incorrectos");
            var currentDate = DateTime.UtcNow;
            return new ConsultarUsuarioResponse()
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
            };
        }
    }
}
