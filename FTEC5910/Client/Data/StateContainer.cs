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

        public GetUserResponseDto User 
        {
            get 
            {
                return _user;
            }
            set 
            {
                _user = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
