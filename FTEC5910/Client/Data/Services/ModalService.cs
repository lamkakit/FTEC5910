using FTEC5910.Client.Data.Interface;
using FTEC5910.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FTEC5910.Client.Data.Services
{
    public class ModalService : IModalService
    {
        public event Func<string, string, string, string, Task<YesNoModalResult>> YesNoModal;
        public event Func<string, string, string, Task> ConfirmModal;

        public ModalService()
        {
        }

        public async Task<YesNoModalResult> ShowYesNoModal(string title, string msg)
        {
            return await YesNoModal?.Invoke(title, msg, "Yes", "No");
        }

        public async Task ShowConfirmModal(string title, string msg)
        {
            await ConfirmModal?.Invoke(title, msg,"Confirm");
        }

    }
}
