﻿using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services
{
    public interface ICertsService
    {
        Task<MethodDataResult<KeycloakKeyDto>> GetRealmCertsAsync(CancellationToken cancellationToken);
    }
}