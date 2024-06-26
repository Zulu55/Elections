﻿using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Elections.Frontend.Pages.VotingStations;
using Elections.Frontend.Repositories;
using Elections.Frontend.Shared;
using Elections.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Elections.Frontend.Pages.ElectoralPositions
{
    [Authorize(Roles = "Admin")]
    public partial class ElectoralPositionCreate
    {
        private FormWithName<ElectoralPosition>? electoralPositionForm;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        private ElectoralPosition electoralPosition = new();
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        private readonly String ELECTORAL_JOURNEY_PATH = "api/electoralPositions";
        private async Task CreateAsync()
        {
            var responseHttp = await Repository.PostAsync(ELECTORAL_JOURNEY_PATH, electoralPosition);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }
            await BlazoredModal.CloseAsync(ModalResult.Ok());
            Return();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
        }
        private void Return()
        {
            electoralPositionForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("electoralPositions");
        }
    }
}
