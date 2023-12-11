﻿using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.KeycloakUtils;

namespace AuthFlowMaui.Shared.Repositories
{
    public interface IApiRepository
    {
        Task<Result> AsignRoleToKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, KeycloakRoleDto keycloakRoleDto, HttpClient httpClient, string clientUuid, CancellationToken cancellationToken);
        Task<Result> DeletKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken);
        Task<Result<KeycloakRoleDto>> GetKeycloakRealmRole(KeycloakRoleDto keycloakRoleDto, KeycloakClientSettings clientSettings, HttpClient httpClient, string clientUuid, string roleName, CancellationToken cancellationToken);
        Task<Result<KeycloakUserDto>> GetKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken);
        Task<Result> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken);
        Task<Result> UpdateKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken);
    }
}