﻿using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Elections.Frontend.Repositories;
using Elections.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Elections.Frontend.Pages.VotingStations
{
    [Authorize(Roles = "Admin")]
    public partial class VotingStationCreate
    {
        private VotingStationForm? votingStationForm;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        private VotingStation votingStation = new();
        private readonly String VOTING_STATION_PATH = "api/votingstations";
        private async Task CreateAsync()
        {
            var responseHttp = await Repository.PostAsync(VOTING_STATION_PATH, votingStation);
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
                Position = SweetAlertPosition.Center,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
        }
        private void Return()
        {
            votingStationForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("votingstations");
        }

    }
}
