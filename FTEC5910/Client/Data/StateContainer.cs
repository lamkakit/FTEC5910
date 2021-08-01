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

        private bool _fullscreen = false;
        public bool Fullscreen
        {
            get { return _fullscreen; }
            set
            {
                if (_fullscreen != value)
                {
                    _fullscreen = value;
                    NotifyStateChanged();
                }
            }
        }

        public string BodyClass
        {
            get { return Fullscreen? "" : "content px-4"; }
        }
        public string BodyStyle
        {
            get { return Fullscreen? "padding-left: unset!important;padding-right:unset!important;" : ""; }
        }
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
