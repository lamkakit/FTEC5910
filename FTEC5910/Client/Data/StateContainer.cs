using FTEC5910.Client.Data.Services;
using FTEC5910.Shared.Entities.Dto;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FTEC5910.Client.Data
{
    public class StateContainer
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public StateContainer(IServiceScopeFactory serviceScopeFactory) 
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        private GetUserResponseDto _user;

        public async Task<GetUserResponseDto> GetUser() {
            if (_user == null)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    _user =  await scope.ServiceProvider.GetService<AccountsService>().GetUserInfo();
                    NotifyStateChanged();
                }
            }
            return _user;
        }

        public void ClearUser()
        {
            _user = null;
            NotifyStateChanged();
        }

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
